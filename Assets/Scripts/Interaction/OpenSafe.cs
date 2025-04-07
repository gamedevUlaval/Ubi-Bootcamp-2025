using System;
using System.Collections;
using PlayerControls;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class OpenSafe : NetworkBehaviour, IInteractable 
{
    [SerializeField] private TextMeshProUGUI codeText;
    [SerializeField] private AudioClip beepSound;
    [SerializeField] private AudioClip successSound;
    [SerializeField] private AudioClip wrongSound;
    PlayerInputHandler playerControls;

    [SerializeField] private WindowBehaviour window;
    private string codeTextValue = "";
    [SerializeField] string safeCode;
    [SerializeField] GameObject codePanel;
    [SerializeField] GameObject UIKey;
    private bool codePanelOpen = false;
    

    [Rpc(SendTo.Everyone)]
    private void AddKeyRPC()
    {
        UIKey.SetActive(true);
    }

    public void AddDigit(string digit)
    {
        if (codeTextValue.Length >= 3)
        {
            codeTextValue += digit;

            if (codeTextValue == safeCode)
            {
                StartCoroutine(SoundSuccessRoutineRPC());
                codePanel.SetActive(false);
            }
            else
            {
                SoundManager.Instance.PlaySFX(wrongSound);
                codeTextValue = "";
            }
        }
        else
        {
            SoundManager.Instance.PlaySFX(beepSound);
            codeTextValue += digit;
        }
    }

    public void Interact()
    {
        playerControls = PlayerInputHandler.Instance;
        
        if (window.isBroken)
        {
            codePanel.SetActive(true);
            codePanelOpen = true;
        }
        
        StartCoroutine(CloseCodePanelOnPlayerMovement());
    }

    IEnumerator CloseCodePanelOnPlayerMovement()
    {
        while (codePanelOpen)
        {
            if (codeTextValue == safeCode)
            {
                AddKeyRPC();
                codePanel.SetActive(false);
                codePanelOpen = false;
            }
            
            if (playerControls.MoveInput.magnitude > 0.001f)
            {
                yield return new WaitForSeconds(0.2f);
                codePanel.SetActive(false);
                codePanelOpen = false;
            }
            
            yield return null;
        }
    }

    IEnumerator SoundSuccessRoutineRPC()
    {
        AudioSource musicSource = SoundManager.Instance.musicSource;

        // FADE OUT musique (sur 1s)
        float fadeOutTime = 2f;
        for (float t = 0; t < fadeOutTime; t += Time.deltaTime)
        {
            musicSource.volume = Mathf.Lerp(musicSource.volume, 0f, t / fadeOutTime);
            yield return null;
        }
        musicSource.volume = 0f;

        // Joue le SFX de succès
        SoundManager.Instance.PlaySFX(successSound);

        // Attends la durée du SFX ou une durée fixe (ici 4s)
        yield return new WaitForSeconds(9.5f);

        // FADE IN musique (sur 3s)
        float fadeInTime = 3f;
        for (float t = 0; t < fadeInTime; t += Time.deltaTime)
        {
            musicSource.volume = Mathf.Lerp(0f, 1f, t / fadeInTime);
            yield return null;
        }
        musicSource.volume = 1f;

    }

    public bool InteractWith(GameObject tryToInteractWith)
    {
        return false;
    }

    public InteractableType InteractableType => InteractableType.Static;
}

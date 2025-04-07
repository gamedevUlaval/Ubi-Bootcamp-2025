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
                SoundSuccessRoutineRPC();
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

    void SoundSuccessRoutineRPC()
    {
        SoundManager.Instance.PlaySFX(successSound);
    }

    public bool InteractWith(GameObject tryToInteractWith)
    {
        return false;
    }

    public InteractableType InteractableType => InteractableType.Static;
}

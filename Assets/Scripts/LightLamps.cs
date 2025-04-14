using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class LightLamps : NetworkBehaviour, IInteractable
{
    [SerializeField] private float delay = 10.0f;
    [SerializeField] private GameObject[] lamps;
    private bool isPlaying = false;

    public InteractableType InteractableType => InteractableType.Cooldown;

    [Header("Sound")]

    [SerializeField] private AudioClip lightSound;

    public void Interact()
    {
        StartSequenceRpc();
    }

    public bool InteractWith(GameObject tryToInteractWith)
    {
        throw new System.NotImplementedException();
    }
    [Rpc(SendTo.Everyone, RequireOwnership = false)]
    public void StartSequenceRpc()
    {
        if (!isPlaying)
        {
            SoundManager.Instance.PlayGhostHaunt();
            StartCoroutine(ShowSequence());
        }
    }

    IEnumerator ShowSequence()
    {
        isPlaying = true;
        for (int i = 0; i < lamps.Length; i++)
        {
            SoundManager.Instance.PlaySFX(lightSound);
            lamps[i].SetActive(true);
            yield return new WaitForSeconds(delay);
            lamps[i].SetActive(false);
        }
        isPlaying = false;
    }
}

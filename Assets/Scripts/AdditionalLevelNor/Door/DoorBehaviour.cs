using UnityEngine;
using Unity.Netcode;
using UnityEngine.Serialization;

public class DoorBehaviour : NetworkBehaviour, IInteractable
{
    public AudioClip failureAudioClip;
    public GameObject endScreenGob;

    [Rpc(SendTo.Everyone, RequireOwnership = false)]
    public void OpenDoorRpc()
    {
        SoundManager.Instance.PlayMainTheme();
        SoundManager.Instance.MuteAllSfx();
        endScreenGob.SetActive(true);
    }

    public void Interact()
    {
        KeyManager km = KeyManager.Instance;
        if (km.HasKey(0) && km.HasKey(1) && km.HasKey(2) && km.HasKey(3))
        {
            
            OpenDoorRpc();
        }
        else
        {
            SoundManager.Instance.PlaySFX(failureAudioClip);
        }
    }

    public bool InteractWith(GameObject tryToInteractWith)
    {
        throw new System.NotImplementedException();
    }

    public InteractableType InteractableType => InteractableType.Cooldown;
}

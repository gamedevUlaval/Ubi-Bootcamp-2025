using Unity.Netcode;
using UnityEngine;

public class DoorInteraction : NetworkBehaviour, IInteractable
{
    public GameObject UIKey;
    
    [ContextMenu("Try open door")]
    public void Interact()
    {
        if (UIKey.activeSelf)
        {
            OpenDoorRpc();
        }
    }
    
    [Rpc(SendTo.Everyone)]
    private void OpenDoorRpc()
    {
        UIKey.SetActive(false);
        gameObject.SetActive(false);
    }

    public bool InteractWith(GameObject tryToInteractWith)
    {
        return false;
    }

    public InteractableType InteractableType => InteractableType.Static;
}

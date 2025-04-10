using Unity.Netcode;
using UnityEngine;

public class BlockedDoorInteraction : NetworkBehaviour, IInteractable
{
    [SerializeField] AudioClip lockedDoor;
    [ContextMenu("Try to open blocked door")]
    public void Interact()
    {
        InteractionRpc();
    }
    
    public bool InteractWith(GameObject tryToInteractWith)
    {
        Debug.Log("Should not interact with something in hand");
        return false;
    }
    
    [Rpc(SendTo.Everyone)]
    void InteractionRpc()
    {
        //play sound
        SoundManager.Instance.PlaySFX(lockedDoor);
        print("tried to open blocked door");
    }
    
    public InteractableType InteractableType => InteractableType.Static;
}

using Unity.Netcode;
using UnityEngine;

public class BlockedDoorInteraction : NetworkBehaviour, IInteractable
{
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
        print("tried to open blocked door");
    }
    
    public InteractableType InteractableType => InteractableType.Static;
}

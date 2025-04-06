using Unity.Netcode;
using UnityEngine;

public class SafeWithoutCombinationInteraction : NetworkBehaviour, IInteractable
{
    
    [ContextMenu("Open chest")]
    public void Interact()
    {
        AddKeyRPC();
    }

    public bool InteractWith(GameObject tryToInteractWith)
    {
        throw new System.NotImplementedException();
    }
    
    [Rpc(SendTo.Everyone, RequireOwnership = false)]
    private void AddKeyRPC()
    {
        KeyManager.Instance.AddKey(3);
    }

    public InteractableType InteractableType => InteractableType.Static;
}

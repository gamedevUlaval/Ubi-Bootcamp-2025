using Unity.Netcode;
using UnityEngine;

public class SafeWithoutCombinationInteraction : NetworkBehaviour, IInteractable
{
    public GameObject UIKey;
    
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
        UIKey.SetActive(true);
    }

    public InteractableType InteractableType => InteractableType.Static;
}

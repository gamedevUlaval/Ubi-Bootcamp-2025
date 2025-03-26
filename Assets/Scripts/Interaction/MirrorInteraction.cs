using UnityEngine;

public class MirrorInteraction : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log("Interacting with mirror");
        
        Debug.Log("change mirror texture");
    }
    
    public bool InteractWith(GameObject tryToInteractWith)
    {
        Debug.Log("Should not interact with something in hand");
        return false;
    }
    
    public InteractableType InteractableType => InteractableType.Static;
}

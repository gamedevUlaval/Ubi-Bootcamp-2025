using UnityEngine;

public class ThrowableInteraction : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        print("took the object");
    }
    
    public bool InteractWith(GameObject tryToInteractWith)
    {
        Debug.Log("Should not interact with something in hand");
        return false;
    }
    
    public InteractableType InteractableType => InteractableType.Pickable;
}

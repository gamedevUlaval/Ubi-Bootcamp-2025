using UnityEngine;

public class BlockedDoorInteraction : MonoBehaviour, IInteractable
{
    [ContextMenu("Try to open blocked door")]
    public void Interact()
    {
        //play sound
        print("tried to open blocked door");
    }
    
    public bool InteractWith(GameObject tryToInteractWith)
    {
        Debug.Log("Should not interact with something in hand");
        return false;
    }
    
    public InteractableType InteractableType => InteractableType.Static;
}

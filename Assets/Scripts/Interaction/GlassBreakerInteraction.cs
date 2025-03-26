using UnityEngine;

public class GlassBreakerInteraction : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log("Interacting with glassBreaker");
        
        Debug.Log("pickup glassBreaker");
    }
    
    public bool InteractWith(GameObject tryToInteractWith)
    {
        Debug.Log("Should not interact with something in hand");
        return false;
    }
    
    public InteractableType InteractableType => InteractableType.Pickable;
}

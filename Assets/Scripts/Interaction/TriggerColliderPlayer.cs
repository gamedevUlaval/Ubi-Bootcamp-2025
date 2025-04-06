using UnityEngine;


public enum PlayerType
{
    Human,
    Ghost
}

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(InteractableObject))]
public class TriggerColliderPlayer : MonoBehaviour
{
    [SerializeField] PlayerType playerType;
    InteractableObject _interactableObject;
    
    void Awake()
    {
        _interactableObject = GetComponent<InteractableObject>();
    }
    
    void OnValidate()
    {
        var objectCollider = GetComponent<Collider>();
        if (!objectCollider.isTrigger)
        {
            objectCollider.isTrigger = true;
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        PlayerInteraction playerInteraction = other.GetComponent<PlayerInteraction>();
        if (other.CompareTag(GetTagForPlayerType()) && playerInteraction != null)
        {
            _interactableObject.SetPlayerNearby(true);
            playerInteraction.AddNearbyInteractableObject(_interactableObject);
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        PlayerInteraction playerInteraction = other.GetComponent<PlayerInteraction>();
        if (other.CompareTag(GetTagForPlayerType()))
        {
            _interactableObject.SetPlayerNearby(false);
            _interactableObject.HideWhiteDot();
            playerInteraction.RemoveNearbyInteractableObject(_interactableObject);
        }
    }
    
    string GetTagForPlayerType()
    {
        return playerType == PlayerType.Human ? "Human" : "Ghost";
    }
}

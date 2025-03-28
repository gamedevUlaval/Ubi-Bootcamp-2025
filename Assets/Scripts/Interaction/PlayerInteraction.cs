using System.Collections.Generic;
using PlayerControls;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] float maxInteractionDistance = 5f;
    [SerializeField] float angleOfDetection = 0.5f;
    [SerializeField] Transform playerHead;
    
    [SerializeField] LayerMask interactableLayer;
    
    [SerializeField] List<InteractableObject> nearbyInteractableObjects = new List<InteractableObject>();
    InteractableObject _currentTarget = null;

    public static PlayerInteraction Instance;
    
    void Awake()
    {
        Instance = this;
    }
    
    public void AddNearbyInteractableObject(InteractableObject interactableObject)
    {
        if (!nearbyInteractableObjects.Contains(interactableObject))
        {
            nearbyInteractableObjects.Add(interactableObject);
        }
    }
    
    public void RemoveNearbyInteractableObject(InteractableObject interactableObject)
    {
        if (nearbyInteractableObjects.Contains(interactableObject))
        {
            nearbyInteractableObjects.Remove(interactableObject);
        }
        
        if (_currentTarget == interactableObject)
        {
            ClearCurrentTarget();
        }
    }
    
    void ClearCurrentTarget()
    {
        if (_currentTarget is not null)
        {
            _currentTarget.HidePrompt();
            _currentTarget = null;
        }
    }

    void FixedUpdate()
    {
        UpdateInteractions();
            
        if (PlayerInputHandler.Instance.InteractInput && _currentTarget is not null)
        {
            _currentTarget.Interact();
            _currentTarget.HidePrompt();
            _currentTarget.HideWhiteDot();
            RemoveNearbyInteractableObject(_currentTarget);
            
            _currentTarget = null;
        }
    }
    
    void UpdateInteractions()
    {
        bool foundTarget = false;
        
        foreach (var obj in nearbyInteractableObjects)
        {
            Vector3 directionToObject = obj.transform.position - playerHead.position;
            float angleBetweenVisionAndObjectDirection = Vector3.Dot(playerHead.forward, directionToObject.normalized);
            
            if (angleBetweenVisionAndObjectDirection > angleOfDetection)
            {
                bool isLookingDirectly = IsLookingDirectlyAt(obj);
                
                if (isLookingDirectly)
                {
                    if (_currentTarget != obj)
                    {
                        _currentTarget?.HidePrompt();
                        _currentTarget = obj;
                        _currentTarget.ShowPrompt();
                    }
                    
                    //print("looking directly");
                    obj.HideWhiteDot();
                    foundTarget = true;
                }
                else
                {
                    //print("not looking directly");
                    obj.ShowWhiteDot();
                }
            }
            else
            {
                //print("not looking in direction");
                obj.HideWhiteDot();
            }
        }

        if (!foundTarget)
        {
            ClearCurrentTarget();
        }
    }
    
    bool IsLookingDirectlyAt(InteractableObject obj)
    {
        Debug.DrawRay(playerHead.position, playerHead.forward * maxInteractionDistance, Color.green);
        
        Ray ray = new Ray(playerHead.position, playerHead.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, maxInteractionDistance, interactableLayer))
        {
            return hit.collider.gameObject == obj.transform.parent.gameObject;
        }
        
        return false;
    }
}

using System.Collections.Generic;
using System.Linq;
using PlayerControls;
using Unity.Netcode;
using UnityEngine;

public class PlayerInteraction : NetworkBehaviour
{
    [SerializeField] float maxInteractionDistance = 10f;
    [SerializeField] float angleOfDetection = 0.5f;
    [SerializeField] Transform playerHead;
    
    [SerializeField] LayerMask interactableLayer;
    
    [SerializeField] List<InteractableObject> nearbyInteractableObjects = new List<InteractableObject>();
    InteractableObject _currentTarget = null;
    
    public void AddNearbyInteractableObject(InteractableObject interactableObject)
    {
        if (!nearbyInteractableObjects.Contains(interactableObject))
        {
            if (!HasAuthority)
            {
                return;
            }
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
        // if (!HasAuthority)
        //     return;
        
        UpdateInteractions();
            
        if (PlayerInputHandler.Instance.InteractInput && _currentTarget is not null)
        {
            //Debug.Log("Trigger interaction");
            _currentTarget.Interact();
            if (_currentTarget.GetInteractableType() == InteractableType.Cooldown)
            {
                return;
            }
            _currentTarget.HidePrompt();
            _currentTarget.HideWhiteDot();
            RemoveNearbyInteractableObject(_currentTarget);
            
            _currentTarget = null;
        }
    }
    
    void UpdateInteractions()
    {
        bool foundTarget = false;
        nearbyInteractableObjects = nearbyInteractableObjects.Where(obj => obj != null).ToList();
        foreach (var obj in nearbyInteractableObjects)
        {
            
            Vector3 directionToObject = obj.transform.position - playerHead.position;
            float angleBetweenVisionAndObjectDirection = Vector3.Dot(playerHead.forward, directionToObject.normalized);
            Debug.DrawRay(playerHead.position, directionToObject, Color.red);
            float newDistance = (obj.transform.position - transform.position).magnitude;
            //Debug.Log(newDistance);
            if (angleBetweenVisionAndObjectDirection > angleOfDetection || newDistance < maxInteractionDistance)
            {
                bool isLookingDirectly = IsLookingDirectlyAt(obj);
                
                if (isLookingDirectly || newDistance < maxInteractionDistance)
                {
                    if (_currentTarget != obj)
                    {
                        _currentTarget?.HidePrompt();
                        _currentTarget = obj;
                        _currentTarget.ShowPrompt();
                    }
                    
                    //Debug.Log("looking directly");
                    obj.ShowWhiteDot();
                    foundTarget = true;
                }
                else
                {
                    //Debug.Log("not looking directly");
                    obj.ShowWhiteDot();
                }
            }
            else
            {
                //Debug.Log("not looking in direction");
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

using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] float maxInteractionDistance = 5f;
    [SerializeField] float angleOfDetection = 45f;
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

        if (Input.GetKeyDown(KeyCode.F) && _currentTarget is not null)//refactor with input system
        {
            
            _currentTarget = null;
        }
    }
    
    void UpdateInteractions()
    {
        bool foundTarget = false;
        
        foreach (var obj in nearbyInteractableObjects)
        {
            //Debug.DrawRay(playerHead.position, obj.transform.position - playerHead.position, Color.red);
            Vector3 directionToObject = obj.transform.position - playerHead.position;
            float angleBetweenVisionAndObjectDirection = Vector3.Angle(playerHead.forward, directionToObject.normalized);
            
            if (angleBetweenVisionAndObjectDirection < angleOfDetection)
            {
                bool isLookingDirectly  = IsLookingDirectlyAt(obj);
                
                if (isLookingDirectly)
                {
                    if (_currentTarget != obj)
                    {
                        _currentTarget?.HidePrompt();
                        _currentTarget = obj;
                        _currentTarget.ShowPrompt();
                    }
                    
                    obj.HideWhiteDot();
                    foundTarget = true;
                }
                else
                {
                    obj.ShowWhiteDot();
                }
            }
            else
            {
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
        Ray ray = new Ray(playerHead.position, obj.transform.position - playerHead.position);
        if (Physics.Raycast(ray, out RaycastHit hit, maxInteractionDistance, interactableLayer))
        {
            return hit.collider.gameObject == obj.gameObject;
        }
        
        return false;
    }
}

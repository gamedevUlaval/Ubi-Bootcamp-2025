using UnityEngine;
using UnityEngine.Serialization;

public class InteractionManager : MonoBehaviour
{
    [SerializeField] private Transform playerEyes;
    
    private float maxDetectionDistance = 3;
    private GameObject currentObject;
    
    private void Update()
    {
        if (IsInteractableReachable())
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                currentObject.GetComponent<Door>().Open();
            }
        }
        
        Debug.DrawRay(playerEyes.position + playerEyes.forward.normalized * 0.2f, playerEyes.forward * maxDetectionDistance, Color.green);
    }
    
    private bool IsInteractableReachable()
    {
        if (Physics.Raycast(playerEyes.position + playerEyes.forward.normalized * 0.2f,
                playerEyes.forward, out var hitInfo,
                maxDetectionDistance))
        {
            GameObject hitObject = hitInfo.collider.gameObject;
            
            Door door = hitObject.GetComponent<Door>();
            
            if (!(hitInfo.distance <= maxDetectionDistance) || door is null) return false;
            currentObject = hitObject;

            return true;
        }
        
        return false;
    }
}

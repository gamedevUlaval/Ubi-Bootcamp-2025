using UnityEngine;
using TMPro;

public class InteractableObject : MonoBehaviour
{
    [Header("Item Data")]
    [SerializeField] InteractableItemData interactableItemData;
    
    [Header("UI Elements")]
    [SerializeField] Canvas whiteDotCanvas;
    [SerializeField] Canvas promptCanvas;
    [SerializeField] TMP_Text itemNameText;
    [SerializeField] TMP_Text actionText;
    
    IInteractable interactable;
    
    bool _isPlayerNearby = false;
    public bool IsPlayerNearby => _isPlayerNearby;
    
    void Awake()
    {
        if (transform.parent == null || transform.parent.GetComponent<IInteractable>() == null)
        {
            Debug.LogError("Parents gameobject of : " + gameObject.name + " must have a component that implements IInteractable interface");
        }
    }
    
    void Start()
    {
        SetCanvasState(whiteDotCanvas, false);
        //SetCanvasState(promptCanvas, false);
        interactable = GetComponentInParent<IInteractable>();
    }
    
    public void Interact()
    {
        interactable.Interact();
    }

    public InteractableType GetInteractableType()
    {
        return interactable.InteractableType;
    }
    
    public bool InteractWith(GameObject tryToInteractWith)
    {
        return interactable.InteractWith(tryToInteractWith);
    }

    void SetCanvasState(Canvas canvas, bool state)
    {
        if (canvas?.gameObject.activeSelf != state)
        {
            canvas?.gameObject.SetActive(state);
        }
    }
    
    public void ShowWhiteDot()
    {
        SetCanvasState(whiteDotCanvas, true);
    }
    
    public void HideWhiteDot()
    {
        SetCanvasState(whiteDotCanvas, false);
    }
    
    public void SetPlayerNearby(bool isNearby)
    {
        _isPlayerNearby = isNearby;
        //SetCanvasState(promptCanvas, isNearby);
    }

    public void HidePrompt()
    {
        //SetCanvasState(promptCanvas, false);
        if (_isPlayerNearby)
        {
            ShowWhiteDot();
        }
    }

    public void ShowPrompt()
    {
        HideWhiteDot();
        //SetCanvasState(promptCanvas, true);
        
        //itemNameText.text = interactableItemData.itemName;
        //actionText.text = interactableItemData.interactionPrompt;
    }
}

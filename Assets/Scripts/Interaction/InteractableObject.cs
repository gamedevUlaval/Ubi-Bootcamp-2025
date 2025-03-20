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
    
    bool _isPlayerNearby = false;
    public bool IsPlayerNearby => _isPlayerNearby;
    
    void Start()
    {
        SetCanvasState(whiteDotCanvas, false);
        //SetCanvasState(promptCanvas, false);
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

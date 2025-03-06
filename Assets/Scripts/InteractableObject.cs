using TMPro;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [Header("Item Data")]
    [SerializeField] private InteractableItemData itemData;

    [Header("UI Elements")]
    //[SerializeField] private Canvas whiteDotCanvas;
    [SerializeField] private Canvas promptCanvas;
    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private TMP_Text actionText;

    private bool isPlayerNearby = false;

    public bool IsPlayerNearby => isPlayerNearby;

    private void Start()
    {
        SetCanvasState(promptCanvas, false);
    }

    public void ShowPrompt()
    {
        SetCanvasState(promptCanvas, true);
    }

    public void HidePrompt()
    {
        SetCanvasState(promptCanvas, false);
    }

    public void SetPlayerNearby(bool isNearby)
    {
        isPlayerNearby = isNearby;
        if (!isNearby)
        {
            HidePrompt();
        }
    }
    
    void SetCanvasState(Canvas canvas, bool state)
    {
        if (canvas != null && canvas.gameObject.activeSelf != state)
        {
            canvas.gameObject.SetActive(state);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            ShowPrompt();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        HidePrompt();
    }
}

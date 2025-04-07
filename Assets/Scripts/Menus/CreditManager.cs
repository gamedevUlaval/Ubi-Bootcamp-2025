using UnityEngine;
using UnityEngine.UI;

public class CreditManager : MonoBehaviour
{
    [SerializeField] private GameObject optionButtonGob;
    private Button audioButton;

    private void Awake()
    {
        audioButton = optionButtonGob.GetComponent<Button>();
        audioButton.onClick.AddListener(OnOptionButtonClicked);
    }
    
    private void OnOptionButtonClicked()
    {
        this.gameObject.SetActive(false);
    }
}

using UnityEngine;
using UnityEngine.UI;

namespace menus
{
    public class OptionMenuManager : MonoBehaviour
    {
        [SerializeField] GameObject audioButtonGob;
        Button audioButton;
        [SerializeField] GameObject gameplayButtonGob;
        Button gameplayButton;
        [SerializeField] GameObject backButtonGob;
        Button backButton;
        
        [SerializeField] GameObject audioWindow;
        [SerializeField] GameObject gameplayWindow;
        [SerializeField] GameObject mainMenu;

        void Awake()
        {
            audioButton = audioButtonGob.GetComponent<Button>();
            audioButton.onClick.AddListener(OnAudioButtonClicked);
            
            gameplayButton = gameplayButtonGob.GetComponent<Button>();
            gameplayButton.onClick.AddListener(OnGameplayButtonClicked);
            
            backButton = backButtonGob.GetComponent<Button>();
            backButton.onClick.AddListener(OnBackButtonClicked);
            
            audioWindow.SetActive(false);
            gameplayWindow.SetActive(false);
        }
        
        void OnAudioButtonClicked()
        {
            disableSideWindows();
            audioWindow.SetActive(true);
        }
        
        void OnGameplayButtonClicked()
        {
            disableSideWindows();
            gameplayWindow.SetActive(true);
        }
        
        void OnBackButtonClicked()
        {
            mainMenu.SetActive(true);
            gameObject.SetActive(false);
            
            audioWindow.SetActive(false);
            gameplayWindow.SetActive(false);
        }
        
        void disableSideWindows()
        {
            audioWindow.SetActive(false);
            gameplayWindow.SetActive(false);
        }
    }  
}

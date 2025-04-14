using UnityEngine;
using UnityEngine.UI;

 namespace menus
 {
     public class MainMenuManager : MonoBehaviour
     {
         [SerializeField] GameObject menus;
         [SerializeField] GameObject mainMenu;
         [SerializeField] GameObject optionsMenu;
         [SerializeField] GameObject creditsMenu;
         
         [SerializeField] GameObject playButtonGob;
         [SerializeField] GameObject optionsButtonGob;
         [SerializeField] GameObject quitButtonGob;
         [SerializeField] GameObject creditsButtonGob;
         
         [SerializeField] GameObject loadingScreen;
         [SerializeField] LoadingScreenManager loadingScreenManager; 
     
         void Awake()
         {
             Button playButton = playButtonGob.GetComponent<Button>();
             Button optionsButton = optionsButtonGob.GetComponent<Button>();
             Button quitButton = quitButtonGob.GetComponent<Button>();
             Button creditsButton = creditsButtonGob.GetComponent<Button>();
             
             playButton.onClick.AddListener(OnPlayButtonClicked);
             optionsButton.onClick.AddListener(OnOptionsButtonClicked);
             quitButton.onClick.AddListener(OnQuitButtonClicked);
             creditsButton.onClick.AddListener(OnCreditsButtonClicked);
             
             loadingScreen.SetActive(false);
         }
         
         void OnPlayButtonClicked()
         {
             loadingScreen.SetActive(true);
             deactivateButtons();
             loadingScreenManager.LoadMainScene();
             menus.SetActive(false);
         }
         
         void OnOptionsButtonClicked()
         {
             optionsMenu.SetActive(true);
             mainMenu.SetActive(false);
             creditsMenu.SetActive(false);
         }
         
         void OnCreditsButtonClicked()
         {
             optionsMenu.SetActive(false);
             creditsMenu.SetActive(true);
         }
         
         void OnQuitButtonClicked()
         {
             Application.Quit();
         }
         
         void OnEnable()
         {
             activateButtons();
         }
         
         void deactivateButtons()
         {
             playButtonGob.SetActive(false);
             optionsButtonGob.SetActive(false);
             quitButtonGob.SetActive(false);
             creditsButtonGob.SetActive(false);
         }
         
         void activateButtons()
        {
            playButtonGob.SetActive(true);
            optionsButtonGob.SetActive(true);
            quitButtonGob.SetActive(true);
            creditsButtonGob.SetActive(true);
        }
     }
 }

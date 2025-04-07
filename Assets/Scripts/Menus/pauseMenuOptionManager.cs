using UnityEngine;

public class pauseMenuOptionManager : MonoBehaviour
{
    [SerializeField] private GameObject buttons;
    
    [SerializeField] private GameObject audioMenuWindow;
    [SerializeField] private GameObject graphicsMenuWindow;
    
    private void Awake()
    {
        closeMenu();
    }
    
    public void closeMenu()
    {
        audioMenuWindow.SetActive(false);
        graphicsMenuWindow.SetActive(false);
        
        buttons.SetActive(true);
    }
    
    public void openAudioMenuWindow()
    {
        audioMenuWindow.SetActive(true);
        graphicsMenuWindow.SetActive(false);
        
        buttons.SetActive(false);
    }
    
    public void openGraphicsMenuWindow()
    {
        audioMenuWindow.SetActive(false);
        graphicsMenuWindow.SetActive(true);
        
        buttons.SetActive(false);
    }
}

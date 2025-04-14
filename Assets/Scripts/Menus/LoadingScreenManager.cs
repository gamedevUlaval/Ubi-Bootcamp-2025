using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreenManager : MonoBehaviour
{
    [SerializeField] GameObject progressBarGob;
    Slider progressBar;
    [SerializeField] GameObject percentageTextGob;
    TMP_Text percentageText;
    [SerializeField] AudioListener audioListener;
    
    void Awake()
    {
        progressBar = progressBarGob.GetComponent<Slider>();
        percentageText = percentageTextGob.GetComponent<TMP_Text>();
    }
    
    public void LoadMainScene()
    {
        audioListener.enabled = false;
        //StartCoroutine(LoadAsynchronously());
    }
    
    // IEnumerator LoadAsynchronously()
    // {
    //     AsyncOperation scene1 = SceneManager.LoadSceneAsync("Scenes/", LoadSceneMode.Additive); //TODO change scene name
    //     //AsyncOperation scene2 = SceneManager.LoadSceneAsync("Scenes/Visual_Forest", LoadSceneMode.Additive);
    //     
    //     while (!scene1.isDone) //|| !scene2.isDone)
    //     {
    //         float progress = Mathf.Clamp01(scene1.progress / .9f);
    //         progressBar.value = progress;
    //         percentageText.text = progress * 100f + "%";
    //         
    //         yield return null;
    //     }
    //     
    //     SceneManager.SetActiveScene(SceneManager.GetSceneByName(""));
    // }
}
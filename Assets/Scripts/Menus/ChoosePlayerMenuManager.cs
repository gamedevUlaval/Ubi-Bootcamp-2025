using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChoosePlayerMenuManager : MonoBehaviour
{
    [SerializeField] GameObject sessionNameGob;
    TMP_InputField sessionNameInput;
    [SerializeField] GameObject humanButtonGob;
    Button humanButton;
    [SerializeField] GameObject ghostButtonGob;
    Button ghostButton;
    [SerializeField] GameObject playButtonGob;
    Button playButton;
    
    void Awake()
    {
        sessionNameInput = sessionNameGob.GetComponent<TMP_InputField>();
        sessionNameInput.onEndEdit.AddListener(OnSessionNameInput);
            
        humanButton = humanButtonGob.GetComponent<Button>();
        humanButton.onClick.AddListener(OnHumanButtonClicked);
            
        ghostButton = ghostButtonGob.GetComponent<Button>();
        ghostButton.onClick.AddListener(OnGhostButtonClicked);
        
        playButton = playButtonGob.GetComponent<Button>();
        playButton.onClick.AddListener(OnPlayButtonClicked);
        playButtonGob.SetActive(false);
        StartCoroutine(LoadAsynchronously());
    }
    
    IEnumerator LoadAsynchronously()
    {
        yield return SceneManager.LoadSceneAsync("PlayerScene", LoadSceneMode.Additive);
    }
    
    IEnumerator UnloadAsynchronously()
    {
        yield return SceneManager.UnloadSceneAsync("MenuScene");
    }

    void OnSessionNameInput(string text)
    {
        ConnectionManager.Instance.SessionName = text;
    }

    void OnHumanButtonClicked()
    {
        ConnectionManager.Instance.ProfileName = $"Human{Environment.UserName+Environment.MachineName}";
        playButtonGob.SetActive(true);
    }

    void OnGhostButtonClicked()
    {
        ConnectionManager.Instance.ProfileName = $"Ghost{Environment.UserName+Environment.MachineName}";
        playButtonGob.SetActive(true);
    }

    void OnPlayButtonClicked()
    {
        ConnectionManager.Instance.CreateOrJoinSessionAsync();
        StartCoroutine(UnloadAsynchronously());
    }
}

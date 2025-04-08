using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SceneLoader : MonoBehaviour
{
    public float cameraTweenDuration = 1f;
    
    Dictionary<string, bool> loadedScenes = new Dictionary<string, bool>();
    Room currentRoom = null;

    void Start()
    {
        loadedScenes.Add(SceneManager.GetActiveScene().name, true);
        currentRoom = DetectCurrentRoom();

        if (currentRoom is null)
        {
            Debug.LogError("No room detected where the player is standing at start.");
        }
        
        LoadAdjacentRooms(currentRoom);
        loadedScenes[currentRoom.SceneName] = true;
    }

    void OnTriggerEnter(Collider other)
    {
        Room detectedRoom = other.GetComponent<Room>();
        if (detectedRoom == null)
            return;
        
        if (detectedRoom.SceneName == currentRoom.SceneName)
            return;
        
        LoadAdjacentRooms(detectedRoom);
        
        UnloadNonAdjacentRooms(detectedRoom, currentRoom);
        
        Transform cameraTargetPosition = detectedRoom.CameraTargetPosition;
        Camera.main.transform.DOMove(cameraTargetPosition.position, cameraTweenDuration);
        
        currentRoom = detectedRoom;
    }
    
    void LoadScene(string sceneName)
    {
        if (loadedScenes.ContainsKey(sceneName) && loadedScenes[sceneName])
            return;
        
        StartCoroutine(LoadSceneAsync(sceneName));
    }
    
    void UnloadScene(string sceneName)
    {
        if (loadedScenes.ContainsKey(sceneName) && !loadedScenes[sceneName])
            return;

        StartCoroutine(UnloadSceneAsync(sceneName));
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        yield return new WaitUntil(() => asyncLoad.isDone);
        loadedScenes[sceneName] = true;
    }

    IEnumerator UnloadSceneAsync(string sceneName)
    {
        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(sceneName);
        yield return new WaitUntil(() => asyncUnload.isDone);
        loadedScenes[sceneName] = false;
    }
    
    Room DetectCurrentRoom()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 0.1f);
        return colliders.Select(collider => collider.GetComponent<Room>()).FirstOrDefault(room => room != null);
    }
    
    void LoadAdjacentRooms(Room room)
    {
        foreach (var sceneName in room.AdjacentRooms)
        {
            LoadScene(sceneName);
        }
    }

    void UnloadNonAdjacentRooms(Room detectedRoom, Room currentRoom)
    {
        foreach (var sceneName in currentRoom.AdjacentRooms)
        {
            if (sceneName == detectedRoom.SceneName || sceneName == currentRoom.SceneName)
                continue;
            
            UnloadScene(sceneName);
        }
    }
}

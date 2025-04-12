using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using NUnit.Framework;
using RoomLoading;
using Unity.Netcode;

public class SceneLoader : NetworkBehaviour
{
    public UnityEditor.SceneAsset StartingScene;
    public float cameraTweenDuration = 1f;
    
    Dictionary<string, Scene?> loadedScenes = new Dictionary<string, Scene?>();
    Dictionary<string, AsyncOperation> loadingScenes = new Dictionary<string, AsyncOperation>();
    Dictionary<string, AsyncOperation> unloadingScenes = new Dictionary<string, AsyncOperation>();
    Dictionary<ulong, string> occupiedRoomNames = new Dictionary<ulong, string>();
    Room currentRoom = null;

    public override void OnNetworkSpawn()
    {
        if (!HasAuthority)
        {
            Destroy(this);
            return;
        }

        NetworkManager.SceneManager.OnLoad += OnLoad;
        NetworkManager.SceneManager.OnUnload += OnUnload;
        NetworkManager.SceneManager.OnLoadComplete += OnLoadComplete;
        NetworkManager.SceneManager.OnUnloadComplete += OnUnloadComplete;
        
        if (IsSessionOwner)
        {
            NetworkManager.SceneManager.OnSceneEvent += OnSceneEvent;           
        }
        
        currentRoom = DetectCurrentRoom();

        if (currentRoom is null)
        {
            Debug.Log("No room detected where the player is standing at start.");
            if (IsSessionOwner)
            {
                StartCoroutine(LoadInitialSceneAsync(StartingScene.name));
            }
            else
            {
                StartCoroutine(WaitForInitialSceneAsync(StartingScene.name));
            }
        }
        else
        {
            occupiedRoomNames[NetworkManager.Singleton.LocalClientId] = currentRoom.SceneName;
            LoadRoomsRpc(new NetworkStringArray(){Array = currentRoom.AdjacentRooms});
        }
    }
    
    IEnumerator LoadInitialSceneAsync(string sceneName)
    {
        LoadScene(sceneName);
        yield return loadingScenes[sceneName];
        currentRoom = DetectCurrentRoom();
        occupiedRoomNames[NetworkManager.Singleton.LocalClientId] = currentRoom.SceneName;
        LoadRoomsRpc(new NetworkStringArray(){Array = currentRoom.AdjacentRooms});
    }

    IEnumerator WaitForInitialSceneAsync(string sceneName)
    {
        if (loadingScenes.TryGetValue(sceneName, out var sceneLoaded))
        {
            yield return sceneLoaded;
            currentRoom = DetectCurrentRoom();
            occupiedRoomNames[NetworkManager.Singleton.LocalClientId] = currentRoom.SceneName;
        }
        else
        {
            Debug.LogError("Initial scene is not loading");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Room detectedRoom = other.GetComponent<Room>();
        if (detectedRoom == null)
            return;
        if (currentRoom == null)
        {
            currentRoom = detectedRoom;
            occupiedRoomNames[NetworkManager.Singleton.LocalClientId] = currentRoom.SceneName;
        }
        
        if (occupiedRoomNames[NetworkManager.Singleton.LocalClientId] == detectedRoom.SceneName)
            return;
        
        LoadRoomsRpc(new NetworkStringArray(){Array = detectedRoom.AdjacentRooms});
        
        UnloadNonAdjacentRoomsRpc(detectedRoom.SceneName, new NetworkStringArray() {Array = currentRoom.AdjacentRooms});
        
        Transform cameraTargetPosition = detectedRoom.CameraTargetPosition;
        Camera.main.transform.DOMove(cameraTargetPosition.position, cameraTweenDuration);
        Camera.main.transform.DORotate(cameraTargetPosition.rotation.eulerAngles, cameraTweenDuration);
        
        currentRoom = detectedRoom;
    }
    void LoadScene(string sceneName)
    {
        if (loadingScenes.ContainsKey(sceneName) || loadedScenes.ContainsKey(sceneName))
            return;
        
        var status = NetworkManager.SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        if (status != SceneEventProgressStatus.Started)
        {
            Debug.LogWarning("Scene " + sceneName + " could not be loaded because " + status);
        }
    }
    
    void UnloadScene(string sceneName)
    {
        Scene? sceneToUnload = loadedScenes[sceneName];
        if (!sceneToUnload.HasValue || unloadingScenes.ContainsKey(sceneName))
            return;

        var status = NetworkManager.SceneManager.UnloadScene(sceneToUnload.Value);
        if (status != SceneEventProgressStatus.Started)
        {
            Debug.LogWarning("Scene " + sceneName + " could not be unloaded because " + status);
        }
    }
    
    Room DetectCurrentRoom()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 0.1f);
        return colliders.Select(collider => collider.GetComponent<Room>()).FirstOrDefault(room => room != null);
    }
    
    [Rpc(SendTo.Everyone)]
    void LoadRoomsRpc(NetworkStringArray rooms)
    {
        if (!IsSessionOwner)
            return;
        foreach (var sceneName in rooms.Array)
        {
            LoadScene(sceneName);
        }
    }

    [Rpc(SendTo.Everyone)]
    void UnloadNonAdjacentRoomsRpc(string detectedRoomName, NetworkStringArray currentRoomAdjacentRooms, RpcParams rpcParams = default)
    {
        occupiedRoomNames[rpcParams.Receive.SenderClientId] = detectedRoomName;
        if (!IsSessionOwner || !occupiedRoomNames.ContainsValue(detectedRoomName)) 
            // Only unload when both players are in the same room
        {
            return;
        }

        foreach (var sceneName in currentRoomAdjacentRooms.Array)
        {
            if (sceneName == detectedRoomName || sceneName == currentRoom.SceneName)
                continue;
            
            UnloadScene(sceneName);
        }
    }

    void OnSceneEvent(SceneEvent sceneEvent)
    {
        if (sceneEvent.SceneEventType == SceneEventType.LoadComplete)
        {
            loadedScenes[sceneEvent.SceneName] = sceneEvent.Scene;
        }
    }
    
    void OnLoad(ulong clientId, string sceneName, LoadSceneMode loadSceneMode, AsyncOperation asyncOperation)
    {
        loadingScenes[sceneName] = asyncOperation;
    }
    
    void OnUnload(ulong clientId, string sceneName, AsyncOperation asyncOperation)
    {
        unloadingScenes[sceneName] = asyncOperation;
    }

    void OnLoadComplete(ulong clientId, string sceneName, LoadSceneMode loadSceneMode)
    {
        loadingScenes.Remove(sceneName);
        loadedScenes.TryAdd(sceneName, null);
    }
    
    void OnUnloadComplete(ulong clientId, string sceneName)
    {
        unloadingScenes.Remove(sceneName);
        loadedScenes.Remove(sceneName);
    }
}

using System.Linq;
using PlayerControls;
using PlayerMovement;
using Unity.Multiplayer.Playmode;
using Unity.Netcode;
using Unity.Netcode.Components;
using Unity.Netcode.Editor;
using UnityEngine;
using UnityEngine.Serialization;
public class PlayerSpawner : NetworkBehaviour
{
    public Vector3 spawnPosition;
    void Start()
    {
        if (!HasAuthority)
        {
            return;
        }
        transform.position = spawnPosition;
        
        GameObject prefab;
        if (!CurrentPlayer.ReadOnlyTags().Contains("INIT_GHOST") && (NetworkManager.ConnectedClients.Count == 1 || CurrentPlayer.ReadOnlyTags().Contains("INIT_HUMAN")))
        {
            prefab = Resources.Load<GameObject>("Human");
            transform.position += new Vector3(3, 0, 0);
        }
        else
        {
            prefab = Resources.Load<GameObject>("Ghost");
        }
        Debug.Log("Trying to spawn local game object");
        var instance = Instantiate(prefab, transform);
        Debug.Log("Trying to spawn get network object");
        var instanceNetworkObject = instance.GetComponent<NetworkObject>();
        Debug.Log("Trying to spawn game object on the network");
        instanceNetworkObject.Spawn();
        Debug.Log("Managed to spawn" + prefab.name);
    }
    
}

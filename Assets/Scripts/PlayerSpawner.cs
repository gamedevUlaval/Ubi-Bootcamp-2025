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
        var instance = Instantiate(prefab, transform);
        var instanceNetworkObject = instance.GetComponent<NetworkObject>();
        instanceNetworkObject.Spawn();
        instanceNetworkObject.RequestOwnership();
        instanceNetworkObject.SetOwnershipLock(true);
        Debug.Log($"Has ownership over {prefab.name}");
    }
    
}

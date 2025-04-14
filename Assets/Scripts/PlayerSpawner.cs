using System.Linq;
using Unity.Multiplayer.Playmode;
using Unity.Netcode;
using UnityEngine;
public class PlayerSpawner : NetworkBehaviour
{
    public Vector3 spawnPosition;
    private static PlayerType availableCharacter = PlayerType.None;
    
    void Start()
    {
        if (!HasAuthority)
        {
            return;
        }
        transform.position = spawnPosition;
        
        GameObject prefab;
        if (CurrentPlayer.ReadOnlyTags().Length != 0)
        {
            // Dev mode
            if (!CurrentPlayer.ReadOnlyTags().Contains("INIT_GHOST") && (NetworkManager.ConnectedClients.Count == 1 ||
                                                                         CurrentPlayer.ReadOnlyTags()
                                                                             .Contains("INIT_HUMAN")))
            {
                prefab = Resources.Load<GameObject>("Human");
                transform.position += new Vector3(3, 0, 0);
            }
            else
            {
                prefab = Resources.Load<GameObject>("Ghost");
            }
        }
        else
        {
            // User mode
            if (IsSessionOwner)
            {
                if (ConnectionManager.Instance.ProfileName.Contains("Ghost"))
                {
                    prefab = Resources.Load<GameObject>("Ghost");
                    availableCharacter = PlayerType.Human;
                    Debug.Log("Setting available to human");
                }
                else
                {
                    prefab = Resources.Load<GameObject>("Human");
                    transform.position += new Vector3(3, 0, 0);
                    availableCharacter = PlayerType.Ghost;
                    Debug.Log("Setting available to ghost");
                }
            }
            else
            {
                Debug.Log($"Requesting available character from session owner");
                RequestAvailableCharacterRpc();
                return;
            }
        }
        var instance = Instantiate(prefab, transform);
        var instanceNetworkObject = instance.GetComponent<NetworkObject>();
        instanceNetworkObject.Spawn();
    }

    [Rpc(SendTo.Everyone, RequireOwnership = false)]
    private void RequestAvailableCharacterRpc()
    {
        if (!IsSessionOwner || availableCharacter == PlayerType.None)
        {
            return;
        }
        Debug.Log("Sending available " + availableCharacter);
        AvailableCharacterResponseRpc(availableCharacter);
    }

    [Rpc(SendTo.Everyone, RequireOwnership = false)]
    private void AvailableCharacterResponseRpc(PlayerType playerType)
    {
        if (IsSessionOwner || !HasAuthority)
        {
            return;
        }
        Debug.Log("Spawning character" + playerType);

        GameObject prefab;
        if (playerType == PlayerType.Human)
        {
            prefab = Resources.Load<GameObject>("Human");
            transform.position += new Vector3(3, 0, 0);
            availableCharacter = PlayerType.Ghost;
        }
        else
        {
            prefab = Resources.Load<GameObject>("Ghost");
            availableCharacter = PlayerType.Human;
        }
        var instance = Instantiate(prefab, transform);
        var instanceNetworkObject = instance.GetComponent<NetworkObject>();
        instanceNetworkObject.Spawn();
    }
    
}

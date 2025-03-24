using System.Linq;
using PlayerControls;
using PlayerMovement;
using Unity.Multiplayer.Playmode;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;
using UnityEngine.Serialization;
public class Player : NetworkTransform
{
    public float Speed = 10;
    public bool ApplyVerticalInputToZAxis;
    private Vector3 m_Motion;

    enum PlayerType
    {
        HUMAN, GHOST
    }
    
    private PlayerType playerType;

    void Start()
    {
        Debug.Log("Prefab spawned");
        if (!HasAuthority)
        {
            // Destroy(GetComponent<PlayerInputHandler>());
            // Destroy(GetComponent<PlayerMovementController>());
            // Destroy(GetComponent<PlayerAnimationController>());
            return;
        }
        
        if (!CurrentPlayer.ReadOnlyTags().Contains("INIT_GHOST") && (NetworkManager.ConnectedClients.Count == 1 || CurrentPlayer.ReadOnlyTags().Contains("INIT_HUMAN")))
        {
            Debug.Log("Human player start");
            playerType = PlayerType.HUMAN;
            tag = "Human";
        }
        else
        {
            Debug.Log("Ghost player start");
            playerType = PlayerType.GHOST;
            tag = "Ghost";
        }
        
    }
    
}

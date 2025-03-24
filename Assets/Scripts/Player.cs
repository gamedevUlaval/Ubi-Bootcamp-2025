using System.Linq;
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
        if (!HasAuthority)
        {
            return;
        }
        
        if (!CurrentPlayer.ReadOnlyTags().Contains("INIT_GHOST") && (NetworkManager.ConnectedClients.Count == 1 || CurrentPlayer.ReadOnlyTags().Contains("INIT_HUMAN")))
        {
            playerType = PlayerType.HUMAN;
            tag = "Human";
        }
        else
        {
            playerType = PlayerType.GHOST;
            tag = "Ghost";
        }
        
    }
    
}

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
    
    void Start()
    {
        if (!HasAuthority)
        {
            return;
        }
        
        GameObject prefab;
        if (!CurrentPlayer.ReadOnlyTags().Contains("INIT_GHOST") && (NetworkManager.ConnectedClients.Count == 1 || CurrentPlayer.ReadOnlyTags().Contains("INIT_HUMAN")))
        {
            prefab = Resources.Load<GameObject>("Human");
        }
        else
        {
            prefab = Resources.Load<GameObject>("Ghost");
        }
        var instance = Instantiate(prefab, transform);
        var instanceNetworkObject = instance.GetComponent<NetworkObject>();
        instanceNetworkObject.Spawn();
    }

    private void Update()
    {
        // If not spawned or we don't have authority, then don't update
        if (!IsSpawned || !HasAuthority)
        {
            return;
        }

        // Handle acquiring and applying player input
        m_Motion = Vector3.zero;
        m_Motion.x = Input.GetAxis("Horizontal");

        // Determine whether the vertical input is applied to the Y or Z axis
        if (!ApplyVerticalInputToZAxis)
        {
            m_Motion.y = Input.GetAxis("Vertical");
        }
        else
        {
            m_Motion.z = Input.GetAxis("Vertical");
        }

        // If there is any player input magnitude, then apply that amount of
        // motion to the transform
        if (m_Motion.magnitude > 0)
        {
            transform.position += m_Motion * Speed * Time.deltaTime;
        }
    }
}

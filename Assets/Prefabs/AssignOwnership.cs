using UnityEngine;
using Unity.Netcode;

public class AssignOwnership : NetworkBehaviour
{
    [SerializeField] private ulong humanClientId;

    public void SetHumanOwner(ulong clientID)
    {
        if (!IsServer) return;
        humanClientId = clientID;
        GetComponent<NetworkObject>().ChangeOwnership(humanClientId);   
    }
}

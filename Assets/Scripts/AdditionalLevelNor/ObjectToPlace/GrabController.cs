using PlayerControls;
using UnityEngine;
using Unity.Netcode;

public class GrabController : NetworkBehaviour
{
    [SerializeField] private Transform grabPoint;
    [SerializeField] private float grabCooldown = 0.3f;

    private PlayerInputHandler playerControls;
    private Rigidbody heldObject;
    private float lastGrabTime = -999f;
    private Collider currentGrabbable;

    void Start()
    {
        playerControls = GetComponent<PlayerInputHandler>();
    }

    void Update()
    {
        if (playerControls == null) return;
        if (currentGrabbable == null) return;

        if (playerControls.InteractInput && Time.time - lastGrabTime > grabCooldown)
        {
            if (heldObject == null)
                Grab();
            else
                Release();

            lastGrabTime = Time.time;
        }

        if (heldObject != null)
        {
            heldObject.MovePosition(grabPoint.position);
        }
    }

    private void Grab()
    {
        if (currentGrabbable.TryGetComponent(out Rigidbody rb))
        {
            heldObject = rb;
            heldObject.useGravity = false;
            heldObject.linearVelocity = Vector3.zero;

            if (heldObject.TryGetComponent(out NetworkObject netObj))
            {
                if (netObj.OwnerClientId != NetworkManager.Singleton.LocalClientId)
                {
                    RequestOwnershipRpc(netObj.NetworkObjectId);
                }
            }
        }
    }

    private void Release()
    {
        if (heldObject != null)
        {
            heldObject.useGravity = true;
            heldObject = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Grabbable")) return;

        if (currentGrabbable == null && other.TryGetComponent(out NetworkObject netObj))
        {
            currentGrabbable = other;
            netObj = other.GetComponent<NetworkObject>();
            // Demander le transfert d'ownership si ce n’est pas déjà le nôtre
            if (netObj.OwnerClientId != NetworkManager.Singleton.LocalClientId)
            {
                RequestOwnershipRpc(netObj.NetworkObjectId);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == currentGrabbable)
        {
            currentGrabbable = null;
        }
    }

    [Rpc(SendTo.Server)]
    private void RequestOwnershipRpc(ulong netId)
    {
        ulong requestingClientId = GetComponent<NetworkObject>().OwnerClientId;

        if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(netId, out NetworkObject netObj))
        {
            if (netObj.IsSpawned && netObj.OwnerClientId != requestingClientId)
            {
                netObj.ChangeOwnership(requestingClientId);
            }
        }
    }
}

using Unity.Netcode;
using UnityEngine;

public class PushObj : NetworkBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (!collision.gameObject.CompareTag("Ghost"))
            return;
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }
    private void OnCollisionExit(Collision collision)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (!collision.gameObject.CompareTag("Ghost"))
            return;
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY;
    }
}

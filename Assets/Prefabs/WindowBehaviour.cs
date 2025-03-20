using UnityEngine;
using Unity.Netcode;


public class WindowBehaviour : NetworkBehaviour
{
    private bool isBroken = false;


    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("BriseVitre"))
        {
            Debug.Log("COLLISION!");
            BreakWindow();
        }
    }
    public void BreakWindow()
    {
        //verify if it is already broken
        if (isBroken) return;


        if (IsOwner) // if it is the owner
        {
            DestroyWindow();
        }
        else // only the server can since it is with a server RPC attribute
        {
            BreakWindowServerRpc();
        }
    }

    [ServerRpc]
    private void BreakWindowServerRpc()
    {
        if (isBroken) return;//verify if it is already broken
        isBroken = true;
        Debug.Log($" Window broken by client {OwnerClientId}");
        DestroyWindow();
    }

    private void DestroyWindow()
    {
        Debug.Log("Vitre Brisée");

        GetComponent<NetworkObject>().Despawn(true); //Destroys object for all clients
    }
}

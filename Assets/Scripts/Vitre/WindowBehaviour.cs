using UnityEngine;
using Unity.Netcode;


public class WindowBehaviour : NetworkBehaviour
{
    private bool isBroken = false;
    Rigidbody rb;


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


        if (HasAuthority) // if it is the owner
        {
            DestroyWindow();
        }
    }
    private void DestroyWindow()
    {
        Debug.Log("Vitre Brisée");
        GetComponent<NetworkObject>().Despawn(true); //Destroys object for all clients
    }
}

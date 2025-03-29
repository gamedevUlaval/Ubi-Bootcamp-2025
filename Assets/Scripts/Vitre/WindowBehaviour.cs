using UnityEngine;
using Unity.Netcode;


public class WindowBehaviour : NetworkBehaviour
{
    public bool isBroken = false;
    public GameObject glass;
    public GameObject brokenGlass;


    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("BriseVitre"))
        {
            BreakWindow();
            
        }
    }
    public void BreakWindow()
    {
        //verify if it is already broken
        if (isBroken) return;
        DestroyWindowRpc();
    }
    [Rpc(SendTo.Everyone)]
    private void DestroyWindowRpc()
    {
        glass.SetActive(false);
        brokenGlass.SetActive(true);
        isBroken = true;
    }
}

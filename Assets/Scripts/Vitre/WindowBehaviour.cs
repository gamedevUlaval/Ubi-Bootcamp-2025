using UnityEngine;
using Unity.Netcode;


public class WindowBehaviour : MonoBehaviour
{
    private bool isBroken = false;
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
        
        DestroyWindow();
    }
    private void DestroyWindow()
    {
        glass.SetActive(false);
        brokenGlass.SetActive(true);
    }
}

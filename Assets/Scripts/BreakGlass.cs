using UnityEngine;
using Unity.Netcode;
public class BreakGlass : NetworkBehaviour 
{
    //public AudioClip sound;
    [SerializeField] private GameObject glass, brokenGlass;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Breakable") && HasAuthority)
        {
            //GetComponent<AudioSource>().PlayOneShot(sound);
            glass.SetActive(false);
            brokenGlass.SetActive(true);
            collision.gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }
}

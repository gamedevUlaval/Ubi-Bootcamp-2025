using UnityEngine;

public class BreakGlass : MonoBehaviour
{
    public AudioClip sound;
    public GameObject glass, brokenGlass;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Breakable")
        {
            GetComponent<AudioSource>().PlayOneShot(sound);
            glass.SetActive(false);
            GetComponent<BoxCollider>().enabled = false;
            brokenGlass.SetActive(true);
        }
    }
}

using UnityEngine;

public class BreakGlass : MonoBehaviour
{
    public AudioClip sound;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Breakable")
        {
            GetComponent<AudioSource>().PlayOneShot(sound);
            collision.gameObject.SetActive(false);
            GetComponent<BoxCollider>().enabled = false;
        }
    }
}

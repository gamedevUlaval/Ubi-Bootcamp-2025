using UnityEngine;

public class BreakGlass : MonoBehaviour
{
    public AudioClip sound;
    [SerializeField] private GameObject glass, brokenGlass;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Breakable")
        {
            GetComponent<AudioSource>().PlayOneShot(sound);
            glass.SetActive(false);
            brokenGlass.SetActive(true);
            GetComponent<BoxCollider>().enabled = false;
        }
    }
}

using UnityEngine;

public class MenuSoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip hoverSound;
    [SerializeField] private AudioClip clickSound;
    private AudioSource audioSource;
    
    private void Awake()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }
    
    public void PlayHoverSound()
    {
        audioSource.volume = 0.01f;
        audioSource.PlayOneShot(hoverSound);
    }
    
    public void PlayClickSound()
    {
        audioSource.volume = 0.2f;
        audioSource.PlayOneShot(clickSound);
    }
}

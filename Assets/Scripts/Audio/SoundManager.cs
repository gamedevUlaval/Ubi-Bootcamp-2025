using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;   // Assigné au groupe "Musics"
    public AudioSource sfxSource;     // Assigné au groupe "SoundEffects"
    public AudioSource menuSource;    // Assigné au groupe "Menus"
    [SerializeField] private AudioSource footstepsSource;

    [Header("Audio Clips")]
    public AudioClip mainTheme; // Musique du menu ou du début de jeu
    [SerializeField] private AudioClip[] woodFootsteps;      //Pas de l'humain sur bois

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Persiste entre les scènes
    }

    private void Start()
    {
        if (mainTheme != null)
        {
            PlayMusic(mainTheme);
        }
    }

    /// <summary>
    /// Joue une musique en boucle via le canal musique.
    /// </summary>
    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        if (clip == null) return;

        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.Play();
    }

    /// <summary>
    /// Stoppe la musique.
    /// </summary>
    public void StopMusic()
    {
        musicSource.Stop();
    }

    /// <summary>
    /// Joue un effet sonore via le canal SFX.
    /// </summary>
    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;

        sfxSource.PlayOneShot(clip);
    }

    /// <summary>
    /// Joue un son d’interface via le canal Menu.
    /// </summary>
    public void PlayMenuSound(AudioClip clip)
    {
        if (clip == null) return;

        menuSource.PlayOneShot(clip);
    }
    public void PlayFootstep()
{
    if (woodFootsteps.Length == 0) return;

    int index = Random.Range(0, woodFootsteps.Length);
    footstepsSource.pitch = Random.Range(0.90f, 1.05f);
    footstepsSource.PlayOneShot(woodFootsteps[index]);
}
}

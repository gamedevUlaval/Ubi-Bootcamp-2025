using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;   // Assigné au groupe "Musics"
    public AudioSource sfxSource;     // Assigné au groupe "SoundEffects"
    public AudioSource menuSource;    // Assigné au groupe "Menus"
    public AudioSource foleysSource;

    public AudioSource loopingSource;

    [Header("Audio Clips")]
    public AudioClip mainTheme; // Musique du menu ou du début de jeu
    public AudioClip successMusic;

    [Header("Level Musics")]

    public AudioClip firstLevelMusic;
    public AudioClip lightningLevelMusic;
    public AudioClip lampsLevelMusic;
    public AudioClip frameLevelMusic;
    [Header("Level Scene Names")] 
    public string levelLampeName;
    public string levelLightningName;
    public string levelFramesName;
    public string levelOneName;
    private Dictionary<string, AudioClip> sceneMusicMap;

    [Header("HumanSounds")]
    [SerializeField] private AudioClip[] woodFootsteps;
          //Pas de l'humain sur bois
    [Header("GhostSounds")]
    [SerializeField] private AudioClip ghostHaunting;
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
        sceneMusicMap = new Dictionary<string, AudioClip> {
            { levelLightningName, lightningLevelMusic} , {levelFramesName, frameLevelMusic} , {levelLampeName, lampsLevelMusic}, {levelOneName, firstLevelMusic}};
        PlayMainTheme();
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

    public void PlayMainTheme()
    {
        if (mainTheme != null)
        {
            PlayMusic(mainTheme);
        }
    }

    public void MuteAllSfx()
    {
        sfxSource.mute = true;
        foleysSource.mute = true;
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
        foleysSource.pitch = Random.Range(0.90f, 1.05f);
        foleysSource.PlayOneShot(woodFootsteps[index]);
    }

    public void PlayGhostHaunt()
    {
        foleysSource.PlayOneShot(ghostHaunting);
    }
    public void PlaySuccessMusic()
    {
        sfxSource.PlayOneShot(successMusic);
    }
    public void PlaySFXLoop(AudioClip clip)
    {
        if (loopingSource.isPlaying && loopingSource.clip == clip) return;
    
        loopingSource.clip = clip;
        loopingSource.loop = true;
        loopingSource.Play();
    }

    public void StopSFXLoop()
    {
        if (loopingSource.isPlaying)
        {
            loopingSource.Stop();
            loopingSource.clip = null;
            loopingSource.loop = false;
        }
    }
    public void ChangeMainMusic(string sceneName)
    {
        if (sceneMusicMap.TryGetValue(sceneName, out AudioClip clip) && clip != null)
        {
            PlayMusic(clip);
        }
        else
        {
            Debug.LogWarning($"No music assigned for scene: {sceneName}");
        }
    }
}

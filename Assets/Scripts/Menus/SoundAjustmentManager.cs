using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundAjustmentManager : MonoBehaviour
{
    [Header("Volume name in preferences file (should be the same as the corresponding name when setting the AudioMixer)")]
    [SerializeField] private string volumeName;
    
    private Slider volumeSlider;
    private Toggle muteToggle;
    private TMP_Text percentageText;
    private float previousVolume;
    
    [SerializeField] private AudioMixer musicAudioMixer;

    private void Awake()
    {
        volumeSlider = GetComponent<Slider>();
        muteToggle = GetComponentInChildren<Toggle>();
        percentageText = GetComponentInChildren<TMP_Text>();
        
        volumeSlider.onValueChanged.AddListener(OnVolumeSliderValueChanged);
        muteToggle.onValueChanged.AddListener(OnMuteToggleValueChanged);
        
        if (!PlayerPrefs.HasKey(volumeName))
        {
            PlayerPrefs.SetFloat(volumeName, 1);
        }
        
        volumeSlider.value = PlayerPrefs.GetFloat(volumeName);
        muteToggle.isOn = volumeSlider.value == 0; //check if ok
        previousVolume = volumeSlider.value;
        percentageText.text = Mathf.RoundToInt(volumeSlider.value * 100) + "%";
        
        musicAudioMixer.SetFloat(volumeName, volumeSlider.value == 0 ? -80 : Mathf.Log10(volumeSlider.value) * 20);
    }
    
    private void OnVolumeSliderValueChanged(float value)
    {
        PlayerPrefs.SetFloat(volumeName, value);
        percentageText.text = Mathf.RoundToInt(value * 100) + "%";
        muteToggle.isOn = value == 0;
        musicAudioMixer.SetFloat(volumeName, volumeSlider.value == 0 ? -80 : Mathf.Log10(volumeSlider.value) * 20);
    }
    
    private void OnMuteToggleValueChanged(bool value)
    {
        PlayerPrefs.SetFloat(volumeName, value ? volumeSlider.value : 0);
        percentageText.text = value ? "0%" : Mathf.RoundToInt(volumeSlider.value * 100) + "%";
        previousVolume = value ? volumeSlider.value : previousVolume;
        volumeSlider.value = value ? 0 : previousVolume;
        musicAudioMixer.SetFloat(volumeName, volumeSlider.value == 0 ? -80 : Mathf.Log10(volumeSlider.value) * 20);
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MouseSensitivityManager : MonoBehaviour
{
    private Slider sensitivitySlider;
    private TMP_Text percentageText;
    
    //[SerializeField] private FPSController fpsController;
    
    private void Awake()
    {
        sensitivitySlider = GetComponent<Slider>();
        percentageText = GetComponentInChildren<TMP_Text>();

        sensitivitySlider.onValueChanged.AddListener(OnSensitivitySliderValueChanged);

        if (!PlayerPrefs.HasKey("MouseSensitivity"))
        {
            PlayerPrefs.SetFloat("MouseSensitivity", 1);
        }

        sensitivitySlider.value = PlayerPrefs.GetFloat("MouseSensitivity");
        percentageText.text = Mathf.RoundToInt(sensitivitySlider.value * 100) + "%";
    }
    
    public void OnSensitivitySliderValueChanged(float value)
    {
        PlayerPrefs.SetFloat("MouseSensitivity", value);
        percentageText.text = Mathf.RoundToInt(value * 100) + "%";
        
        // if (fpsController != null)
        // {
        //     fpsController.setMouseSensitivity(value);
        // }
    }
}

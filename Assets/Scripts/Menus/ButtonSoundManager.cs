using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class ButtonSoundManager : MonoBehaviour, IPointerEnterHandler 
{
    private Button button;
    private GameObject menuSoundManagerGb;
    private MenuSoundManager menuSoundManager;
    
    private void Awake()
    {
        menuSoundManagerGb = GameObject.Find("MenuSoundManager");
        
        menuSoundManager = menuSoundManagerGb.GetComponent<MenuSoundManager>();
        
        button = GetComponent<Button>();
        button.onClick.AddListener(PlayButtonSound);
        
        EventTrigger eventTrigger = button.gameObject.AddComponent<EventTrigger>();
        
        EventTrigger.Entry entryEnter = new EventTrigger.Entry();
        entryEnter.eventID = EventTriggerType.PointerEnter;
        entryEnter.callback.AddListener((data) => { OnPointerEnter((PointerEventData)data); });
        eventTrigger.triggers.Add(entryEnter);
    }
    
    private void PlayButtonSound()
    {
        menuSoundManager.PlayClickSound();
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        menuSoundManager.PlayHoverSound();
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        return;
    }
}

using PlayerControls;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;


public class OpenSafe : MonoBehaviour 
{
    [SerializeField] private TextMeshProUGUI codeText;
    [SerializeField] private AudioClip beepSound;
    [SerializeField] private AudioClip successSound;
    [SerializeField] private AudioClip wrongSound;
    [SerializeField] private PlayerInputHandler playerControls;

    [SerializeField] private WindowBehaviour window;
    private string codeTextValue = "";
    public string safeCode;
    public GameObject codePannel;
    public GameObject UIKey;
    public bool HaveKey = false;
    private bool isInReach = false;

    // Update is called once per frame
    void Update()
    {
        codeText.text = codeTextValue;
        
        if (isInReach && codeTextValue != safeCode && window.isBroken)
        {
            if (playerControls.InteractInput)
            {
                codePannel.SetActive(true);
            }
        }
        else if (isInReach && codeTextValue == safeCode) 
        {
            HaveKey = true;
            codePannel.SetActive(false);
            UIKey.SetActive(true);
        }
        else
        {
            codePannel.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Human" && !isInReach)
        {
            Debug.Log("Okay");
            playerControls = other.gameObject.GetComponent<PlayerInputHandler>();
            isInReach = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isInReach = false;
    }

    public void AddDigit(string digit)
    {
        if (codeTextValue.Length >= 3)
        {
            codeTextValue += digit;

            if (codeTextValue == safeCode)
            {
                GetComponent<AudioSource>().PlayOneShot(successSound);
                codePannel.SetActive(false);
            }
            else
            {
                GetComponent<AudioSource>().PlayOneShot(wrongSound);
                codeTextValue = "";
            }
        }
        else
        {
            GetComponent<AudioSource>().PlayOneShot(beepSound);
            codeTextValue += digit;
        }
    }
}

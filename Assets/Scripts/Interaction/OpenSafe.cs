using PlayerControls;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;


public class OpenSafe : NetworkBehaviour, IInteractable 
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
            codePannel.SetActive(false);
            AddKeyRPC();
        }
        else
        {
            codePannel.SetActive(false);
        }
    }

    [Rpc(SendTo.Everyone)]
    private void AddKeyRPC()
    {
        UIKey.SetActive(true);
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

    public void Interact()
    {
        if (isInReach && window.isBroken)
        {
            if (playerControls.InteractInput)
            {
                codePannel.SetActive(true);
            }
        }
    }

    public bool InteractWith(GameObject tryToInteractWith)
    {
        return false;
    }

    public InteractableType InteractableType => InteractableType.Static;
}

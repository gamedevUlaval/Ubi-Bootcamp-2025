using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System;

public class OpenSafe : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI codeText;

    private InputHumain playerControls;
    private string codeTextValue = "";
    public string safeCode;
    public GameObject codePannel;
    private bool isInReach = false;


    private void OnEnable()
    {
        playerControls = new InputHumain();
        playerControls.Player.Interact.Enable();
    }

    private void OnDisable()
    {
        playerControls.Player.Interact.started -= Interact;
        playerControls.Player.Interact.Disable();
    }

    private void Interact(InputAction.CallbackContext context)
    {
        isInReach = true;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        codeText.text = codeTextValue;

        if (isInReach)
        {
            codePannel.SetActive(true);
        }
        else
        {
            codePannel.SetActive(false);
        }

        if (codeTextValue == safeCode)
        {
            codePannel.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerControls.Player.Interact.started += Interact;
        }
        else
        {
            isInReach = false;
            playerControls.Player.Interact.started -= Interact;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isInReach = false;
        playerControls.Player.Interact.started -= Interact;
    }

    public void AddDigit(string digit)
    {
        if (codeTextValue.Length >= 3)
        {
            codeTextValue = "";
        }
        else
        {
            codeTextValue += digit;
        }
    }
}

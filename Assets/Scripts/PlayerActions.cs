using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActions : MonoBehaviour
{
    [SerializeField] private float playerActivationDistance;
    private bool activeRay = false;
    private CharacterController _characterController;
    private InputHumain playerControls;
    public GameObject spawner;
    public GameObject objectToSpawn;

    private void Awake()
    {
        playerControls = new InputHumain();
        _characterController = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        playerControls.Player.Interact.Enable();
        playerControls.Player.Interact.started += Interact;
    }

    private void OnDisable()
    {
        playerControls.Player.Interact.started -= Interact;
        playerControls.Player.Interact.Disable();
    }

    private bool isInReach()
    {
        RaycastHit hit;
        activeRay = Physics.Raycast(_characterController.transform.position + Vector3.up, _characterController.transform.forward, out hit);

        //if player is in reach from an interactable object
        if (activeRay && hit.distance < playerActivationDistance && hit.transform.tag == "Interactible")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void Interact(InputAction.CallbackContext context)
    {
        //if player is in reach from an interactable object
        if (isInReach())
        {
            Instantiate(objectToSpawn, spawner.transform);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

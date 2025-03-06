using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemIsPicked : MonoBehaviour
{
    private InputHumain playerControls;
    private Transform pickupPoint;
    private Transform player;

    public float pickupDistance;
    public float force;

    public bool readyToThrow;
    public bool isPicked;

    Rigidbody rb;

    private void OnEnable()
    {
        playerControls = new InputHumain();
        playerControls.Player.Interact.Enable();
        playerControls.Player.Interact.started += PickOrThrow;
    }

    private void OnDisable()
    {
        playerControls.Player.Interact.started -= PickOrThrow;
        playerControls.Player.Interact.Disable();
    }

    private void PickOrThrow(InputAction.CallbackContext context)
    {
        pickupDistance = Vector3.Distance(player.position, transform.position);

        if (isPicked)
        {
            rb.AddForce(player.transform.forward * force);
            this.transform.parent = null;
            this.transform.position = transform.position;
            GetComponent<Rigidbody>().useGravity = true;
            isPicked = false;
        }

        if (pickupDistance <= 0.5)
        {
            if (isPicked == false && pickupPoint.childCount == 0)
            {
                GetComponent<Rigidbody>().useGravity = false;
                this.transform.position = pickupPoint.position;
                this.transform.parent = GameObject.Find("PickupPoint").transform;
                isPicked = true;
            }
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.Find("Idle (1)").transform;
        pickupPoint = GameObject.Find("PickupPoint").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

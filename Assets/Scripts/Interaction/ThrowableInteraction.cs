using UnityEngine;
using System.Collections;
using PlayerControls;

public class ThrowableInteraction : MonoBehaviour, IInteractable
{
    [Header("Pickup Settings")]
    public float pickupRange = 2f;
    [SerializeField] private Transform pickupPoint;
    [SerializeField] private Transform player;

    [Header("Throw Settings")]
    public float throwForce = 10f;

    [Header("Anti-Spam Settings")]
    public float interactionCooldown = 0.3f;
    private bool isOnCooldown = false;

    private bool isHeld = false;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(WaitForPlayerReference());
        if (rb != null) rb.isKinematic = true;
    }

    void Update()
    {
        if (player == null || pickupPoint == null) return;

        // simulate parenting
        if (isHeld)
        {
            FollowPickupPoint();
        }
        if (PlayerInputHandler.Instance.InteractInput && isHeld)
        {
            Interact();
        }
    }

    public void Interact()
    {
        if (isOnCooldown) return;
        print("took the object");

        StartCoroutine(CooldownRoutine());

        if (!isHeld && Vector3.Distance(player.position, transform.position) < pickupRange)
        {
            Pickup();
        }
        else if (isHeld)
        {
            Throw();
        }
    }

    public bool InteractWith(GameObject tryToInteractWith)
    {
        return true;
    }

    void Pickup()
    {
        rb.isKinematic = true;
        isHeld = true;

        Debug.Log("[Pickup] Object picked up.");
    }

    void Throw()
    {
        isHeld = false;
        rb.isKinematic = false;
        rb.AddForce(player.forward * throwForce, ForceMode.Impulse);

        Debug.Log("[Throw] Object thrown.");
    }

    void FollowPickupPoint()
    {
        transform.position = pickupPoint.position;
        transform.rotation = pickupPoint.rotation;
    }

    IEnumerator WaitForPlayerReference()
    {
        while (player == null || pickupPoint == null)
        {
            GameObject playerGO = GameObject.FindGameObjectWithTag("Human");

            if (playerGO != null)
            {
                player = playerGO.transform;
                pickupPoint = player.Find("PickupPoint");
            }

            yield return null;
        }
    }

    IEnumerator CooldownRoutine()
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(interactionCooldown);
        isOnCooldown = false;
    }

    public InteractableType InteractableType => InteractableType.Pickable;

    public bool IsHeld => isHeld;
}

using System.Collections;
using UnityEngine;

public class GrabNThrow : MonoBehaviour
{
    [Header("Pickup Settings")]
    public float pickupRange = 2f;
    [SerializeField] private Transform pickupPoint;
    [SerializeField] private Transform player;

    [Header("Throw Settings")]
    public float throwForce = 10f;

    private bool isHeld = false;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(WaitForPlayerReference());
    }

    void Update()
    {
        if (player == null || pickupPoint == null) return;

        if (Input.GetKeyDown(KeyCode.E)) // à remplacer par Input System si besoin
        {
            if (!isHeld && Vector3.Distance(player.position, transform.position) < pickupRange)
            {
                Pickup();
            }
            else if (isHeld)
            {
                Throw();
            }
        }

        if (isHeld)
        {
            FollowPickupPoint(); // simulate parenting safely
        }
    }

    void Pickup()
    {
        rb.isKinematic = true;
        isHeld = true;
    }

    void Throw()
    {
        isHeld = false;
        rb.isKinematic = false;
        rb.AddForce(player.forward * throwForce, ForceMode.Impulse);
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
}

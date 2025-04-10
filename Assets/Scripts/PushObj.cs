using System;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class PushObj : NetworkBehaviour
{
    private Rigidbody rb;

    [Header("Sound")]
    [SerializeField] private AudioClip pushingSound;

    private float movementThreshold = 0.009f;
    private float stopDelay = 0.3f;
    private float timeSinceMoving = 0f;
    private bool isLoopPlaying = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!IsOwner) return;

        float velocity = rb.linearVelocity.magnitude;
        Debug.Log(velocity);
        if (velocity > movementThreshold)
        {
            timeSinceMoving = 0f;

            if (!isLoopPlaying)
            {
                SoundManager.Instance.PlaySFXLoop(pushingSound);
                isLoopPlaying = true;
            }
        }
        else
        {
            timeSinceMoving += Time.deltaTime;

            if (isLoopPlaying && timeSinceMoving > stopDelay)
            {
                SoundManager.Instance.StopSFXLoop();
                isLoopPlaying = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Ghost"))
            return;
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Ghost"))
            return;
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY;
    }
}


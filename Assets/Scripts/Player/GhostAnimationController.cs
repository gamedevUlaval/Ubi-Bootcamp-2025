using UnityEngine;
using Unity.Netcode;
using PlayerMovement; // Important ! Pour reconnaître PlayerMovementController

[RequireComponent(typeof(Animator))]
public class GhostAnimationController : NetworkBehaviour
{
    private Animator _animator;
    private PlayerMovementController _movementController;
    private bool isMoving;

    private static readonly int IsMoving = Animator.StringToHash("isMoving");

    [SerializeField] private float idleSpeed = 0.4f;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _movementController = GetComponent<PlayerMovementController>();
    }

    void FixedUpdate()
    {
        if (!HasAuthority || _movementController == null)
            return;

        Vector3 move = _movementController.GetPlayerMovement();
        isMoving = move.magnitude > 0.05f;

        _animator.SetBool("IsMoving", isMoving);
        _animator.speed = isMoving ? 1f : idleSpeed;
    }
}

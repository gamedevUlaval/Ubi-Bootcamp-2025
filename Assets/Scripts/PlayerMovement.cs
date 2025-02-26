using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private InputHumain playerControls;
    private CharacterController _characterController;
    [SerializeField]
    private float pushStrength;
    [SerializeField]
    private float speed;
    [Header("Animators")]
    [SerializeField]
    private Animator _animator;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        playerControls = new InputHumain();
    }

    private void OnEnable()
    {
        playerControls.Player.Enable();
    }

    private void OnDisable()
    {
        playerControls.Player.Disable();
    }

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        RotatePlayer();
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.tag == "PushableObject")
        {
            Rigidbody box = hit.collider.GetComponent<Rigidbody>();
            if (box != null)
            {
                Vector3 pushDirection = new Vector3(hit.moveDirection.x, 0.0f, hit.moveDirection.z);
                box.AddForce(pushDirection * pushStrength * Time.deltaTime, ForceMode.Force);
            }
        }
    }

    private Vector2 GetCurrentMovement()
    {
        return playerControls.Player.Move.ReadValue<Vector2>();
    }

    private void MovePlayer()
    {
        Vector2 direction = GetCurrentMovement();
        if (direction != Vector2.zero)
        {
            _animator.SetBool("isWalking", true);
            _characterController.Move(new Vector3(direction.x, 0.0f, direction.y) * speed * Time.deltaTime);
        }
        else {
            _animator.SetBool("isWalking", false);
        }
    }

    private void RotatePlayer()
    {
        Vector2 currentMovement = GetCurrentMovement();
        Vector3 currentPosition = _characterController.transform.position;
        Vector3 newPosition = new Vector3(currentMovement.x, 0.0f, currentMovement.y) * Time.deltaTime;
        Vector3 positionToLookAt = (currentPosition + newPosition);
        _characterController.transform.LookAt(positionToLookAt);
    }
}

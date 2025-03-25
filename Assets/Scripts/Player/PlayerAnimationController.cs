using PlayerMovement;
using Unity.Netcode;
using UnityEngine;

public class PlayerAnimationController : NetworkBehaviour
{
    PlayerMovementController _playerMovementController;
    Animator _animator;
    
    static readonly int VelocityZ = Animator.StringToHash("VelocityZ");
    static readonly int VelocityX = Animator.StringToHash("VelocityX");
    static readonly int IsSprinting = Animator.StringToHash("IsSprinting");
    
    bool _isRunning = false;
    bool _isMoving = true;
    
    void Awake()
    {
        _playerMovementController = GetComponent<PlayerMovementController>();
        _animator = GetComponent<Animator>();
    }
    
    void FixedUpdate()
    {
        if (!HasAuthority)
        {
            return;
        }
        UpdateAnimator();
    }
    
    void UpdateAnimator()
    {
        if (!_isMoving) return;
        
        bool isSprinting = _playerMovementController.IsSprinting();
        _animator.SetBool(IsSprinting, isSprinting);
        
        Vector3 moveDirection = _playerMovementController.GetPlayerMovement();
        
        if (moveDirection.magnitude > 0.01f)
        {
            float velocityX = Vector3.Dot(moveDirection.normalized, transform.forward);
            float velocityZ = Vector3.Dot(moveDirection.normalized, transform.right);
        
            _animator.SetFloat(VelocityZ, velocityZ, 0.1f, Time.deltaTime);
            _animator.SetFloat(VelocityX, velocityX, 0.1f, Time.deltaTime);
        }
        else
        {
            _animator.SetFloat(VelocityZ, 0f, 0.1f, Time.deltaTime);
            _animator.SetFloat(VelocityX, 0f, 0.1f, Time.deltaTime);
        }
    }
}

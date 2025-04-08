using PlayerControls;
using Unity.Netcode;
using UnityEngine;

namespace PlayerMovement
{
    public class PlayerMovementController : NetworkBehaviour
    {
        [SerializeField] float walkSpeed = 0.3f;
        [SerializeField] float sprintMultiplier = 1.5f;
        [SerializeField] LayerMask aimLayerMask;
        
        Camera _mainCamera;
        PlayerInputHandler _playerInputHandler;
    
        void Awake()
        {
            _mainCamera = Camera.main;
        }
        
        void Start()
        {
            _playerInputHandler = PlayerInputHandler.Instance;
        }

        void FixedUpdate()
        {
            if (!HasAuthority)
            {
                return;
            }
            MovePlayer();
            LookTowardsMouse();
        }

        void MovePlayer()
        {
            Vector3 moveDirection = GetPlayerMovement();
    
            transform.Translate(moveDirection, Space.World);
        }
        
        void LookTowardsMouse()
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, aimLayerMask))
            {
                Vector3 direction = hitInfo.point - transform.position;
                direction.y = 0f;
                direction.Normalize();
                transform.forward = direction;
            }
        }

        public Vector3 GetPlayerMovement()
        {
            float moveSpeed = _playerInputHandler.IsSprinting ? walkSpeed * 0.3f * sprintMultiplier : walkSpeed * 0.3f;
            Vector2 input = _playerInputHandler.MoveInput;
            
            Vector3 cameraForward = _mainCamera.transform.forward;
            Vector3 cameraRight = _mainCamera.transform.right;
            
            cameraForward.y = 0;
            cameraRight.y = 0;
            cameraForward.Normalize();
            cameraRight.Normalize();
            
            Vector3 moveDirection = cameraRight * input.x + cameraForward * input.y;
            
            if (moveDirection.magnitude > 1)
            {
                moveDirection.Normalize();
            }
            moveDirection *= moveSpeed;
    
            return moveDirection;
        }
        
        public bool IsSprinting()
        {
            return _playerInputHandler.IsSprinting;
        }
    }
}

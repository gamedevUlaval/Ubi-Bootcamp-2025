using System;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerControls
{
    public class PlayerInputHandler : NetworkBehaviour
    {
        [SerializeField] InputActionAsset playerControls;
        
        readonly string movementActionMapName = "PlayerMovement";
        
        readonly string move = "Move";
        readonly string look = "Look";
        readonly string sprint = "Sprint";
        readonly string interact = "Interact";
        readonly string drop = "Drop";
        readonly string pause = "Pause";
        
        InputAction moveAction;
        InputAction lookAction;
        InputAction sprintAction;
        InputAction interactAction;
        InputAction dropAction;
        InputAction pauseAction;
        
        [SerializeField] float controllerStickDeadZone = 0.125f;
        
        public Vector2 MoveInput { get; private set; }
        public Vector2 LookInput { get; private set; }
        public bool IsSprinting { get; private set; }
        public bool InteractInput { get; private set; }
        public bool DropInput { get; private set; }
        public bool PauseInput { get; private set; }
        
        public static PlayerInputHandler Instance { get; private set; }
        
        public override void OnNetworkSpawn()
        {
            if (!HasAuthority)
            {
                DontDestroyOnLoad(gameObject);
                return;
            }
            if (Instance is null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            FindActionMap();

            RegisterMovementActions();
            
            OnEnable();
            
            InputSystem.settings.defaultDeadzoneMin = controllerStickDeadZone;
        }

        void Start()
        {
            if (PlayerInputHandler.Instance is null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        void FindActionMap()
        {
            moveAction = playerControls.FindActionMap(movementActionMapName).FindAction(move);
            lookAction = playerControls.FindActionMap(movementActionMapName).FindAction(look);
            sprintAction = playerControls.FindActionMap(movementActionMapName).FindAction(sprint);
            interactAction = playerControls.FindActionMap(movementActionMapName).FindAction(interact);
            dropAction = playerControls.FindActionMap(movementActionMapName).FindAction(drop);
            pauseAction = playerControls.FindActionMap(movementActionMapName).FindAction(pause);
        }

        void RegisterMovementActions()
        {
            moveAction.performed += context => MoveInput = context.ReadValue<Vector2>();
            moveAction.canceled += context => MoveInput = Vector2.zero;
            
            lookAction.performed += context => LookInput = context.ReadValue<Vector2>();
            lookAction.canceled += context => LookInput = Vector2.zero;
            
            sprintAction.performed += context => IsSprinting = true;
            sprintAction.canceled += context => IsSprinting = false;
            
            interactAction.started += context => InteractInput = true;
            interactAction.canceled += context => InteractInput = false;
            
            dropAction.started += context => DropInput = true;
            dropAction.canceled += context => DropInput = false;
            
            pauseAction.started += context => PauseInput = true;
            pauseAction.canceled += context => PauseInput = false;
        }
        
        void OnEnable()
        {
            if (moveAction == null)
            {
                return;
            }
            moveAction.Enable();
            lookAction.Enable();
            sprintAction.Enable();
            interactAction.Enable();
            dropAction.Enable();
            pauseAction.Enable();
        }
        
        void OnDisable()
        {
            if (moveAction == null)
            {
                return;
            }
            moveAction.Disable();
            lookAction.Disable();
            sprintAction.Disable();
            interactAction.Disable();
            dropAction.Disable();
            pauseAction.Disable();
        }
    }
}
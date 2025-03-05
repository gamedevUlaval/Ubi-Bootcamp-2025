using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private static readonly int VelocityZ = Animator.StringToHash("VelocityZ");
    private static readonly int VelocityX = Animator.StringToHash("VelocityX");
    [SerializeField] private float speed = 5f;
    [SerializeField] private LayerMask aimLayerMask;

    private Animator animator;
    private Camera mainCamera;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        MovePlayer();
        AimTowardsMouse();
    }

    private void MovePlayer()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        Vector3 movement = new Vector3(horizontal, 0f, vertical);

        if (movement.magnitude > 0.01f)
        {
            movement.Normalize();
            movement *= speed * Time.deltaTime;
            transform.Translate(movement, Space.World);
            
            float velocityZ = Vector3.Dot(movement.normalized, transform.right);
            float velocityX = Vector3.Dot(movement.normalized, transform.forward);

            animator.SetFloat(VelocityZ, velocityZ, 0.1f, Time.deltaTime);
            animator.SetFloat(VelocityX, velocityX, 0.1f, Time.deltaTime);
        }
        else
        {
            animator.SetFloat(VelocityZ, 0f, 0.1f, Time.deltaTime);
            animator.SetFloat(VelocityX, 0f, 0.1f, Time.deltaTime);
        }
    }

    private void AimTowardsMouse()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, aimLayerMask))
        {
            Vector3 direction = hitInfo.point - transform.position;
            direction.y = 0f;
            direction.Normalize();
            transform.forward = direction;
        }
    }
}

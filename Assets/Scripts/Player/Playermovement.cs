using UnityEngine;
using UnityEngine.InputSystem;

public class Playermovement : MonoBehaviour
{
    public static Playermovement instance;
    [SerializeField] private float walkspeed, runSpeed, jumpForce;
    [SerializeField] LayerMask ground;
    [SerializeField] Transform groundCheck;

    private Rigidbody rb;
    private Vector2 input;
    private float speed;
    private bool isGrounded;
 

    private void Start()
    {
        instance = this;
        rb = GetComponent<Rigidbody>();
        speed = walkspeed;
    }

    public void playerMovement(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
    }

    public void jumpInput()
    {
        if (isGrounded)
        {
            rb.velocity = new Vector3(0, jumpForce, 0);
        }
    }

    public void runMovement(InputAction.CallbackContext context)
    {
        if (input.sqrMagnitude != 0)
        {
            if (context.ReadValueAsButton())
            {
                speed = runSpeed;
            }
            else
            {
                speed = walkspeed;
            }
        }
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.2f, ground);
    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3((transform.forward.x * input.y * speed * Time.deltaTime) + (transform.right.x * input.x * speed * Time.deltaTime), 0, (transform.forward.z * input.y * speed * Time.deltaTime) + (transform.right.z * input.x * speed * Time.deltaTime));
        rb.MovePosition(transform.position + movement);
    }
}

using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class Playermovement : MonoBehaviour
{
    public static Playermovement instance;
    [SerializeField] private float walkspeed, runSpeed, jumpForce, airControl;
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

    public void jumpInput(InputAction.CallbackContext context)
    {
        if (isGrounded && context.ReadValueAsButton())
        {
            rb.AddForce(Vector3.up * jumpForce * Time.deltaTime * 1500, ForceMode.Force);
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
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.1f, ground);
    }

    private void FixedUpdate()
    {
        float friction = airControl;
        if (isGrounded)
        {
            friction = 1;
            rb.drag = 8;
        }
        else
        {
            if(speed == runSpeed)
            {
                friction = airControl * 0.2f;
            }
            else
            {
                friction = airControl * 0.4f;
            }
            
            rb.drag = 0.05f;
        }

        if(rb.velocity.y < 0 && !isGrounded)
        {
            rb.mass += Time.deltaTime*5;
        }
        else
        {
            rb.mass = 1;
        }
        Vector3 movement = new Vector3((transform.forward.x * input.y) + (transform.right.x * input.x), 0, (transform.forward.z * input.y) + (transform.right.z * input.x));
        rb.AddForce(movement.normalized * speed * friction * Time.deltaTime * 500, ForceMode.Force);
    }
}

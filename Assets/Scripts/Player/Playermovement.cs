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
            rb.AddForce(new Vector3(0,jumpForce * Time.deltaTime * 1500f, 0));
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
        float friction;
        if (isGrounded)
        {
            friction = 1;
            rb.drag = 8;
        }
        else
        {
            friction = 0.2f;
            if(speed == runSpeed)
            {
                rb.drag = 0.01f * airControl;
            }
            else
            {
                rb.drag = 0.05f * airControl;
            }

            rb.velocity = new Vector3(Mathf.Clamp(rb.velocity.x, -10, 10), Mathf.Clamp(rb.velocity.y, -10, 10), Mathf.Clamp(rb.velocity.z, -10, 10));
        }

        if(rb.velocity.y < 0 && !isGrounded)
        {
            rb.mass += Time.deltaTime*5f;
        }
        else
        {
            rb.mass = 1;
        }
        Vector3 movement = new Vector3((transform.forward.x * input.y) + (transform.right.x * input.x), 0, (transform.forward.z * input.y) + (transform.right.z * input.x));
        rb.AddForce(movement.normalized * speed * Time.deltaTime * 500 * friction, ForceMode.Force);
    }
}

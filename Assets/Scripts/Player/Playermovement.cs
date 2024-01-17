using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

public class Playermovement : MonoBehaviour
{
    public static Playermovement instance;


    [SerializeField] private float walkspeed, runSpeed, jumpForce, airControl;
    [SerializeField] LayerMask ground;
    [SerializeField] Transform groundCheck;

    private Rigidbody rb;
    private Vector2 input;
    private float speed;
    private bool isGrounded, isSprinting;

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
            rb.AddForce(new Vector3(0,jumpForce*25, 0));
        }
    }
   

    public void runMovement(InputAction.CallbackContext context)
    {
        if (input.sqrMagnitude != 0)
        {
            if (context.performed)
            {
                isSprinting = true;
            }
            else if (context.canceled)
            {
                isSprinting= false;
            }
        }
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.1f, ground);
        if (InventoryScript.instance.GetAim() || !isSprinting)
        {
            speed = walkspeed;
        }
        else if(isSprinting &&  !InventoryScript.instance.GetAim())
        {
            speed = runSpeed;
        }
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
            if (rb.velocity.y < 0.5f)
            {
                rb.AddForce(new Vector3(0, -200 * Time.fixedDeltaTime, 0));
            }
            rb.velocity = new Vector3(Mathf.Clamp(rb.velocity.x, -10, 10), Mathf.Clamp(rb.velocity.y, -10, 10), Mathf.Clamp(rb.velocity.z, -10, 10));
        }
        Vector3 movement = new Vector3((transform.forward.x * input.y) + (transform.right.x * input.x), 0, (transform.forward.z * input.y) + (transform.right.z * input.x));
        rb.AddForce(movement.normalized * speed * Time.fixedDeltaTime * 500 * friction, ForceMode.Force);
    }
}

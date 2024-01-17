using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

public class Playermovement : MonoBehaviour
{
    public static Playermovement instance;

    [Header("Settings")]
    [SerializeField] private float walkspeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float airControl;
    [Header("References")]
    [SerializeField] LayerMask ground;
    [SerializeField] Transform groundCheck;
    [Header("Bobbing")]
    [SerializeField] float BobbingAmplitude;
    [SerializeField] float BobbingSpeed;

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
            if (context.performed)
            {
                isSprinting = true;
            }
            else if (context.canceled)
            {
                isSprinting= false;
            }
    }

    private void Update()
    {
        if(input.sqrMagnitude != 0)
        {
            PlayerCam.instance.transform.localPosition = PlayerCam.instance.transform.localPosition + new Vector3(0, Mathf.Sin(Time.timeSinceLevelLoad * BobbingSpeed * (speed/10)) * BobbingAmplitude * Time.deltaTime, 0);

        }
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

    public void ModifySpeed()
    {
        walkspeed *= 1.5f;
        runSpeed *= 1.5f;

        Invoke(nameof(ResetSpeed), 15f);
    }
    private void ResetSpeed()
    {
        walkspeed /= 1.5f;
        runSpeed /= 1.5f;
    }
}

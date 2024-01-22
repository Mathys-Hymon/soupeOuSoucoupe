using UnityEngine;
using UnityEngine.InputSystem;

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
    [SerializeField] AudioSource footstepSoudSource;
    [Header("Bobbing")]
    [SerializeField] float BobbingAmplitude;
    [SerializeField] float BobbingSpeed;
    [SerializeField] GameObject BobbingRef;
    [Header("Crouch")]
    [SerializeField] private float crouchSpeed;
    [SerializeField] private float crouchHeight;
    [SerializeField] private float normalHeight;
    [Header("Footsteps")]
    [SerializeField] private AudioClip[] GrassWalkSound;
    [SerializeField] private AudioClip[] GrassJumpSound;
    [SerializeField] private AudioClip[] GrassLandingSound;
    [SerializeField] private AudioClip[] WoodWalkSound;
    [SerializeField] private AudioClip[] WoodJumpSound;
    [SerializeField] private AudioClip[] WoodLandingSound;


    private Rigidbody rb;
    private Vector2 input;
    private float speed;
    private bool isGrounded, isSprinting, isJumping, isCrouching, isPaused, canPlaySound = true;
    private Vector3 initialLocalPos;


    public void PlayerCanMove(bool canMove)
    {
        isPaused = !canMove;
    }
    public bool GetPause()
    {
        return isPaused;
    }
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
        if(!isPaused)
        {
            if (isGrounded && context.ReadValueAsButton() && !isJumping)
            {
                AudioClip[] footstepsSounds = null;
                if (floorIsOutside())
                {
                    footstepsSounds = GrassJumpSound;
                }
                else
                {
                    footstepsSounds = WoodJumpSound;
                }
                int randomJumpSound = Random.Range(0, footstepsSounds.Length - 1);
                footstepSoudSource.clip = footstepsSounds[randomJumpSound];
                footstepSoudSource.Play();

                if (isCrouching)
                {
                    isCrouching = false;
                }
                rb.AddForce(new Vector3(0, jumpForce * 25, 0));
                Invoke(nameof(setIsJumping), 0.2f);
            }
        }
    }
    private void setIsJumping()
    {
        isJumping = true;
    } 

    public void Crouch(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isCrouching = true;
        }
        else if (context.canceled)
        {
            isCrouching = false;
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
        if (!isPaused) 
        {
            if (input.sqrMagnitude != 0)
            {
                if (InventoryScript.instance.GetAim() || isCrouching)
                {
                    BobbingRef.transform.localPosition = BobbingRef.transform.localPosition + new Vector3(0, Mathf.Sin(Time.time * BobbingSpeed * (speed)) * (BobbingAmplitude / 10) * Time.deltaTime, 0);
                }
                else
                {
                    BobbingRef.transform.localPosition = BobbingRef.transform.localPosition + new Vector3(0, Mathf.Sin(Time.time * BobbingSpeed * (speed)) * (BobbingAmplitude / 5) * Time.deltaTime, 0);
                }

            }
            else
            {
                BobbingRef.transform.localPosition = new Vector3(0, Mathf.Lerp(BobbingRef.transform.localPosition.y, 0.6f, 6f * Time.deltaTime), 0);
            }
            isGrounded = Physics.CheckSphere(groundCheck.position, 0.1f, ground);
            if (isGrounded && isJumping)
            {
                AudioClip[] landingSound = null;
                if (floorIsOutside())
                {
                    landingSound = GrassLandingSound;
                }
                else
                {
                    landingSound = WoodLandingSound;
                }
                int randomJumpSound = Random.Range(0, landingSound.Length - 1);
                footstepSoudSource.clip = landingSound[randomJumpSound];
                footstepSoudSource.Play();

                isJumping = false;
            }

            if (InventoryScript.instance.GetAim())
            {
                if (isSprinting)
                {
                    speed = runSpeed / 2;
                }
                else if (isCrouching)
                {
                    speed = crouchSpeed / 2;
                }
                else
                {
                    speed = walkspeed / 2;
                }
            }
            else if (isCrouching)
            {
                speed = crouchSpeed;
            }

            else if (!isSprinting && !isCrouching)
            {
                speed = walkspeed;
            }

            else
            {
                speed = runSpeed;
            }

            if (isCrouching)
            {
                gameObject.GetComponent<CapsuleCollider>().height = Mathf.Lerp(gameObject.GetComponent<CapsuleCollider>().height, crouchHeight, (crouchSpeed * 10) * Time.deltaTime);
                groundCheck.transform.localPosition = new Vector3(0, Mathf.Lerp(groundCheck.transform.localPosition.y, -0.44065f, (crouchSpeed * 10) * Time.deltaTime), 0);
            }
            else
            {
                gameObject.GetComponent<CapsuleCollider>().height = Mathf.Lerp(gameObject.GetComponent<CapsuleCollider>().height, normalHeight, (crouchSpeed * 10) * Time.deltaTime);
                groundCheck.transform.localPosition = new Vector3(0, Mathf.Lerp(groundCheck.transform.localPosition.y, -0.8813f, (crouchSpeed * 10) * Time.deltaTime), 0);

            }

            if (canPlaySound && input.sqrMagnitude != 0 && isGrounded && !isJumping)
            {
                canPlaySound = false;

                AudioClip[] footstepsSounds = null;

                if (floorIsOutside())
                {
                    footstepSoudSource.pitch = 1;
                    footstepsSounds = GrassWalkSound;
                }
                else
                {
                    float pitch = Random.Range(0.9f, 1.1f);
                    footstepSoudSource.pitch = pitch;
                    footstepsSounds = WoodWalkSound;
                }

                int footstepNum = Random.Range(0, footstepsSounds.Length - 1);
                footstepSoudSource.clip = footstepsSounds[footstepNum];
                footstepSoudSource.pitch = Random.Range(0.8f, 1f);
                footstepSoudSource.Play();

                Invoke(nameof(ResetFootstep), 3 / speed);
            }
        }
    }

    private bool floorIsOutside()
    {
        Collider[] hitFloor;
        hitFloor = Physics.OverlapSphere(groundCheck.position, 0.1f, ground);

        if(hitFloor[0].gameObject.name == "Terrain")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void ResetFootstep()
    {
        canPlaySound = true;
    }

    private void FixedUpdate()
    {
        if(!isPaused)
        {
            float friction;
            if (isGrounded && !isJumping)
            {
                friction = 1;
                rb.drag = 8;
            }
            else
            {
                friction = 0.2f;
                if (speed == runSpeed)
                {
                    rb.drag = 0.01f * airControl;
                }
                else
                {
                    rb.drag = 0.05f * airControl;
                }
                if (rb.velocity.y < 0.5f)
                {
                    rb.AddForce(new Vector3(0, -250 * Time.fixedDeltaTime, 0));
                }
                rb.velocity = new Vector3(Mathf.Clamp(rb.velocity.x, -10, 10), Mathf.Clamp(rb.velocity.y, -10, 10), Mathf.Clamp(rb.velocity.z, -10, 10));
            }
            Vector3 movement = new Vector3((transform.forward.x * input.y) + (transform.right.x * input.x), 0, (transform.forward.z * input.y) + (transform.right.z * input.x));
            rb.AddForce(movement.normalized * speed * Time.fixedDeltaTime * 500 * friction, ForceMode.Force);
        }
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

    public Vector2 getInput()
    {
        return input;
    }
}


using UnityEngine;

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
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.2f, ground);
        input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (input.sqrMagnitude != 0)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                speed = runSpeed;
            }
            else
            {
                speed = walkspeed;
            }
        }

        if(isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = new Vector3(0, jumpForce, 0);
        }
    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3((transform.forward.x * input.y * speed * Time.deltaTime) + (transform.right.x * input.x * speed * Time.deltaTime), 0, (transform.forward.z * input.y * speed * Time.deltaTime) + (transform.right.z * input.x * speed * Time.deltaTime));
        rb.MovePosition(transform.position + movement);
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Playermovement : MonoBehaviour
{
    public static Playermovement instance;
    [SerializeField] private float walkspeed, runSpeed, jumpForce, airControl;
    [SerializeField] LayerMask ground;
    [SerializeField] Transform groundCheck, weaponTransform;


    [Header("Weapons Settings")]
    [SerializeField] private float switchWeaponSpeed;
    [SerializeField] private float throwWeaponForce;
    [Header("Sway Settings")]
    [SerializeField] private float swaySpeed;
    [SerializeField] private float swayMultiplier;
    [SerializeField] private Vector2 swayClamp;

    [Header("Recoil Settings")]
    [SerializeField] private Vector3 recoil;
    [SerializeField] private float snappiness;
    [SerializeField] private float returnSpeed;

    private Rigidbody rb;
    private Vector2 input;
    private float speed;
    private bool isGrounded, isAiming;
    private List<WeaponScript> weapons;
    private int actualWeapon = 1;
 

    private void Start()
    {
        instance = this;

        weapons = new List<WeaponScript>();
        rb = GetComponent<Rigidbody>();
        speed = walkspeed;
    }

    public void playerMovement(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
    }

    public void changeWeapon(InputAction.CallbackContext context)
    {
        if(weapons.Count > 0)
        {
            if(weapons.Count == 1)
            {
                actualWeapon = 0;
                print(actualWeapon);
            }
            else
            {
                if (context.ReadValue<float>() > 0)
                {
                    actualWeapon++;
                    if (actualWeapon > weapons.Count - 1)
                    {
                        actualWeapon = 0;
                    }
                    print(actualWeapon);
                }
                else if (context.ReadValue<float>() < 0)
                {
                    actualWeapon--;
                    if (actualWeapon < 0)
                    {
                        actualWeapon = weapons.Count - 1;
                    }
                    print(actualWeapon);
                }
                MoveWeapons();
               
            }        
        }
        
    }

    public void jumpInput(InputAction.CallbackContext context)
    {
        if (isGrounded && context.ReadValueAsButton())
        {
            rb.AddForce(new Vector3(0,jumpForce*25, 0));
        }
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton())
        {
            if (weapons.Count > actualWeapon)
            {
                weapons[actualWeapon].ShootButtonPressed(true);
            }
        }
        else
        {
            if (weapons.Count > actualWeapon)
            {
                weapons[actualWeapon].ShootButtonPressed(false);
            }
        }
    }
    public void Reload(InputAction.CallbackContext context)
    {
        if (context.performed && weapons.Count >= actualWeapon)
        {
            weapons[actualWeapon].Reload();
        }
    }

    private void MoveWeapons()
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            if (i != actualWeapon)
            {
                weapons[i].transform.localPosition = new Vector3(0.218002319f, -0.265999913f, 0.862998962f);
                weapons[i].transform.localRotation = Quaternion.Euler(84, 75, 165);
            }
        }
    }

    

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<WeaponScript>() != null && !weapons.Contains(other.gameObject.GetComponent<WeaponScript>()))
        {
            weapons.Add(other.gameObject.GetComponent<WeaponScript>());
            weapons[weapons.Count - 1].transform.parent = weaponTransform;
            weapons[weapons.Count - 1].GetComponent<BoxCollider>().enabled = false;
            weapons[weapons.Count - 1].transform.position = weaponTransform.position;
            weapons[weapons.Count - 1].transform.rotation = weaponTransform.rotation;
            weapons[weapons.Count - 1].gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            actualWeapon = weapons.Count - 1;
            MoveWeapons();
        }
    }

    public void KickWeapon(InputAction.CallbackContext context)
    {
        if (context.performed && weapons.Count > 0)
        {
            weapons[actualWeapon].GetComponent<BoxCollider>().enabled = true;
            weapons[actualWeapon].transform.parent = null;
            weapons[actualWeapon].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            weapons[actualWeapon].GetComponent<Rigidbody>().AddForce(((transform.forward * throwWeaponForce*100) + (transform.up * throwWeaponForce*30)) * Time.deltaTime, ForceMode.Impulse);
            weapons.Remove(weapons[actualWeapon]);
            if (actualWeapon >= weapons.Count)
            {
                actualWeapon = 0;
            }
        }
       
    }

    public void runMovement(InputAction.CallbackContext context)
    {
        if (input.sqrMagnitude != 0)
        {
            if (context.performed)
            {
                speed = runSpeed;
            }
            else if (context.canceled)
            {
                speed = walkspeed;
            }
        }
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.1f, ground);
        if (actualWeapon <= weapons.Count-1)
        {
            Quaternion targetRotQuaternion = Quaternion.AngleAxis(Mathf.Clamp(-PlayerCam.instance.getMouseInput().y * swayMultiplier, -swayClamp.y, swayClamp.y), Vector3.right) * Quaternion.AngleAxis(Mathf.Clamp(PlayerCam.instance.getMouseInput().x * swayMultiplier, -swayClamp.x, swayClamp.x), Vector3.up);
            weapons[actualWeapon].transform.localRotation = Quaternion.Lerp(weapons[actualWeapon].transform.localRotation, targetRotQuaternion, swaySpeed * Time.deltaTime);
            weapons[actualWeapon].transform.localPosition = Vector3.Lerp(weapons[actualWeapon].transform.localPosition, new Vector3(0, 0, 0), switchWeaponSpeed * Time.deltaTime);
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

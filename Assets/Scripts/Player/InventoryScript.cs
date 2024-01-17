using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryScript : MonoBehaviour
{
    public static InventoryScript instance;

    [Header("Weapons Settings")]
    [SerializeField] private float switchWeaponSpeed;
    [SerializeField] private float throwWeaponForce;
    [Header("Sway Settings")]
    [SerializeField] private float swaySpeed;
    [SerializeField] private float swayMultiplier;
    [SerializeField] private Vector2 swayClamp;

    private List<WeaponScript> weapons;
    private int actualWeapon = 1;
    private Transform targetTransform;
    private bool isAiming, canShoot;
    private Transform weaponTransform;
    private void Start()
    {
        instance = this;

        weapons = new List<WeaponScript>();
        targetTransform = weaponTransform;
    }

    public bool GetAim()
    {
        return isAiming;
    }

    public void changeWeapon(InputAction.CallbackContext context)
    {
        if (weapons.Count > 0)
        {
            if (weapons.Count == 1)
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
                }
                else if (context.ReadValue<float>() < 0)
                {
                    actualWeapon--;
                    if (actualWeapon < 0)
                    {
                        actualWeapon = weapons.Count - 1;
                    }
                }
                MoveWeapons();

            }
        }

    }

    public void Shoot(InputAction.CallbackContext context)
    {
        if (context.performed && canShoot)
        {
            if (weapons.Count > actualWeapon)
            {
                weapons[actualWeapon].ShootButtonPressed(true);
            }
        }
        else if (context.canceled || !canShoot)
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<WeaponScript>() != null && !weapons.Contains(other.gameObject.GetComponent<WeaponScript>()))
        {
            weapons.Add(other.gameObject.GetComponent<WeaponScript>());
            weapons[weapons.Count - 1].transform.parent = weaponTransform;
            weapons[weapons.Count - 1].GetComponent<BoxCollider>().enabled = false;
            weapons[weapons.Count - 1].GetComponent<SphereCollider>().enabled = false;
            weapons[weapons.Count - 1].transform.position = weaponTransform.position;
            weapons[weapons.Count - 1].transform.rotation = weaponTransform.rotation;
            weapons[weapons.Count - 1].gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            actualWeapon = weapons.Count - 1;
            MoveWeapons();
        }
    }

    private void MoveWeapons()
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            if (i != actualWeapon)
            {
                weapons[i].transform.localPosition = new Vector3(-0.377999991f, -0.209999993f, -0.885999978f);
                weapons[i].transform.localRotation = Quaternion.Euler(272.603821f, 106.006058f, 180.000229f);
            }
        }
    }

    public void KickWeapon(InputAction.CallbackContext context)
    {

        if (context.performed && weapons.Count > 0)
        {
            weapons[actualWeapon].GetComponent<BoxCollider>().enabled = true;
            weapons[actualWeapon].transform.parent = null;
            weapons[actualWeapon].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            weapons[actualWeapon].GetComponent<Rigidbody>().AddForce(((transform.forward * throwWeaponForce) + (transform.up * throwWeaponForce * 0.3f)), ForceMode.Impulse);
            weapons[actualWeapon].kickWeapon();
            weapons.Remove(weapons[actualWeapon]);
            if (actualWeapon >= weapons.Count)
            {
                actualWeapon = 0;
            }
        }

    }

    public void Aim(InputAction.CallbackContext context)
    {
        if (context.performed && canShoot)
        {
            if (weapons.Count > actualWeapon)
            {
                isAiming = true;
            }
        }
        else if (context.canceled)
        {
            if (weapons.Count > actualWeapon)
            {
                isAiming = false;
            }
        }
    }
    private void Update()
    {
        if (actualWeapon <= weapons.Count - 1)
        {
            if (PlayerCam.instance.AimCenter().distance != 0)
            {
                if (PlayerCam.instance.AimCenter().distance <= 1)
                {
                    canShoot = false;
                }
                else
                {
                    canShoot = true;
                }

                if (!isAiming)
                {
                    targetTransform.LookAt(PlayerCam.instance.AimCenter().point);
                    weaponTransform.rotation = Quaternion.Lerp(weaponTransform.rotation, targetTransform.rotation, 1 * Time.deltaTime);
                }
                else
                {
                    weaponTransform.localRotation = Quaternion.identity;
                }
            }
            else
            {
                weaponTransform.localRotation = Quaternion.identity;
            }

            if (isAiming)
            {
                Quaternion targetRotQuaternion = Quaternion.AngleAxis(Mathf.Clamp(-PlayerCam.instance.getMouseInput().y * swayMultiplier * 0.1f, -swayClamp.y, swayClamp.y), Vector3.right) * Quaternion.AngleAxis(Mathf.Clamp(PlayerCam.instance.getMouseInput().x * swayMultiplier * 0.1f, -swayClamp.x, swayClamp.x), Vector3.up);
                weapons[actualWeapon].transform.localRotation = Quaternion.Lerp(weapons[actualWeapon].transform.localRotation, targetRotQuaternion, swaySpeed * Time.deltaTime);
                weapons[actualWeapon].transform.localPosition = Vector3.Lerp(weapons[actualWeapon].transform.localPosition, weapons[actualWeapon].GetAimPos(), switchWeaponSpeed * Time.deltaTime);
            }
            else
            {
                Quaternion targetRotQuaternion = Quaternion.AngleAxis(Mathf.Clamp(-PlayerCam.instance.getMouseInput().y * swayMultiplier, -swayClamp.y, swayClamp.y), Vector3.right) * Quaternion.AngleAxis(Mathf.Clamp(PlayerCam.instance.getMouseInput().x * swayMultiplier, -swayClamp.x, swayClamp.x), Vector3.up);
                weapons[actualWeapon].transform.localRotation = Quaternion.Lerp(weapons[actualWeapon].transform.localRotation, targetRotQuaternion, swaySpeed * Time.deltaTime);
                weapons[actualWeapon].transform.localPosition = Vector3.Lerp(weapons[actualWeapon].transform.localPosition, new Vector3(0, 0, 0), switchWeaponSpeed * Time.deltaTime);
            }

        }
    }
}

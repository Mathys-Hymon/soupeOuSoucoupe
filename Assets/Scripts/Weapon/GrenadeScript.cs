using UnityEngine;
using UnityEngine.InputSystem;

public class GrenadeScript : MonoBehaviour
{
    [SerializeField] private float throwGrenadeForce;
    [SerializeField] private int grenadeAmount;

    [SerializeField] private Transform grenadePosition;
    [SerializeField] private GameObject grenade;
    [SerializeField] private float grenadeDelay;


    private void Update()
    {
        grenadePosition.LookAt(PlayerCam.instance.AimCenter().point);
    }

    public void KickGrenade(InputAction.CallbackContext context)
    {

        if (context.performed && grenadeAmount > 0)
        {
            grenadeAmount--;
            GameObject lastGrenade = Instantiate(grenade, grenadePosition.position + transform.forward, grenadePosition.rotation);
            if (PlayerCam.instance.AimCenter().distance == 0)
            {
                lastGrenade.GetComponent<Rigidbody>().AddForce(((transform.forward * throwGrenadeForce) + (transform.up * throwGrenadeForce * 0.3f)), ForceMode.Impulse);
            }
            else
            {
                lastGrenade.GetComponent<Rigidbody>().AddForce(((transform.forward * Mathf.Clamp(PlayerCam.instance.AimCenter().distance, 0f, throwGrenadeForce)) + (transform.up * Mathf.Clamp(PlayerCam.instance.AimCenter().distance, 0f, throwGrenadeForce) * 0.3f)), ForceMode.Impulse);
            }

            lastGrenade.GetComponent<GrenadeExplosion>().InvokeExplosion(grenadeDelay);
        }

    }
}

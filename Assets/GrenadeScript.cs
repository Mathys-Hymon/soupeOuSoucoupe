using UnityEngine;
using UnityEngine.InputSystem;

public class GrenadeScript : MonoBehaviour
{
    [SerializeField] private float throwGrenadeForce;
    [SerializeField] private int grenadeAmount;

    [SerializeField] private GameObject grenade;
    [SerializeField] private float grenadeDelay;


    public void KickGrenade(InputAction.CallbackContext context)
    {

        if (context.performed && grenadeAmount > 0)
        {
            
            GameObject lastGrenade = Instantiate(grenade, transform.position + transform.forward, Quaternion.identity);
            lastGrenade.GetComponent<Rigidbody>().AddForce(((transform.forward * throwGrenadeForce) + (transform.up * throwGrenadeForce * 0.3f)), ForceMode.Impulse);
            lastGrenade.GetComponent<GrenadeExplosion>().InvokeExplosion(grenadeDelay);
        }

    }
}

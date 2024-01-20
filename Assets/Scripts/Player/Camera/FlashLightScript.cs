using UnityEngine;
using UnityEngine.InputSystem;

public class FlashLightScript : MonoBehaviour
{
    private bool lightON = true;
    private Light lightRef;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        lightRef = GetComponent<Light>();
    }
    void Update()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, PlayerCam.instance.transform.rotation, 0.06f);
        transform.position = PlayerCam.instance.transform.position + transform.forward*0.4f;
    }


    public void OnOffFlashLight(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            if (lightON)
            {
                lightON = false;
                lightRef.enabled = false;
            }
            else
            {
                lightON = true;
                lightRef.enabled = true;
            }
            float RandomPitch = Random.Range(1f, 1.3f);
            audioSource.pitch = RandomPitch;
            audioSource.Play();
        }
    }
}

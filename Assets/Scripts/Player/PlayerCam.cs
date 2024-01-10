using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCam : MonoBehaviour
{
    [SerializeField] private float mouseSensibility = 1f;
    private float cameraVerticalRotation;
    private float offsetY;
    private Vector2 input;
    private void Start()
    {
        offsetY = transform.position.y - Playermovement.instance.transform.position.y;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void mousePosition(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
    }

    private void Update()
    {
        transform.position = Playermovement.instance.transform.position + new Vector3( transform.forward.x*0.2f, offsetY, transform.forward.z*0.2f);

        cameraVerticalRotation -= input.y *10 *  mouseSensibility * Time.deltaTime;
        cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, -90f, 75f);
        transform.localEulerAngles = Vector3.right * cameraVerticalRotation;

        Playermovement.instance.transform.Rotate(Vector3.up * 10 * input.x * mouseSensibility * Time.deltaTime);
    }
}

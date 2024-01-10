using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCam : MonoBehaviour
{
    [SerializeField] private float mouseSensibility = 2f;
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
        transform.position = Playermovement.instance.transform.position + new Vector3( transform.forward.x, offsetY, transform.forward.z);

        cameraVerticalRotation -= input.y;
        cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, -90f, 75f);
        transform.localEulerAngles = Vector3.right * cameraVerticalRotation;

        Playermovement.instance.transform.Rotate(Vector3.up * input.x);
    }
}

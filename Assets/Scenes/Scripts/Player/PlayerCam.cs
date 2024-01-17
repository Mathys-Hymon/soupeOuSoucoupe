using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCam : MonoBehaviour
{
    public static PlayerCam instance;

    [SerializeField] private float mouseSensibility = 1f;
    private float cameraVerticalRotation;
    private float offsetY;
    private Vector2 input;
    private void Start()
    {
        instance = this;

        offsetY = transform.position.y - Playermovement.instance.transform.position.y;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void mousePosition(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
    }
    
    public Vector2 getMouseInput()
    {
        return input;
    }
    public RaycastHit AimCenter()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, ~Playermovement.instance.gameObject.layer))
        {
            return hit;
        }
        else
        {
            return hit;
        }
        
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

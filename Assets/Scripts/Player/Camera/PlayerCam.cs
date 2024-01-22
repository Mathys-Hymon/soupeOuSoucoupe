using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCam : MonoBehaviour
{
    public static PlayerCam instance;

    [SerializeField] private float mouseSensibility = 1f;
    private float cameraVerticalRotation;
    private Vector2 input;
    private void Start()
    {
        instance = this;

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
        if(!Playermovement.instance.GetPause()) 
        {
            cameraVerticalRotation -= ((input.y / 10) * mouseSensibility);
            cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, -90f, 75f);
            var VerticalRot = Quaternion.AngleAxis(cameraVerticalRotation, Vector3.right);
            gameObject.transform.parent.transform.localRotation = VerticalRot;

            Playermovement.instance.transform.Rotate((Vector3.up * (input.x / 10) * mouseSensibility));
        }
    }
}

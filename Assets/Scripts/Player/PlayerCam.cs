using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    [SerializeField] private float mouseSensibility = 2f;
    private float cameraVerticalRotation;
    private float offsetY;
    private void Start()
    {
        offsetY = transform.position.y - Playermovement.instance.transform.position.y;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        transform.position = Playermovement.instance.transform.position + new Vector3( transform.forward.x, offsetY, transform.forward.z);

        float inputX = Input.GetAxis("Mouse X")*mouseSensibility;
        float inputY = Input.GetAxis("Mouse Y")*mouseSensibility;

        cameraVerticalRotation -= inputY;
        cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, -90f, 75f);
        transform.localEulerAngles = Vector3.right * cameraVerticalRotation;

        Playermovement.instance.transform.Rotate(Vector3.up * inputX);
    }
}

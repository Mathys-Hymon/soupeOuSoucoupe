using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    [SerializeField] private float mouseSensibility = 2f;
    private float cameraVerticalRotation;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        transform.position = Playermovement.instance.transform.position;

        float inputX = Input.GetAxis("Mouse X")*mouseSensibility;
        float inputY = Input.GetAxis("Mouse Y")*mouseSensibility;

        cameraVerticalRotation -= inputY;
        cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, -90f, 90f);
        transform.localEulerAngles = Vector3.right * cameraVerticalRotation;

        Playermovement.instance.transform.Rotate(Vector3.up * inputX);
    }
}

using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    private Vector2 movementInput;

    new private Rigidbody2D rigidbody;

    public int cameraSpeed = 6;

    public int cameraSpeedFast = 12;

    private int cameraSpeedMultiplier;

    private bool isFast = false;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        cameraSpeedMultiplier = cameraSpeed;
    }

    private void Update()
    {
        rigidbody.linearVelocity = movementInput * cameraSpeedMultiplier;
    }

    public void Move(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    public void SpeedUp(InputAction.CallbackContext context)
    {   

        if (context.started)
            return;
        else if (!isFast)
        {
            cameraSpeedMultiplier = cameraSpeedFast;

            isFast = true;
        }
        else
        {
            cameraSpeedMultiplier = cameraSpeed;

            isFast = false;
        }
    }

}

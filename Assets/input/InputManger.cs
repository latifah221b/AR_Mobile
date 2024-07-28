using UnityEngine;
using UnityEngine.InputSystem;

public class InputManger : MonoBehaviour
{
    private TouchControl touchControl;

    private void Awake() { 
        touchControl = new TouchControl();
    }

    private void OnEnable() {
        touchControl.Enable();
    }
    private void OnDisable()
    {
        touchControl.Disable();
    }
     void Start() {
        touchControl.Touch.TouchInput.started += ctx => starttouch(ctx);
        touchControl.Touch.TouchInput.started -= ctx => endtouch(ctx);
    }

    public void starttouch(InputAction.CallbackContext context)
    {
        Debug.Log("Touch started: "+ context.ReadValue<Vector2>());
    }

    public void endtouch(InputAction.CallbackContext context)
    {

    }
}

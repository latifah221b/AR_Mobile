using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    private TouchControl touchControl;

    private void Awake()
    {

        // Check if an instance of InputHandler already exists
        if (FindObjectsOfType<InputHandler>().Length > 1)
        {
            Destroy(gameObject); // Destroy duplicate instances
            return;
        }

        // Make this instance persistent
        DontDestroyOnLoad(gameObject);

        touchControl = new TouchControl();

    }

    private void OnEnable()
    {
        touchControl.Enable();
    }

    private void OnDisable()
    {
        touchControl.Disable();
        touchControl.Touch.TouchInput.started -= ctx => OnTap(ctx);
    }

    private void OnDestroy()
    {
        touchControl.Disable();
        touchControl.Touch.TouchInput.started -= ctx => OnTap(ctx);
    }

    void Start()
    {
        touchControl.Touch.TouchInput.started += ctx => OnTap(ctx);
    }


    private void OnTap(InputAction.CallbackContext context)
    {
        //Debug.Log("OnTap");
        NotifyTap(context);

    }

    private void NotifyTap(InputAction.CallbackContext context)
    {
        //Debug.Log("NotifyTap");
        Vector2 tapPosition = context.ReadValue<Vector2>();
        GameObjectManager.Instance.NotifyAll(tapPosition);
    }
}
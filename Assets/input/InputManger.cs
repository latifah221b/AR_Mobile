using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManger : MonoBehaviour
{
    private TouchControl touchControl;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private GameObject _visuals;

    private void Awake() { 
        touchControl = new TouchControl();
    }

    private void OnEnable() {
        touchControl.Enable();
        touchControl.Touch.TouchInput.started += ctx => starttouch(ctx);
        touchControl.Touch.TouchInput.started -= ctx => endtouch(ctx);
    }
    private void OnDisable()
    {
        touchControl.Disable();
    }
     void Start() {
        
    }

    public void starttouch(InputAction.CallbackContext context)
    {
        Debug.Log("The time: " + context.time);
        Debug.Log("The phase: " + context.phase);
        Debug.Log("The Duration: " + context.duration);

        Debug.Log("The action: " + context.action.name);

        Debug.Log("The performed: " + context.performed);

        Debug.Log("The control: " + context.control.name);

        handleTap(context.ReadValue<Vector2>());
        Debug.Log("Touch started: "+ context.ReadValue<Vector2>());
    }

    public void endtouch(InputAction.CallbackContext context)
    {

    }

    private void handleTap(Vector2 screenpos)
    {
        RaycastHit hit;
        if (screenpos != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(screenpos);
            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.collider != null && 
                    hit.collider.gameObject != null && 
                    hit.collider.gameObject == _visuals) 
                {
                   // _canvas.gameObject.SetActive(true);
                }

            }
        }
    }
}

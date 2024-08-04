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
        Debug.Log("Awake: Input");
    }

    private void OnEnable() {
        Debug.Log("OnEnable: Input");
        touchControl.Enable();
    }
    private void OnDisable()
    {
        Debug.Log("OnDisable: Input");
        touchControl.Disable();
    }
     void Start() {
        Debug.Log("Start: Input");
        
        touchControl.Touch.TouchInput.started += ctx => starttouch(ctx);
        touchControl.Touch.TouchInput.started -= ctx => endtouch(ctx);
    }

    public void starttouch(InputAction.CallbackContext context)
    {
        handleTap(context.ReadValue<Vector2>());
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
                    Debug.Log("The canvas will be activiated");
                    _canvas.gameObject.SetActive(true);
                }

            }
        }
    }
}

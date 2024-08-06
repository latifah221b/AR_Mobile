using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManger : MonoBehaviour
{
   
    [SerializeField] private Canvas _canvas;
    [SerializeField] private GameObject _visuals;
    [SerializeField] private GameObject _rocket_part_attached;

    private TouchControl touchControl;
    private GameObject customPart;
    [SerializeField] private TextMeshProUGUI _Main_Quest_txt;

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

        // look for custom part 
        if (_rocket_part_attached != null) {
            var mesh_col = _rocket_part_attached.GetComponentInChildren<MeshCollider>();
            if (mesh_col != null) { Debug.Log("The meshCollider has been found");
                customPart = mesh_col.gameObject; }

        }
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
                    hit.collider.gameObject != null ) 
                {
                    check(hit.collider.gameObject);
                }

            }
        }
    }

    private void check(GameObject collider_game)

    {
        if(_visuals != null && collider_game == _visuals 
            && _canvas != null) {

            Debug.Log("The canvas will be activiated");
            _canvas.gameObject.SetActive(true);

        } else if(customPart != null && 
            customPart == collider_game)
        {
             _rocket_part_attached.SetActive(false);
            
            if(_Main_Quest_txt != null)
            {
                int value_int = Int32.Parse(_Main_Quest_txt.text);
                value_int++;
                _Main_Quest_txt.text = value_int.ToString();

            }
            Destroy(this.gameObject);

            //  Destroy(_rocket_part_attached);

            //TODO: update the main screen & inventory
        }




    }
}

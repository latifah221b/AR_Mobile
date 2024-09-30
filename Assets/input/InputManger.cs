using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManger : MonoBehaviour
{
   
    [SerializeField] private Canvas _canvas;
    [SerializeField] private GameObject _visuals;
    [SerializeField] private GameObject _rocket_part_attached;
    [SerializeField] private TextMeshProUGUI _Main_Quest_txt;
    [SerializeField] private GameObject [] _final_dialogs ; 

    private TouchControl touchControl;
    private GameObject customPart;
    private float timeThresholdMicroseconds = 30000f; 
    private bool first_time = true; 

    public void set_Main_Quest_txt(TextMeshProUGUI txt_ref)
    {
        _Main_Quest_txt = txt_ref;
    }

    private void Awake() {
        touchControl = new TouchControl();
        Debug.Log("Awake: Input");
    }
    private void OnDestroy()
    {
        touchControl.Disable();
        touchControl.Touch.TouchInput.started -= ctx => starttouch(ctx);
    }

    private void OnEnable() {
        Debug.Log("OnEnable: Input");
        touchControl.Touch.TouchInput.Enable();
        // touchControl.Touch.TouchInput.started += ctx => starttouch(ctx);
         touchControl.Touch.TouchInput.started += ctx => starttouchInit(ctx);
        StartCoroutine(Coroutine());
    }

    IEnumerator Coroutine()
    {
        yield return new WaitForSeconds(2);
        touchControl.Touch.TouchInput.started -= ctx => starttouchInit(ctx);
        touchControl.Touch.TouchInput.started += ctx => starttouch(ctx);


    }

    private void OnDisable()
    {
        Debug.Log("OnDisable: Input");
        touchControl.Touch.TouchInput.Disable();
        touchControl.Touch.TouchInput.started -= ctx => starttouch(ctx);
    }
     void Start() {
        Debug.Log("Start: Input");

        // look for custom part 
        if (_rocket_part_attached != null) {
            var mesh_col = _rocket_part_attached.GetComponentInChildren<MeshCollider>();
            if (mesh_col != null) { Debug.Log("The meshCollider has been found");
                customPart = mesh_col.gameObject; }

        }
    }

    public void starttouch(InputAction.CallbackContext context) {

        // Debug.Log($"Tap diff: {old_tap_or_fresh_ones(context)} microseconds");
       // Debug.Log($"first time: {first_time} ");

       // if (old_tap_or_fresh_ones(context)> 500f)
       // {
            handleTap(context.ReadValue<Vector2>());
       // }
      //  first_time = false;
    }


    public void starttouchInit(InputAction.CallbackContext context)
    {
        Debug.Log(context.ToString());
    }

    private float old_tap_or_fresh_ones(InputAction.CallbackContext context)
    {
        float tapTime = (float)context.startTime * 1000000f;
        float currentTime = (float) Time.realtimeSinceStartup * 1000000f;

        Debug.Log($"Tap detected at: {tapTime} microseconds");
        Debug.Log($"Tap Current Time: {currentTime} microseconds");

        return Mathf.Abs(currentTime - tapTime);

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
           

            //  Destroy(_rocket_part_attached);

            //TODO: update the main screen & inventory

            // check if we reached the final count:
            int value_int_1 = Int32.Parse(_Main_Quest_txt.text);
            if (value_int_1 >= 3)
            {
                StartCoroutine(final_logic());
            }
            else
            {
                Destroy(this.gameObject);
            }

        }

    }



    IEnumerator final_logic()
    {
        _final_dialogs[0].SetActive(true);
        yield return new WaitForSecondsRealtime(3);
        _final_dialogs[0].SetActive(false);

        yield return new WaitForSecondsRealtime(1);

        _final_dialogs[1].SetActive(true);
        yield return new WaitForSecondsRealtime(3);
        _final_dialogs[1].SetActive(false);
        Destroy(this.gameObject);


    }
}

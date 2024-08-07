
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Input_Manger_collectable : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _Collectable_count_txt;
    [SerializeField] private Transform _rocket_Transform;
    [SerializeField] private GameObject _star_prefab;

    private TouchControl touchControl;

    private float spawnRadiusMin = 1f;
    private float spawnRadiusMax = 5f;

    private void Awake()
    {
        touchControl = new TouchControl();
        Debug.Log("Awake: Input");
    }

    private void OnEnable()
    {
        Debug.Log("OnEnable: Input");
        touchControl.Enable();
    }
    private void OnDisable()
    {
        Debug.Log("OnDisable: Input");
        touchControl.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start: Input");

        touchControl.Touch.TouchInput.started += ctx => starttouch(ctx);
        touchControl.Touch.TouchInput.started -= ctx => endtouch(ctx);


        // Get a Random Pos .. 
      var pos =  GetRandomSpawnPosition(_rocket_Transform.position);
        // init 
        _star_prefab.transform.position = pos;
        Instantiate(_star_prefab, pos, Quaternion.identity);
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
                    hit.collider.gameObject != null)
                {
                    check(hit.collider.gameObject);
                }

            }
        }
    }

    private void check(GameObject collider_game)

    {
       if(collider_game != null && collider_game.tag =="star") { 
            Destroy(collider_game);
            string text = _Collectable_count_txt.text;
            int value = System.Int32.Parse(text);
            value++;
            _Collectable_count_txt.text = value.ToString();

        }
    }

    private Vector3 GetRandomSpawnPosition(Vector3 pos)
    {
        Vector3 randomPosition = (Vector3)Random.insideUnitCircle;
        return pos + randomPosition.normalized * spawnRadiusMin + randomPosition * (spawnRadiusMax - spawnRadiusMin);
    }
}
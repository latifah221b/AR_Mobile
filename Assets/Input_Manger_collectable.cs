
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;


public class Input_Manger_collectable : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _Collectable_count_txt;
    [SerializeField] private Transform _rocket_Transform;
    [SerializeField] private GameObject _star_prefab;

    private int star_to_spawn = 5 ; 

    private TouchControl touchControl;

    private float spawnRadiusMin = 1f;
    private float spawnRadiusMax = 5f;

    private Vector3 _leftpos = new Vector3(-5f, 0f, 0.77f); 
    private Vector3 _rightpos = new Vector3(5f, 0f, 0.77f);

    private Vector3 _uppos = new Vector3(0.06f, 0f, 5f);
    private Vector3 _downpos = new Vector3(0.06f, 0f, -5f);

    private Vector3[] list_of_ref;
    private int index_ref = 0;

    private Vector3 boxSize = new  Vector3(1,1,1);

    private float timer = 50f;

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

        list_of_ref = new Vector3[4] { _leftpos, _rightpos, _uppos, _downpos };

        touchControl.Touch.TouchInput.started += ctx => starttouch(ctx);
        touchControl.Touch.TouchInput.started -= ctx => endtouch(ctx);


        StartCoroutine(spawn_all_object());

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
       if(collider_game != null && collider_game.tag =="star_box") {
            Destroy(collider_game.transform.parent.gameObject);
            Destroy(collider_game);

            string text = _Collectable_count_txt.text;
            int value = System.Int32.Parse(text);
            value++;
            _Collectable_count_txt.text = value.ToString();

        }
    }

    private Vector3 GetRandomSpawnPosition(Vector3 pos)
    {
        Vector3 randomPosition = (Vector3) UnityEngine.Random.insideUnitCircle;
        return pos + randomPosition.normalized * spawnRadiusMin + randomPosition * (spawnRadiusMax - spawnRadiusMin);
    }

    private void spawn_star(Vector3 pos_arg)
    {
        // Get a Random Pos .. 
        var pos = GetRandomSpawnPosition(pos_arg);
         pos.y = -0.1f;

        if(IsPositionValid(pos))
        {
            SpawnObject(pos);
        }

      

    }

    Vector3 GetPosition() {
        Vector3 randomPos;
        randomPos.x = UnityEngine.Random.Range(1f, 5f);
        randomPos.y = -0.1f;
        randomPos.z = UnityEngine.Random.Range(1f, 5f);

        return randomPos;
    }

    void SpawnObject(Vector3 position)
    {
        // init 
        Instantiate(_star_prefab, position, Quaternion.identity);
    }
    bool IsPositionValid(Vector3 pos)
    {
        Collider[] colliders = Physics.OverlapBox(pos, boxSize / 2);
      
        return colliders.Length == 0; 
    }
   

    IEnumerator spawn_all_object()
    {
        for (int i = 0; i < star_to_spawn; i++)
        {
            spawn_star(list_of_ref[index_ref]);
            index_ref++;
            if (index_ref >= list_of_ref.Length)
            {
                index_ref = 0;
            }


        }

        yield return new WaitForSeconds(timer);
        
        StartCoroutine(spawn_all_object());
    }
}

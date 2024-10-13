
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;


public class CollectableSpawner : MonoBehaviour
{
    //[SerializeField] private TextMeshProUGUI _Collectable_count_txt;
    [SerializeField] private GameObject _ObjectPrefab;

    private int NumberOfObjectToSpawn = 5 ; 

  //  private TouchControl touchControl;

    private float spawnRadiusMin = 1f;
    private float spawnRadiusMax = 5f;

    private Vector3 _leftpos = new Vector3(-5f, 0f, 0.77f); 
    private Vector3 _rightpos = new Vector3(5f, 0f, 0.77f);

    private Vector3 _uppos = new Vector3(0.06f, 0f, 5f);
    private Vector3 _downpos = new Vector3(0.06f, 0f, -5f);

    private Vector3[] listOfReferencePositions;
    private int ReferencePositionsCursor = 0;

    private Vector3 boxSize = new  Vector3(1,1,1);

    private float TimerDuration = 50f;


   // private string _tag = "star_box";

    private void Awake()
    {
       // touchControl = new TouchControl();
    }

    private void OnEnable()
    {
       // touchControl.Enable();
    }
    private void OnDisable()
    {
        //touchControl.Disable();
       // touchControl.Touch.TouchInput.started -= ctx => starttouch(ctx);
    }
    private void OnDestroy()
    {
       // touchControl.Disable();
       // touchControl.Touch.TouchInput.started -= ctx => starttouch(ctx);
    }

    // Start is called before the first frame update
    void Start()
    {
       // Debug.Log("Start: Input");

        listOfReferencePositions = new Vector3[4] { _leftpos, _rightpos, _uppos, _downpos };

       // touchControl.Touch.TouchInput.started += ctx => starttouch(ctx);

        StartCoroutine(spawn_all_object());

    }

    // The function will spawn a list of a specified object,
    // generating a new objects every "timer" seconds.  

    IEnumerator spawn_all_object()
    {
        for (int i = 0; i < NumberOfObjectToSpawn; i++)
        {
            spawnObject(listOfReferencePositions[ReferencePositionsCursor]);
            ReferencePositionsCursor++;

            if (ReferencePositionsCursor >= listOfReferencePositions.Length)
            {
                ReferencePositionsCursor = 0;
            }
        }

        yield return new WaitForSeconds(TimerDuration);
        StartCoroutine(spawn_all_object());
    }



    // public void starttouch(InputAction.CallbackContext context)
    // {

    //   handleTap(context.ReadValue<Vector2>());
    //}



    // private void handleTap(Vector2 screenpos)
    // {
    //    RaycastHit hit;
    //    if (screenpos != null)
    //   {
    //     Ray ray = Camera.main.ScreenPointToRay(screenpos);
    //      if (Physics.Raycast(ray, out hit, 100))
    //      {
    //        if (hit.collider != null &&
    //            hit.collider.gameObject != null)
    //      {
    //         CheckCollision(hit.collider.gameObject);
    //    }

    // }
    // }
    //}

    /*  private void CheckCollision(GameObject collider_game)

      {
         if(collider_game != null && 
              collider_game.tag == _tag) {

              Destroy(collider_game.transform.parent.gameObject);
              Destroy(collider_game);

              string text = _Collectable_count_txt.text;
              int value = System.Int32.Parse(text);
              value++;
              _Collectable_count_txt.text = value.ToString();

          }
      }*/

    // This function generates a random position within a circle defined by a randomly selected radius,
    // with the circle's center determined by specified argument variables. 

    private Vector3 GetRandomSpawnPosition(Vector3 pos)
    {
        Vector3 randomPosition = (Vector3) UnityEngine.Random.insideUnitCircle;
        return pos + randomPosition.normalized * spawnRadiusMin + randomPosition * (spawnRadiusMax - spawnRadiusMin);
    }


    private void spawnObject(Vector3 pos_arg)
    {
        var pos = GetRandomSpawnPosition(pos_arg);
         pos.y = -0.1f;

        if(IsPositionValid(pos))
        {
            InitiateObject(pos);
        }
    }

    void InitiateObject(Vector3 position)
    {
        Instantiate(_ObjectPrefab, position, Quaternion.identity);
    }

    //The function verifies if the position is valid by checking 
    // whether an object occupies it.
    // If an object is present at that position, it is considered invalid. 

    bool IsPositionValid(Vector3 pos)
    {
        Collider[] colliders = Physics.OverlapBox(pos, boxSize / 2);
        return colliders.Length == 0; 
    }
   

}

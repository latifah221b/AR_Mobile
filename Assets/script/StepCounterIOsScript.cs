using System.Runtime.InteropServices;
using UnityEngine;
using TMPro;

public class StepCounterIOSScript : MonoBehaviour
{
    public GameObject cube;
    public Vector3 rotate;
    public GUIStyle msgStyle;

    [SerializeField] private TMP_Text step_counter_txt;

    //private Rect _rect
    private string _msg;

    [SerializeField] private GameObject[] papers;
    private int currentStepCount = 0;
    private int nextPaperStep = 8;
    private int paperIndex = 0;
    private const float maxSpawnDistance = 2.0f;
    private int maxPapers = 6;


#if UNITY_IOS && !UNITY_EDITOR
   [DllImport("__Internal")]
    private static extern void UnityOnStart();
#endif


    
    void Start()
    {
        //_rect = new Rect(0, 0, Screen.width, Screen.height);
        _msg = "0";

#if UNITY_IOS && !UNITY_EDITOR
        UnityOnStart();
#endif
    }

    private void OnMessageReceived(string msg)
    {
        _msg = msg;


        int steps = int.Parse(_msg);
        step_counter_txt.text = steps.ToString();
        HandleStepCount(steps);
    }


    private void HandleStepCount(int steps)
    {
        currentStepCount = steps;


        if (paperIndex >= maxPapers) return;


        if (currentStepCount >= nextPaperStep && paperIndex < papers.Length)
        {
            SpawnPaper();
            nextPaperStep += 5;
        }
    }


    private void SpawnPaper()
    {
        if (paperIndex >= maxPapers) return;


        Vector3 playerPosition = Camera.main.transform.position;


        Vector3 randomDirection = Random.insideUnitSphere * maxSpawnDistance;
        randomDirection.y = 0;
        Vector3 spawnPosition = playerPosition + randomDirection;


        Instantiate(papers[paperIndex], spawnPosition, Quaternion.identity);

        Debug.Log("Spawned: " + papers[paperIndex].name + " at position: " + spawnPosition);

        paperIndex++;
    }

    void Update()
    {
        //cube.transform.Rotate(rotate * Time.deltaTime);
        step_counter_txt.text = _msg;
    }
}

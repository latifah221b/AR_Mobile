using System.Runtime.InteropServices;
using UnityEngine;
using TMPro;

public class StepCounterIOSScript : MonoBehaviour
{
    public GameObject cube;
    public Vector3 rotate;
    public GUIStyle msgStyle;

    [SerializeField] private TMP_Text step_counter_txt;

    //private Rect _rect;
    private string _msg;

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
    }

    

     void Update()
    {
        //cube.transform.Rotate(rotate * Time.deltaTime);
        step_counter_txt.text = _msg;
    }
}

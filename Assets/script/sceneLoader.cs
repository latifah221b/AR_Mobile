using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Timers;
using Unity.VisualScripting;
using UnityEngine.Events;
using System;
using System.Threading;
using UnityEditor.Rendering;
using System.Runtime.CompilerServices;
using UnityEngine.Rendering.Universal.Internal;



public struct question{
    private string question_text;
    public string get_question_text() { return question_text; }

    private List<String> options;
    public List<String> get_options() { return options; }

    private string correct_op;
    public string get_correct_op() { return correct_op; }

    public question(string question_txt, List<String> opts, 
        string correct)
    {
        question_text = question_txt;
        options = opts;
        correct_op = correct;
    }
    
}

public class sceneLoader : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject _startingSceneTransition;
    [SerializeField] private GameObject _endingSceneTransition;
    [SerializeField] private GameObject _blurEffect;
    [SerializeField] private GameObject _dialog;
    [SerializeField] private GameObject _rocket_scene;
    [SerializeField] private GameObject _Enemy;
    [SerializeField] private int _num_of_enemy;
    [SerializeField] private GameObject _eventSystem; 
    private question[] _question_list;
    private List<question> Questions = new List<question>();






    private float distance = 2f;
    private float _rocket_offset = 2.0f;
    Transform CameraTransfrom = null;

    public int pick_a_random_index(int max) {
         System.Random rnd = new System.Random();
         int r = rnd.Next(max);
        return r;

    }
    void fill_questions() {

        question Q1 = new question("How many satellites has KACST launched from 2000 to 2021 ",
            new List<string> { "10", "13", "15", "17" }, "17");

        question Q2 = new question("What are Shaheen Sat missions?",
           new List<string>{ 
               "Earth imagery and vessel tracking",
               "Telecommunications", 
               "Anticipating flood risks", 
               "Conducting physics experiment using uv lights" }, "Earth imagery and vessel tracking");

        question Q3 = new question(
            "Under which sector in KACST is the Space Technologies institute?",
           new List<string> { "Health & wellness", 
               "Sustainable economies", 
               "Future economies", 
               "Energy and industry" }, "Future economies");

        question Q4 = new question("What is the mission of Saudi Sat 5 A & B satellites that were launched in 2018?",
           new List<string> { "High-resolution satellite images", 
               "Tracking ships using AIS systems+", 
               "Moon exploration", "High speed telecommunication" }, "High-resolution satellite images");

        foreach(question q in new question[4]{ Q1, Q2, Q3 , Q4})
            Questions.Add(q);




    }

    public question pick_a_question_for_an_enemy() {
        int indx = pick_a_random_index(Questions.Count);
        var q = Questions[indx];
        Questions.RemoveAt(indx);
        return q;
    }

    void Start() {
        fill_questions();
        if (_startingSceneTransition) { _startingSceneTransition.SetActive(true); 
            StartCoroutine(transition()); }

        if(_eventSystem != null) { DoNotDestoryOnload(_eventSystem); }
       
        


    }
    private void Update()
    {
    }

    
    IEnumerator transition()
    {
        yield return new WaitForSeconds(3f);
        _rocket_scene.SetActive(true);
    }
    

    private Vector2 Random_positioning(Vector2 original_point, 
        float radius)
    {

        return original_point + UnityEngine.Random.insideUnitCircle * radius;
    }

    private void DeactiviteUI() {
    }


   
    private void PlacePrefab()
    {
        

        Vector3 spawnPosition = CameraTransfrom.position +
            CameraTransfrom.forward * 50;

        _rocket_scene.transform.position = spawnPosition;
        _rocket_scene.SetActive(true);

    }
    



    public void LoadA(string scenename)
        {

        //SceneManager.LoadScene(scenename);
        StartCoroutine(LoadYourAsyncScene(scenename));
        }


    // Async load
    IEnumerator LoadYourAsyncScene(string scenename)
    {

        var asyncload = SceneManager.LoadSceneAsync(scenename);
        while(! asyncload.isDone)
        {
            yield return null;
        }
    }

    public void DoNotDestoryOnload(GameObject obj)
    {
        DontDestroyOnLoad(obj);
    }

}



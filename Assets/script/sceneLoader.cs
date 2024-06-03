using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Timers;
using Unity.VisualScripting;
using UnityEngine.Events;
using System;

public class sceneLoader : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private  GameObject _startingSceneTransition;
    [SerializeField] private  GameObject _endingSceneTransition;
    private System.Timers.Timer timer = new System.Timers.Timer(5000);
    private static UnityEvent myEvent = new UnityEvent();

    void Start()
        {
        _startingSceneTransition.SetActive(true);
        timer.Elapsed += OnTimerElapsed;
        timer.Enabled  = true;
        timer.AutoReset = true;
        myEvent.AddListener(DisableStartingSceneTransition);


    }
    private void DisableStartingSceneTransition()
    {
        
        _startingSceneTransition.SetActive(false);
        myEvent.RemoveListener(DisableStartingSceneTransition);
        myEvent.AddListener(EnableEndingSceneTransition);

    }

    private void EnableEndingSceneTransition()
    {
        _endingSceneTransition.SetActive(true);
       

    }

    private static void OnTimerElapsed(object sender, ElapsedEventArgs e)
    {
       
        myEvent.Invoke();

    }

   

    public void LoadA(string scenename)
        {

            SceneManager.LoadScene(scenename);
        }
    
}

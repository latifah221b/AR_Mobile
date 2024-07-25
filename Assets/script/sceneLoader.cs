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



public class sceneLoader : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]  private  GameObject _startingSceneTransition;
    [SerializeField]  private  GameObject _endingSceneTransition;
    [SerializeField] private GameObject _blurEffect;
    [SerializeField] private GameObject  _dialog;
    [SerializeField] private GameObject _rocket_scene;
    [SerializeField] private GameObject _Enemy;

    

    private float distance = 2f;
    private float _rocket_offset = 2.0f;
    Transform CameraTransfrom = null;


    void Start() {
        _startingSceneTransition.SetActive(true);
        StartCoroutine(transition());


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

            SceneManager.LoadScene(scenename);
        }
    
}



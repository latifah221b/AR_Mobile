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


    void Start() {
       if(_startingSceneTransition != null)
        {
            _startingSceneTransition.SetActive(true);
            Invoke(nameof(transition), 3f);
        }
        
       

    }
    private void transition()
    {
        _startingSceneTransition.SetActive(false);
        _endingSceneTransition.SetActive(true);
        Invoke(nameof(DeactiviteUI), 1f);
 
    }

    private Vector2 Random_positioning(Vector2 original_point, 
        float radius)
    {

        return original_point + UnityEngine.Random.insideUnitCircle * radius;
    }

    private void DeactiviteUI() {
        _endingSceneTransition.SetActive(false);
        _blurEffect.SetActive(false);
        _dialog.SetActive(false);
        //_rocket_scene.SetActive(true);
        PlacePrefab();

    }


   
    private void PlacePrefab()
    {
        Transform CameraTransfrom = Camera.main.transform;

        Vector3 spawnPosition = CameraTransfrom.position +
            CameraTransfrom.forward * 20;

        GameObject instantiatePrefab = Instantiate(_rocket_scene, 
            spawnPosition, Quaternion.identity);

        Destroy(_rocket_scene);
        instantiatePrefab.SetActive(true);

    }
    



    public void LoadA(string scenename)
        {

            SceneManager.LoadScene(scenename);
        }
    
}



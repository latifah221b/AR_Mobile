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
    [SerializeField] private GameObject _dialog;
    [SerializeField] private GameObject _rocket;
    [SerializeField] private GameObject _Enemy;

    private float distance = 2f;
    private float _rocket_offset = 2.0f;


    void Start() {
        _startingSceneTransition.SetActive(true);


        Invoke(nameof(transition), 3f);

    }
    private void transition()
    {
        _startingSceneTransition.SetActive(false);
        _endingSceneTransition.SetActive(true);
        Invoke(nameof(DeactiviteUI), 1f);
 
    }

    private void DeactiviteUI() {
        _endingSceneTransition.SetActive(false);
        _blurEffect.SetActive(false);
        _dialog.SetActive(false);
      
        _rocket.transform.position = Camera.main.transform.position + Camera.main.transform.forward * distance;
        

        this._rocket.transform.position = new Vector3(this._rocket.transform.position.x, 
            this._rocket.transform.position.y+ _rocket_offset, 
            this._rocket.transform.position.z);
        _rocket.SetActive(true);

        var startPos = this._rocket.transform.position;
        var endPos = Camera.main.transform.position;

        CoroutineUtils.Lerp(this, 1f, t => {
            var EndPos = endPos;

            float l = Mathf.Lerp(startPos.y,
                endPos.y, t);
            
            this._rocket.transform.position = new Vector3(this._rocket.transform.position.x, l, 
            this._rocket.transform.position.z);

            if(t == 1f)
                this._Enemy.transform.position = EndPos;
            

        });
       


    }



    public void LoadA(string scenename)
        {

            SceneManager.LoadScene(scenename);
        }
    
}



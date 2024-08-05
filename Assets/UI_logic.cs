using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_logic : MonoBehaviour
{
    [SerializeField] private GameObject _blurEffect;
    [SerializeField] private GameObject [] _dialog;
    [SerializeField] private sceneLoader Sceneloader;
    [SerializeField] private GameObject _startSceneTransition;
    [SerializeField] private GameObject _endSceneTransition;
    [SerializeField] private GameObject[] enemyinputs;
    [SerializeField] private Animator _animator; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void activiteUI()
    {
        StartCoroutine(activiateDialogs());
      
      
    }

    IEnumerator activiateDialogs()
    {
        yield return new WaitForSecondsRealtime(2);
        _startSceneTransition.SetActive(false);
        _blurEffect.SetActive(true);
        _dialog[0].SetActive(true);
        yield return new WaitForSecondsRealtime(6);
    }
    public void activiate_dialog_0()
    {
        _dialog[0].SetActive(false);
        _dialog[1].SetActive(true);

    }

    public void activiate_dialog_00()
    {
        _dialog[1].SetActive(false);
        _dialog[2].SetActive(true);

    }

    public void activiate_dialog_1()
    {
        _dialog[2].SetActive(false);
        _dialog[3].SetActive(true);
    }
    public void activiate_dialog_2()
    {
        _dialog[3].SetActive(false);
        _dialog[4].SetActive(true);
    }
    public void activiate_dialog_3()
    {
        _blurEffect.SetActive(false);
        _dialog[4].SetActive(false);
        _dialog[5].SetActive(true);
        foreach (var enemy in enemyinputs)
        {
            enemy.GetComponent<InputManger>().enabled = true;
        }

        if(_animator != null)
        {
            _animator.enabled = false;
        }
    }

  


}

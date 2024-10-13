using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class scene3_UI_Interaction : MonoBehaviour
{
    [SerializeField] private GameObject _greetingCanvas, _tappingCanvas, 
        _scanningCanvas, _tapResponder;

    [SerializeField] private ARObjectSpawner _arObjectSpawner;


    public void On_click_continue_button_Greeting()
    {
        if (_greetingCanvas != null)
        {
            _greetingCanvas.SetActive(false);
            StartCoroutine(CoroutineUI());
        }
    }

    public void On_click_continue_button_Tapping()
    {
        if (_tappingCanvas != null){
            _tappingCanvas.SetActive(false); 
            _tapResponder.SetActive(true);
        }
    }

    public void On_click_continue_button_Scanning()
    {
        if (_scanningCanvas != null)
        {
            _scanningCanvas.SetActive(false);

            if (_arObjectSpawner != null)
            {
                _arObjectSpawner.enabled = true;
            }
        }
    }

    IEnumerator CoroutineUI()
    {
        yield return new WaitForSeconds(2);
        _scanningCanvas.SetActive(true);
    }
}
 



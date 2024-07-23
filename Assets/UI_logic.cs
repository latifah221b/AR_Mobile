using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_logic : MonoBehaviour
{
    [SerializeField] private GameObject _blurEffect;
    [SerializeField] private GameObject [] _dialog;
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

        _blurEffect.SetActive(true);
        _dialog[0].SetActive(true);


    }
}

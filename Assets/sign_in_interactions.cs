using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sign_in_interactions : MonoBehaviour
{
    [SerializeField] private GameObject _Sector_dialog;
    [SerializeField] private GameObject _login_dialog;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void on_sign_in_click()
    {
        if(_login_dialog != null && _Sector_dialog != null)
        {
            _login_dialog.SetActive(false);
            _Sector_dialog.SetActive(true);
        }

      
    }
}

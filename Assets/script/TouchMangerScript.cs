using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;


public class TouchMangerScript : MonoBehaviour
{
    public XRScreenSpaceController controller;
    InputAction m_Action;
    public UnityEvent<Vector2> m_event = new UnityEvent<Vector2>();

    private void Start()
    {
        //m_Input = GetComponent<PlayerInput>();
        m_Action = controller.tapStartPositionAction.action;
        //m_Action = m_Input.actions.FindAction("Tap Start Position");


    }


    private void Awake()
    {

        m_Action = controller.tapStartPositionAction.action;

    }

    private void OnEnable()
    {
        if (m_Action != null)
        {
            m_Action.started += TapPressed;
        }
       
    }

    private void OnDisable()
    {
        if (m_Action != null)
        {
            m_Action.started -= TapPressed;
        }
    }


     public void TapPressed(InputAction.CallbackContext c)
    {
        Vector2 tappos = c.ReadValue<Vector2>();
         m_event.Invoke(tappos);
        //Debug.Log(tappos);

    }
}

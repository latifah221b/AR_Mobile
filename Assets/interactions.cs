using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interactions : MonoBehaviour
{
    [SerializeField] private GameObject[] dialogues;
    [SerializeField] private int last_index;
    public void on_sign_in_click(int pre)
    {
        if (last_index == pre)
        {
            if (dialogues[pre] != null)
            {
                dialogues[pre].SetActive(false);
            }
            return; 
        }

        if (dialogues[pre] != null && dialogues[pre+1] != null)
        {
            dialogues[pre].SetActive(false);
            dialogues[pre + 1].SetActive(true);
        }

        if (last_index == pre)
        {
            if (dialogues[pre] != null){
                dialogues[pre].SetActive(false);
            }
        }

      
    }
}

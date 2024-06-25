using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;




public class collectable_script : MonoBehaviour
{
   
   public TouchMangerScript mangerScript;
   public TextMeshProUGUI score_text;
   public score_struct score_struct;
   public sceneLoader sceneLoader;



    // Start is called before the first frame update
    void Start()
    {
        mangerScript.m_event.AddListener(handleTap);
    }


    // Update is called once per frame


    private void handleTap(Vector2 screenpos)
    {
        RaycastHit hit;
        if (screenpos != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(screenpos);
            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.collider != null && 
                    hit.collider.gameObject != null 
                    && hit.collider.gameObject.tag != null &&
                    hit.collider.gameObject.tag == "star")
                {
                    score_struct.score++;
                    score_text.text = "3 / " + score_struct.score.ToString();

                    Destroy(hit.collider.gameObject);
                    Destroy(this.gameObject);

                    if (score_struct.score == 3)
                    {
                        sceneLoader.LoadA("scene4");
                    }


                }

            }
        }
    }
    
}

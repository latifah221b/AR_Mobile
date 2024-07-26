using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;


public class fetch_question_for_enemy : MonoBehaviour
   

{
    [SerializeField] private TMP_Text _question;
    [SerializeField] private List<TMP_Text>  _button_text;
    [SerializeField] private sceneLoader sceneloader;
    [SerializeField] private GameObject incorrect_dailog;
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject _attached_part;
    [SerializeField] private GameObject _visual;

    private string correct_answer ; 
  

    // Start is called before the first frame update
    void Start()
    {
        if(sceneloader != null)
        {
          
          var q =  sceneloader.pick_a_question_for_an_enemy();
            _question.text = q.get_question_text();
            var opt = q.get_options();

            while (_button_text.Count != 0)
            {
                int indx = sceneloader.pick_a_random_index(_button_text.Count);
                var button = _button_text[indx];
                _button_text.RemoveAt(indx);
                button.text = opt[indx];
                opt.RemoveAt(indx);
            }
            correct_answer = q.get_correct_op();
            canvas.SetActive(true);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // event listeners 
    public void OnClick_button(TMP_Text _button_text)
    {
        if(correct_answer != null && _button_text != null)
        {
             if(_button_text.text != correct_answer)
            {
                incorrect_dailog.SetActive(true);
                //To do: change the trans & hide the game object for a while 
            }
            else
            {

                canvas.SetActive(false);
                _visual.SetActive(false);
                _attached_part.SetActive(true);

                //Destory the enemy once the part is collect to clean up the scene. 



                //To do: change the button  to correct & show the object to collect "Inventory System"
            }
        }

    }
   
}

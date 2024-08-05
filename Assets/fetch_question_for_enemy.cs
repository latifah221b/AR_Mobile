using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityFigmaBridge.Runtime.UI;
using UnityEngine.UI;


public class fetch_question_for_enemy : MonoBehaviour


{
    [SerializeField] private TMP_Text _question;
    [SerializeField] private List<TMP_Text> _button_text;
    [SerializeField] private List<Button> _buttons;
    [SerializeField] private sceneLoader sceneloader;
    [SerializeField] private GameObject incorrect_dailog;
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject _attached_part;
    [SerializeField] private GameObject _visual;
    [SerializeField] private GameObject _particles;
    [SerializeField] private Transform _transform;


    private string correct_answer;
    private float spawnRadiusMin = 1f;
    private float spawnRadiusMax = 5f;


    // Start is called before the first frame update
    void Start()
    {
        // listen to the tap on the enemy 

        fill_the_canvas();

        // event listener ..
        foreach (var button in _buttons)
        {
            button.onClick.AddListener(() =>
            {
                StartCoroutine(button_logic(button));
            });
        }

        

    }

    // Update is called once per frame
    void Update()
    {

    }
    void fill_the_canvas()
    {
        if (sceneloader != null)
        {
            var q = sceneloader.pick_a_question_for_an_enemy();
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
           
        }


    }
    IEnumerator button_logic(Button butt)
    {
        var button_text = butt.GetComponentInChildren<TMP_Text>();

        if (button_text != null && correct_answer != null)
        {
            if (button_text.text == correct_answer)
            {
                ColorBlock colorBlock = butt.colors;
                colorBlock.normalColor = Color.green;
                colorBlock.pressedColor = Color.green;
                colorBlock.selectedColor = Color.green;

                butt.colors = colorBlock;

                yield return new WaitForSecondsRealtime(2);
                canvas.SetActive(false);
                _visual.SetActive(false);
                _attached_part.SetActive(true);

                //To do: show the object that was collected to "Inventory System"
                //Destory the enemy once the part is collect to clean up the scene. 
            }

            else
            {
                incorrect_dailog.SetActive(true);
                
                yield return new WaitForSecondsRealtime(2);
                
                canvas.SetActive(false);
                _particles.SetActive(true);
                _visual.SetActive(false);
                yield return new WaitForSecondsRealtime(0.8f);
                _particles.SetActive(false);
                //To do: change the trans & hide the game object for a while 

                yield return new WaitForSecondsRealtime(3f);

                Debug.Log("old pos" + _transform.position);
                var new_pos = GetRandomSpawnPosition(_transform.position);
                Debug.Log("new pos" + new_pos);
                _transform.position = new_pos;

                incorrect_dailog.SetActive(false);
                _visual.SetActive(true);

            }

        }

    }

    private Vector3 GetRandomSpawnPosition(Vector3 pos)
    {
        Vector3 randomPosition = (Vector3) Random.insideUnitCircle;
        return pos + randomPosition.normalized * spawnRadiusMin + randomPosition * (spawnRadiusMax - spawnRadiusMin);
    }
}







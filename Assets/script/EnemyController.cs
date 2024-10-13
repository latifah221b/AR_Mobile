using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityFigmaBridge.Runtime.UI;
using UnityEngine.UI;


public class EnemyController : MonoBehaviour


{
    [SerializeField] private TMP_Text _question;
    [SerializeField] private List<TMP_Text> _buttonText;
    [SerializeField] private List<Button> _buttons;
  

    [SerializeField] private sceneLoader sceneloader;
    [SerializeField] private GameObject _incorrectDailog, 
        _canvas, _attachedPart, _visual, _particles;

    [SerializeField] private Transform _transform;
    private float spawnRadiusMin = 1f;
    private float spawnRadiusMax = 5f;
    private string correctAnswer;

    // Start is called before the first frame update
    void Start()
    {
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

    public void set_Scene_Loader(sceneLoader scene) {
        sceneloader = scene;
}
    public void set_transform(Transform _tra)
    {
        _transform = _tra;
    }

    void fill_the_canvas()
    {
        if (sceneloader != null)
        {
            var q = sceneloader.pick_a_question_for_an_enemy();
            _question.text = q.get_question_text();
            var opt = q.get_options();
            while (_buttonText.Count != 0)
            {
                int indx = sceneloader.pick_a_random_index(_buttonText.Count);
                var button = _buttonText[indx];
                _buttonText.RemoveAt(indx);
                button.text = opt[indx];
                opt.RemoveAt(indx);
            }
            correctAnswer = q.get_correct_op();
        }


    }
    IEnumerator button_logic(Button butt)
    {
        var button_text = butt.GetComponentInChildren<TMP_Text>();

        if (button_text != null && correctAnswer != null)
        {
            if (button_text.text == correctAnswer)
            {
                ColorBlock colorBlock = butt.colors;
                colorBlock.normalColor = Color.green;
                colorBlock.pressedColor = Color.green;
                colorBlock.selectedColor = Color.green;

                butt.colors = colorBlock;

                yield return new WaitForSecondsRealtime(2);
                _canvas.SetActive(false);
                _visual.SetActive(false);
                _attachedPart.SetActive(true);
            }

            else
            {
                _incorrectDailog.SetActive(true);
                
                yield return new WaitForSecondsRealtime(2);

                _canvas.SetActive(false);
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

                _incorrectDailog.SetActive(false);
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







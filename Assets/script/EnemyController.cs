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

    //private Transform _transform;
    private float spawnRadiusMin = 1f;
    private float spawnRadiusMax = 5f;
    [SerializeField] private float maxDistance; // Maximum distance to translate
    [SerializeField] private float minDistance; 


    private string correctAnswer;
    private AudioManager audioManager;
    //public Item rocketPart;

    // Start is called before the first frame update
    void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
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
   // public void set_transform(Transform _tra)
   // {
      // transform = _tra;
   // }

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
    // Answer feedback colours, from the design system.
    private static readonly Color CorrectGreen = new Color(0.2314f, 0.7294f, 0.3569f, 1f); // #3BBA5B
    private static readonly Color WrongRed = new Color(0.6980f, 0.2392f, 0.2471f, 1f);     // #B23D3F

    // The answer buttons are FigmaImages and the Button tint MULTIPLIES the fill,
    // so tinting alone over the dark #3A3D43 background would give a murky colour.
    // Set the fill directly and hold the tints at white. Returns the previous fill
    // so it can be put back (alpha 0 means there was nothing to restore).
    private Color SetAnswerFill(Button butt, Color colour)
    {
        if (butt == null) return Color.clear;

        ColorBlock cb = butt.colors;
        cb.normalColor = Color.white;
        cb.highlightedColor = Color.white;
        cb.pressedColor = Color.white;
        cb.selectedColor = Color.white;
        butt.colors = cb;

        FigmaImage fill = butt.targetGraphic as FigmaImage;
        if (fill != null)
        {
            Color previous = fill.FillColor;
            fill.FillColor = colour;
            return previous;
        }
        if (butt.targetGraphic != null)
        {
            Color previous = butt.targetGraphic.color;
            butt.targetGraphic.color = colour;
            return previous;
        }
        return Color.clear;
    }

    private void RestoreAnswerFill(Button butt, Color colour)
    {
        if (butt == null || colour.a <= 0f) return;
        FigmaImage fill = butt.targetGraphic as FigmaImage;
        if (fill != null) { fill.FillColor = colour; return; }
        if (butt.targetGraphic != null) butt.targetGraphic.color = colour;
    }

    IEnumerator button_logic(Button butt)
    {
        var button_text = butt.GetComponentInChildren<TMP_Text>();

        if (button_text != null && correctAnswer != null)
        {
            if (button_text.text == correctAnswer)
            {
                audioManager.PlaySFX(audioManager.correct);
                SetAnswerFill(butt, CorrectGreen);

                yield return new WaitForSecondsRealtime(2);
                _canvas.SetActive(false);
                _visual.SetActive(false);
                _attachedPart.SetActive(true);
                //if (rocketPart != null && rocketPart.isRocketPart)
                //{
                    //FindObjectOfType<StarRewardSystem>().CollectRocketPart();
                //}
            }

            else
            {
                audioManager.PlaySFX(audioManager.wrong);
                Color wrongPrevious = SetAnswerFill(butt, WrongRed);
                _incorrectDailog.SetActive(true);
                
                yield return new WaitForSecondsRealtime(2);

                _canvas.SetActive(false);
                _particles.SetActive(true);
                _visual.SetActive(false);
                yield return new WaitForSecondsRealtime(0.8f);
                _particles.SetActive(false);

                yield return new WaitForSecondsRealtime(3f);

                
                //  var new_pos = GetRandomSpawnPosition(transform.position);
                //transform.position = new Vector3(new_pos.x, transform.position.y, new_pos.y);

                
                //Debug.Log("old pos" + transform.position);
                    TranslateRandomly();
               // Debug.Log("new pos" + transform.position);

                _incorrectDailog.SetActive(false);
                RestoreAnswerFill(butt, wrongPrevious);
                _visual.SetActive(true);

            }

        }

    }

   // private Vector2 GetRandomSpawnPosition(Vector2 pos)
    //{
      //  Vector2 randomPosition = (Vector2)UnityEngine.Random.insideUnitCircle;
       // return pos + randomPosition.normalized * spawnRadiusMin + randomPosition * (spawnRadiusMax - spawnRadiusMin);
   // }

    private void TranslateRandomly()
    {
        // Generate a random distance and direction along X or Z
        float randomDistance = Random.Range(minDistance, maxDistance);

        bool translateX = Random.value > 0.5f; // Randomly choose X or z
        Vector3 translation;
        if (translateX)
        {
            randomDistance = Random.value > 0.5f ? randomDistance : -(randomDistance);  
            translation = new Vector3(  randomDistance, 0, 0); // Translate along X
        }
        else
        {
            translation = new Vector3(0, 0, randomDistance); // Translate along Z
        }

        // Translate the object
        transform.position += translation;
    }

}







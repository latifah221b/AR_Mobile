using UnityEngine;
using TMPro;
using System;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class TapResponder : MonoBehaviour, INotifyOnTap
{
    //variables resident on the scenes 

    [SerializeField] private TextMeshProUGUI _Main_Quest_txt, _Collectable_count_txt;
    [SerializeField] private GameObject[] _final_dialogs;
    [SerializeField] private int _theTargetScore;
    [SerializeField] private sceneLoader _sceneLoaderRef;
    [SerializeField] private Animator _flyanimation;
    [SerializeField] private Renderer objectRenderer;
    [SerializeField] private GameObject resultsPanel;
    [SerializeField] private Image resultStarImage;
    [SerializeField] private TextMeshProUGUI resultStepCountText;
    [SerializeField] private TextMeshProUGUI resultItemCountText;

    [SerializeField] private TextMeshProUGUI resultTimeTakenText;
    private float startTime;
    private bool isTiming = false;

    private AudioManager audioManager;

    public void set_Main_Quest_txt(TextMeshProUGUI txt_ref)
    {
        _Main_Quest_txt = txt_ref;
    }

    private void Start()
    {
        GameObjectManager.Instance.RegisterNotifier(this);
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

        startTime = Time.time;
        isTiming = true;
    }




    public void OnTap(Vector2 tapPosition)
    {
        Collider hitCollider = CheckTapPosition(tapPosition);

        if (hitCollider != null)
        {
           // Debug.Log($"Hit: {hitCollider.gameObject.name}");

            HandleTag(hitCollider);
        }
        else
        {
           // Debug.Log("No collider was hit.");
        }

    }

    private void HandleTag(Collider collider)
    {
        GameObject parent; 
        // Check the tag of the collider and handle accordingly
        switch (collider.tag)
        {
            case "Enemy":
                // Handle enemy objects
                //Debug.Log("Hit an enemy!");
                 parent = GameObjectManager.Instance.GetParentGameObject(collider.gameObject);
               var canvas = GameObjectManager.Instance.FindChildCanvas(parent);
                canvas.gameObject.SetActive(true);
                break;

            case "rocket_part":

                audioManager.PlaySFX(audioManager.partsitems);

                //Debug.Log("Interacting with rocket Part");
                parent = GameObjectManager.Instance.GetParentGameObject(collider.gameObject);
                //Debug.Log($"Parent: {parent.name}");

                // if we are using a box 
                parent = GameObjectManager.Instance.GetParentGameObject(parent);
                //Debug.Log($"Parent: {parent.name}");

                // update the inventory system  
                Item item = GameObjectManager.Instance.GetItemScriptableObject(parent);
                if (item != null && InventoryManager.Instance != null)
               {
                   InventoryManager.Instance.Add(item);
                   FindObjectOfType<StarRewardSystem>().CollectRocketPart();
                    //Debug.Log(item.itemName + " added to inventory");
                }

                // rocket part prefab 
                collider.gameObject.SetActive(false);

                // update the UI to reflect collecting a coin
                if (_Main_Quest_txt != null)
                {
                    int value_int = Int32.Parse(_Main_Quest_txt.text);
                    value_int++;
                    _Main_Quest_txt.text = value_int.ToString();
                }

                // Destroy the enemy prefab no longer is needed
                    Destroy(GameObjectManager.Instance.GetParentGameObject(parent));

                // check if we reached our goal
                if (Int32.Parse(_Main_Quest_txt.text) >= _theTargetScore)
                {
       
                    string currentSceneName = SceneManager.GetActiveScene().name;
                        switch (currentSceneName)
                        {
                            case "scene3":
                                //Debug.Log("Action for Scene 3");
                            StartCoroutine(finalLogicScene3());
                            break;

                            case "scene6":
                            //Debug.Log("Action for Scene6");
                            StartCoroutine(finalLogicScene6());
                            break;
                            case "scene7":
                            //Debug.Log("Action for Scene6");
                            //StartCoroutine(finalLogicScene6());
                            break;

                        default:
                                //Debug.Log("Default action for other scenes");
                                // Default code
                                break;
                        }
                }


                break;


            case "star_box":
                // Handle collectible objects
                //Debug.Log("Collected an item!");

                audioManager.PlaySFX(audioManager.coin);

                Destroy(collider.transform.parent.gameObject);
                Destroy(collider);

                string text = _Collectable_count_txt.text;
                int value = System.Int32.Parse(text);
                value++;
                _Collectable_count_txt.text = value.ToString();
                break;
            case "rocket":
                Debug.Log("I have hit the rocket");
                // check if we reached our goal
                // Attempt to parse _Main_Quest_txt.text safely
                if (Int32.Parse(_Main_Quest_txt.text) >= _theTargetScore)
                {

                    StartCoroutine(runfinalanimation());
                }
                break;

            default:
                //Debug.Log("Hit an object with no specific tag.");
                break;
           
        }
    }

    private Collider CheckTapPosition(Vector2? tapPosition)
    {
        // Check if tapPosition is null
        if (tapPosition == null)
        {
            return null; // Exit if no tap position is provided
        }

        RaycastHit hit;
        Vector2 screenPos = (Vector2)tapPosition; 

        // Cast a ray from the camera to the tap position
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        if (Physics.Raycast(ray, out hit, 100))
        {
            if (hit.collider != null)
            {
                // Return the collider that was hit
                return hit.collider;
            }
        }

        // Return null if no collider was hit
        return null;
    }

    private void OnDestroy()
    {
        // Unregister this instance from the GameObjectManager
        //Debug.Log("TapResponder is Destroyed");
        GameObjectManager.Instance.UnregisterNotifier(this);
    }

    IEnumerator finalLogicScene3()
    {
        {
            yield return new WaitForSecondsRealtime(2);
            _sceneLoaderRef.LoadA("scene4");
            Destroy(this.gameObject);

        }


    }

    IEnumerator finalLogicScene6()
    {
        if (_final_dialogs.Length > 0)
        {
            yield return new WaitForSecondsRealtime(6);
            _final_dialogs[2].SetActive(true);
            yield return new WaitForSecondsRealtime(3);
            _final_dialogs[2].SetActive(false);

           // while (!IsFullyVisible())
           // {
              //  yield return new WaitForSecondsRealtime(1);
            //}

           // _final_dialogs[3].SetActive(true);
           // yield return new WaitForSecondsRealtime(3);
           // _final_dialogs[3].SetActive(false);


            // Wait until the distance between pointA and pointB is less than or equal to targetDistance
           // while (Vector3.Distance(Camera.main.transform.position, objectRenderer.transform.position) > 4f)
            //{
               
             //   yield return null; // Wait for the next frame
               
                
           // }
           // yield return new WaitForSecondsRealtime(2);


           // if (_flyanimation != null)
           // {
               // audioManager.PlaySFX(audioManager.clear);
               // _flyanimation.enabled = true;
                // Wait for the animation duration
              //  yield return new WaitForSeconds(_flyanimation.GetCurrentAnimatorStateInfo(0).length);
           // }

            //yield return new WaitForSecondsRealtime(3);

           // _final_dialogs[0].SetActive(true);
           // yield return new WaitForSecondsRealtime(3);
          //  _final_dialogs[0].SetActive(false);

          //  yield return new WaitForSecondsRealtime(1);

           // _final_dialogs[1].SetActive(true);
          //  yield return new WaitForSecondsRealtime(3);
          //  _final_dialogs[1].SetActive(false);
        }


    }

    IEnumerator runfinalanimation() {

        if (_flyanimation != null)
        {
            audioManager.PlaySFX(audioManager.clear);
            _flyanimation.enabled = true;
            // Wait for the animation duration
            yield return new WaitForSeconds(_flyanimation.GetCurrentAnimatorStateInfo(0).length);
        }

        yield return new WaitForSecondsRealtime(1);

        _final_dialogs[0].SetActive(true);
        yield return new WaitForSecondsRealtime(3);
        _final_dialogs[0].SetActive(false);

        yield return new WaitForSecondsRealtime(1);

        _final_dialogs[1].SetActive(true);
        yield return new WaitForSecondsRealtime(3);
        _final_dialogs[1].SetActive(false);

        _final_dialogs[3].SetActive(true);

        StarRewardSystem starSystem = FindObjectOfType<StarRewardSystem>();
        if (starSystem != null && resultStarImage != null)
        {
            resultStarImage.sprite = starSystem.GetCurrentRewardSprite();
        }

        StepCounterIOSScript stepCounter = FindObjectOfType<StepCounterIOSScript>();
        if (stepCounter != null && resultStepCountText != null)
        {
            resultStepCountText.text = stepCounter.GetCurrentStepCount().ToString();
        }

        if (starSystem != null && resultItemCountText != null)
        {
            resultItemCountText.text = starSystem.GetItemCount().ToString();
        }

        if (resultTimeTakenText != null)
        {
            float totalTime = Time.time - startTime;
            resultTimeTakenText.text = FormatTime(totalTime);
        }

    }

    private string FormatTime(float timeInSeconds)
    {
        int minutes = (int)(timeInSeconds / 60f);
        int seconds = (int)(timeInSeconds % 60f);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    /*
    private bool IsFullyVisible()
    {
        // Get the bounding box of the GameObject
        Bounds bounds = objectRenderer.bounds;
        Vector3[] corners = new Vector3[8];

        // Calculate corners of the bounding box
        corners[0] = bounds.min;
        corners[1] = bounds.max;
        corners[2] = new Vector3(bounds.min.x, bounds.min.y, bounds.max.z);
        corners[3] = new Vector3(bounds.min.x, bounds.max.y, bounds.min.z);
        corners[4] = new Vector3(bounds.max.x, bounds.min.y, bounds.min.z);
        corners[5] = new Vector3(bounds.min.x, bounds.max.y, bounds.max.z);
        corners[6] = new Vector3(bounds.max.x, bounds.min.y, bounds.max.z);
        corners[7] = new Vector3(bounds.max.x, bounds.max.y, bounds.min.z);

        // Check if all corners are visible to the camera
        foreach (Vector3 corner in corners)
        {
            Vector3 screenPoint = Camera.main.WorldToViewportPoint(corner);
            if (screenPoint.x < 0 || screenPoint.x > 1 || screenPoint.y < 0 || screenPoint.y > 1 || screenPoint.z < 0)
            {
                return false; // At least one corner is outside the view
            }
        }

        return true; // All corners are inside the camera's view
    }
    */
}

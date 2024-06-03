using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class button_logic : MonoBehaviour, IDeselectHandler
{
    // Start is called before the first frame update
    private TextMeshProUGUI text;
    private Button button;
    private Color TextColor = Color.white;
    void Start()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        button = GetComponent<Button>();
        button.onClick.AddListener(toggle_color);
        


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDeselect(BaseEventData eventData)
    {
        ((IDeselectHandler)button).OnDeselect(eventData);

        toggle_color();

    }

    public void toggle_color() {
   
        if(text != null && button != null) {
            if(text.color != TextColor){ 
                Color temp = text.color; 
                text.color = TextColor;
                TextColor = temp;}

           }
           
        
    }

  
}


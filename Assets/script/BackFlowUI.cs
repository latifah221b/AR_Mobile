using UnityEngine;

public class BackFlowUI : MonoBehaviour
{
    public GameObject backPanel;
    public GameObject confirmBackPanel;

    void Start()
    {
        backPanel.SetActive(false);
        confirmBackPanel.SetActive(false);
    }

    
    public void OpenBackPanel()
    {
        confirmBackPanel.SetActive(false);
        backPanel.SetActive(true);
    }

    
    public void OpenConfirmPanel()
    {
        backPanel.SetActive(false);
        confirmBackPanel.SetActive(true);
    }

    public void ConfirmNo()
    {
        confirmBackPanel.SetActive(false);
       
    }

    public void CancelBack()
    {
        backPanel.SetActive(false);
    }
}

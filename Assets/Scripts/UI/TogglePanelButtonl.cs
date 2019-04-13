using UnityEngine;

public class TogglePanelButton : MonoBehaviour
{
    public void TogglePanel(GameObject panel)
    {
        panel.SetActive(!panel.activeSelf);
    }
}
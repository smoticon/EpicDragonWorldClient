using UnityEngine;
using UnityEngine.UI;

/**
 * Author: Pantelis Andrianakis
 * Date: December 25th 2017
 */
public class ButtonQuitGame : MonoBehaviour
{
    public Button quitButton;

    void Start()
    {
        quitButton.GetComponent<Button>().onClick.AddListener(OnClickTask);
    }

    void OnClickTask()
    {
        Application.Quit();
    }
}

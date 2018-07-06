using UnityEngine;
using UnityEngine.UI;

/**
 * @author Pantelis Andrianakis
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

using TMPro;
using UnityEngine;
using UnityEngine.UI;

/**
 * Author: Pantelis Andrianakis
 * Date: December 22th 2018
 */
public class ConfirmDialog : MonoBehaviour
{
    public static ConfirmDialog Instance { get; private set; }

    public Canvas canvas;
    public Button acceptButton;
    public Button declineButton;
    public Button closeButton;
    public TextMeshProUGUI  messageText;
    [HideInInspector]
    public bool confirmDialogActive = false;
    private int confirmDialogId;

    private void Start()
    {
        Instance = this;

        // Click listeners.
        acceptButton.onClick.AddListener(AcceptConfirmDialog);
        declineButton.onClick.AddListener(CloseConfirmDialog);
        closeButton.onClick.AddListener(CloseConfirmDialog);
        // Close UI.
        CloseConfirmDialog();
    }

    private void Update()
    {
        if (InputManager.ESCAPE_DOWN && canvas.enabled)
        {
            CloseConfirmDialog();
        }
    }

    public void PlayerConfirm(string question, int dialogId)
    {
        // Return false when waiting other dialog confirm.
        if (confirmDialogActive)
        {
            return;
        }
        confirmDialogActive = true;
        messageText.text = question;
        confirmDialogId = dialogId;
        canvas.enabled = true;
    }

    private void AcceptConfirmDialog()
    {
        canvas.enabled = false;
        switch (confirmDialogId)
        {
            case 1:
                System.Diagnostics.Process.GetCurrentProcess().Kill();
                break;

            case 2:
                CharacterSelectionManager.Instance.DeleteCharacter();
                break;

            case 3:
                NetworkManager.ChannelSend(new ExitWorldRequest());
                WorldManager.Instance.ExitWorld();
                OptionsManager.Instance.optionsCanvas.enabled = false;
                MainManager.Instance.LoadScene(MainManager.CHARACTER_SELECTION_SCENE);
                break;
        }
        confirmDialogActive = false;
    }

    private void CloseConfirmDialog()
    {
        canvas.enabled = false;
        confirmDialogActive = false;
    }
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Collections;

/**
 * @author Pantelis Andrianakis
 */
public class ChatBoxManager : MonoBehaviour
{
    public static ChatBoxManager instance;
    public GameObject textMessageObject;
    public GameObject scrollViewContent;
    public GameObject scrollViewRightBar;
    public InputField inputField;
    public bool isFocused = false;
    private string lastTell = "";
    private int maxMessages = 50;
    [SerializeField]
    private List<Message> messageList = new List<Message>();

    private void Start()
    {
        instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (inputField.text != "")
            {
                if (NetworkManager.instance != null)
                {
                    NetworkManager.instance.ChannelSend(new ChatRequest(inputField.text));
                    string[] messageSplit = Regex.Replace(inputField.text, @"\s+", " ").Trim().Split(' ');
                    if (messageSplit.Length > 2 && messageSplit[0].ToLower().Equals("/tell"))
                    {
                        lastTell = messageSplit[1];
                    }
                    else
                    {
                        lastTell = "";
                    }
                }
                else
                {
                    SendMessageToChat(inputField.text, 1);
                }
                inputField.text = "";
            }
            else
            {
                inputField.Select();
                if (lastTell != "")
                {
                    inputField.text = "/tell " + lastTell + " ";
                    StartCoroutine(MoveTextEnd_NextFrame());
                }
            }
        }

        if (inputField.isFocused)
        {
            isFocused = true;
            scrollViewRightBar.SetActive(true);
        }
        else
        {
            isFocused = false;
            scrollViewRightBar.SetActive(false);
        }
    }

    private IEnumerator MoveTextEnd_NextFrame()
    {
        yield return 0; // Skip the first frame in which this is called.
        inputField.MoveTextEnd(false); // Do this during the next frame.
    }

    public void SendMessageToChat(string text, int type)
    {
        if (messageList.Count >= maxMessages)
        {
            Destroy(messageList[0].textObject.gameObject);
            messageList.Remove(messageList[0]);
        }
        Message newMessage = new Message();
        newMessage.text = text;
        GameObject newText = Instantiate(textMessageObject, scrollViewContent.transform);
        newMessage.textObject = newText.GetComponent<Text>();
        newMessage.textObject.text = newMessage.text;
        switch (type)
        {
            case 0: // system
                newMessage.textObject.color = Color.yellow;
                break;

            case 1: // normal chat
                newMessage.textObject.color = Color.white;
                break;

            case 2: // personal message
                newMessage.textObject.color = Color.magenta;
                break;
        }
        messageList.Add(newMessage);
    }
}

[System.Serializable]
public class Message
{
    public string text;
    public Text textObject;
}
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

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
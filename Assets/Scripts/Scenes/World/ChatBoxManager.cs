using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using System.Collections;

/**
 * Author: Pantelis Andrianakis
 * Date: September 26th 2018
 */
public class ChatBoxManager : MonoBehaviour
{
    public static ChatBoxManager Instance { get; private set; }

    public GameObject chatPanel;
    public GameObject textObject;
    public InputField inputField;
    private List<Message> messageList = new List<Message>();
    private static readonly string TIMESTAMP_FORMAT = "HH:mm:ss tt";
    private static readonly int MAX_MESSAGE_COUNT = 50;
    private string lastTell = "";

    private void Start()
    {
        if (Instance != null)
        {
            return;
        }
        Instance = this;
    }

    private void Update()
    {
        if (InputManager.RETURN_DOWN)
        {
            MainManager.Instance.isChatBoxActive = !MainManager.Instance.isChatBoxActive;

            if (MainManager.Instance.isChatBoxActive)
            {
                if (lastTell.Length > 0)
                {
                    inputField.text = "/tell " + lastTell + " ";
                    StartCoroutine(MoveToTextEndOnNextFrame());
                }
                inputField.ActivateInputField();
                return;
            }

            if (inputField.text.Length > 0)
            {
                if (Application.isEditor)
                {
                    SendMessageToChat(inputField.text, 1);
                }
                else
                {
                    NetworkManager.ChannelSend(new ChatRequest(inputField.text));
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
                inputField.text = "";
                inputField.DeactivateInputField();
            }
        }

        if (InputManager.ESCAPE_DOWN && MainManager.Instance.isChatBoxActive)
        {
            MainManager.Instance.isChatBoxActive = false;
            inputField.DeactivateInputField();
        }
    }

    private IEnumerator MoveToTextEndOnNextFrame()
    {
        yield return 0; // Skip the first frame in which this is called.
        inputField.MoveTextEnd(false); // Do this during the next frame.
    }

    public void SendMessageToChat(string text, int type)
    {
        if (messageList.Count >= MAX_MESSAGE_COUNT)
        {
            Destroy(messageList[0].textObject.gameObject);
            messageList.Remove(messageList[0]);
        }
        Message message = new Message { text = OptionsManager.useChatTimestamps ? DateTime.Now.ToString(TIMESTAMP_FORMAT) + " " + text : text };
        GameObject newText = Instantiate(textObject, chatPanel.transform);
        message.textObject = newText.GetComponent<Text>();
        message.textObject.text = message.text;
        switch (type)
        {
            case 0: // system
                message.textObject.color = Util.IntToColor(OptionsManager.chatColorSystemIntValue);
                break;

            case 1: // normal chat
                message.textObject.color = Util.IntToColor(OptionsManager.chatColorNormalIntValue);
                break;

            case 2: // personal message
                message.textObject.color = Util.IntToColor(OptionsManager.chatColorMessageIntValue);
                break;
        }
        messageList.Add(message);
    }
}

public class Message
{
    public string text;
    public Text textObject;
}
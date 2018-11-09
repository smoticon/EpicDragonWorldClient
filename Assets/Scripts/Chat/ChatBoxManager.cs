using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Collections;

/**
 * Author: Pantelis Andrianakis
 * Date: September 26th 2018
 */
public class ChatBoxManager : MonoBehaviour
{
    public static ChatBoxManager instance;
    public GameObject textMessageObject;
    public GameObject scrollViewContent;
    public GameObject scrollViewRightBar;
    public InputField inputField;
    private Color32 colorSystem = new Color32(255, 110, 0, 255);
    private Color32 colorNormal = new Color32(255, 255, 255, 255);
    private Color32 colorMessage = new Color32(255, 0, 80, 255);
    public bool isFocused = false;
    private bool decayEnabled = true; // Use to enable or disable decay.
    private float decayTime = 0; // Time when decay will occur.
    private float decayDelay = 5; // Time chat will be visible after message.
    private float decayFadeDelay = 0.5f; // Time of fade transition.
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
        float time = Time.time;

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
                if (decayEnabled && !IsVisible())
                {
                    StartCoroutine(FadeChat(0, 1)); // Show chat.
                }
            }
        }

        if (inputField.isFocused)
        {
            isFocused = true;
            scrollViewRightBar.SetActive(true);
            decayTime = time + decayDelay;
        }
        else
        {
            isFocused = false;
            scrollViewRightBar.SetActive(false);

            // Hide.
            if (decayEnabled && IsVisible() && decayTime < time)
            {
                StartCoroutine(FadeChat(1, 0)); // Hide chat.
            }
        }
    }

    private IEnumerator MoveTextEnd_NextFrame()
    {
        yield return 0; // Skip the first frame in which this is called.
        inputField.MoveTextEnd(false); // Do this during the next frame.
    }

    public IEnumerator FadeChat(float start, float end)
    {
        if (!(inputField.isFocused && end == 0))
        {
            float _timeStartedLerping = Time.time;
            float timeSinceStarted = Time.time - _timeStartedLerping;
            float percentageComplete = timeSinceStarted / decayFadeDelay;

            while (true)
            {
                timeSinceStarted = Time.time - _timeStartedLerping;
                percentageComplete = timeSinceStarted / decayFadeDelay;
                float currentValue = Mathf.Lerp(start, end, percentageComplete);
                inputField.GetComponentInParent<CanvasGroup>().alpha = currentValue;
                if (percentageComplete >= 1)
                {
                    break;
                }
                yield return new WaitForFixedUpdate();
            }
        }
    }

    private bool IsVisible()
    {
        return inputField.GetComponentInParent<CanvasGroup>().alpha == 1;
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
                newMessage.textObject.color = colorSystem;
                break;

            case 1: // normal chat
                newMessage.textObject.color = colorNormal;
                break;

            case 2: // personal message
                newMessage.textObject.color = colorMessage;
                break;
        }
        messageList.Add(newMessage);

        if (decayEnabled && !IsVisible())
        {
            decayTime = Time.time + decayDelay; // Decay time delay is 5 seconds.
            StartCoroutine(FadeChat(0, 1)); // Show chat.
        }
    }
}

[System.Serializable]
public class Message
{
    public string text;
    public Text textObject;
}
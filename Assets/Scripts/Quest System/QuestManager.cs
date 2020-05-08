using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using System;
using UnityEngine.UI;
using UnityEditor;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance { get; private set; }
    public Dictionary<int, Quest> questDictionary = new Dictionary<int, Quest>();

    Transform questWindow;
    Transform questListWindow;

    void Start()
    {
        instance = this;
        questWindow = GameObject.Find("Canvas - Quest/Quest Info").transform;
        questWindow.gameObject.SetActive(false);

        questListWindow = GameObject.Find("Canvas - Quest/Quest Grid").transform;
        questListWindow.gameObject.SetActive(false);

        //        LoadQuests();

        LoadQuest(1);
        LoadQuest(2);

    }

    public void ShowQuestInfo(Quest quest)
    {
        // Show Quest Info Panel
        questWindow.gameObject.SetActive(true);

        // Show or Hide accept button dependinig on "do we have this quest?".
        questWindow.Find("Accept Button").gameObject.SetActive(!Character.activeQuests.ContainsKey(quest.id));

        // Remove previous functions from the Accept Button
        questWindow.Find("Accept Button").GetComponent<Button>().onClick.RemoveAllListeners();
        // Set functions on Accept Button
        questWindow.Find("Accept Button").GetComponent<Button>().onClick.AddListener(() => {
            Character.AddQuest(quest.id);
            questWindow.gameObject.SetActive(false);
            ShowActiveQuests();
        });

        questWindow.Find("Quest Name").GetComponent<TextMeshProUGUI>().text = quest.questName;
        questWindow.Find("Quest Name").GetComponent<TextMeshProUGUI>().text = questWindow.Find("Quest Name").GetComponent<TextMeshProUGUI>().text.Replace("[NpcName]", "<color=yellow>" + NpcName + "</color>");

        TextMeshProUGUI textMesh = questWindow.Find("Quest Description").GetComponent<TextMeshProUGUI>();
        textMesh.text = quest.questDescription;
        textMesh.text = textMesh.text.Replace("[CharacterName]", "<color=green>" + MainManager.Instance.selectedCharacterData.GetName() + "</color>");
        textMesh.text = textMesh.text.Replace("[NpcName]", "<color=yellow>" + NpcName + "</color>");

        // TASK
        string taskString = "Task:\n";
        if (quest.task.kills != null)
        {
            int killIndex = 0;
            foreach (Quest.QuestKill qk in quest.task.kills)
            {
                // Current kills in zero when we haven't taken the quest
                int currentKills = 0;
                if (Character.activeQuests.ContainsKey(qk.id) && Character.monstersKilled.ContainsKey(qk.id))
                    // if we are showing the info during the progress of the quest (we took it already) show the progress
                    currentKills = Character.monstersKilled[qk.id].amount - Character.activeQuests[quest.id].kills[killIndex].initialAmount;
                taskString += "Kill " + (currentKills) + " / " + qk.amount + " " + MonsterDatabase.monsters[qk.id] + "\n";
                killIndex++;
            }
        }

        if (quest.task.items != null)
        {
            foreach (Quest.QuestItem qi in quest.task.items)
            {
                taskString += "Bring " + qi.amount + " x " + ItemData.itemDB[qi.id].ItemName + ".\n";
            }
        }

        if (quest.task.talkTo != null)
        {
            foreach (int id in quest.task.talkTo)
            {
                taskString += "Go and talk with " + "<color=yellow>" + NpcData.GetNpc(id).GetName() + "</color>\n";
            }
        }
        questWindow.Find("Quest Task").GetComponent<TextMeshProUGUI>().text = taskString;

        // REWARD
        string rewardString = "Reward:\n";
        if (quest.reward.items != null)
        {
            foreach (Quest.QuestItem qi in quest.reward.items)
            {
                rewardString += qi.amount + " x " + ItemData.itemDB[qi.id].ItemName + "\n";
            }
        }
        questWindow.Find("Quest Reward").GetComponent<TextMeshProUGUI>().text = rewardString;
    }

    private void ShowActiveQuests()
    {
        foreach(Character.ActiveQuest activeQuest in Character.activeQuests.Values)
        {
            int i = activeQuest.id;

            if (GameObject.Find("Canvas - Quest/Quest Grid/Scroll View/Viewport/Content/" + i.ToString()) != null)
            {
                Debug.Log("Quest Id: " + i + " found.");
                continue; // if we found this quest id as one of the children of questBookContent, we skip the creation of this button
            }
            // Create new quest button
            // GameObject QuestButtonGo = Instantiate(Resources.Load("Prefabs/Quest Button") as GameObject);
            questListWindow.gameObject.SetActive(true);
            GameObject QuestButtonGo = (GameObject)Instantiate((GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Quest Button.prefab", typeof(GameObject)));
            QuestButtonGo.name = questDictionary[i].id.ToString();
            QuestButtonGo.transform.SetParent(GameObject.Find("Canvas - Quest/Quest Grid/Scroll View/Viewport/Content").transform);
            QuestButtonGo.transform.localScale = Vector3.one;
            QuestButtonGo.transform.Find("Text").GetComponent<Text>().text = questDictionary[i].questName;
            int questId = new int();
            questId = i;
            QuestButtonGo.GetComponent<Button>().onClick.AddListener(() => {
                ShowQuestInfo(questDictionary[questId]);
            });
        }
    }

    public bool IsQuestAvailable(int questId, Character character)
    {
        return (questDictionary[questId].requiredLevel <= character.playerLevel);
    }

    public bool IsQuestFinished(int questId)
    {
        Quest quest = questDictionary[questId];
        // If there is at least one kill that we are required to do.
        if(quest.task.kills.Length > 0)
        {
            // Foreach kill that must do
            foreach(var questKill in quest.task.kills)
            {
                if (!Character.monstersKilled.ContainsKey(questKill.id))
                {
                    return false;
                }
                int currentKills = Character.monstersKilled[questKill.id].amount - Character.activeQuests[quest.id].kills[questKill.id].initialAmount;
                if(currentKills < questKill.amount)
                {
                    return false;
                }
            }
        }

        if(quest.task.talkTo.Length > 0)
        {
            foreach(var talkTo in quest.task.talkTo)
            {
                if (Character.activeQuests.ContainsKey(talkTo))
                    Debug.Log("You talk to " + talkTo);
//                if(!Character.talkTo.ContainsKey(talkTo))
            }
        }
        Debug.Log("TODO IsQuestFinished in QuestManager.");
        //Do the same but check Items on Inventory.
        //If we don't have the required items at any point, return false.

        //Same for "talked to". Return false if incomplete.

        //If at any point the quest is incomplete, we would have returned false and stop running.
        //Since we reach this point, the quest is complete, so we return true.

        return true;
    }

    public void HideQuestInfo()
    {
        if (questWindow != null)
            questWindow.gameObject.SetActive(false);
    }

    string NpcName;

    public void QuestInfo(WorldObject target)
    {
        // Temp fix NpcId = QuestId
        if(target != null)
        {
            // Did player finished this quest?
            if (!Character.Instance.finishedQuest.Contains((int)target.objectId) &&
            // Do the player meet the requirements?
            IsQuestAvailable((int)target.objectId, GameObject.Find("Manager - Player").GetComponent<Character>()))
            {
                ShowQuestInfo(questDictionary[(int)target.objectId]);

                //Set the Complete Quest Button
                if (QuestManager.instance.IsQuestFinished((int)target.objectId))
                {
                    // TODO
                    Debug.Log("Remove from Quest List");
                }
            }
        }
        

/*        NpcController npcController = target.GetComponent<NpcController>();
        if(npcController != null)
        {
            npcController.OnClick();
            return;
        }
*/               
/*        if (target != null && target.objectId == 1)
        {
            NpcName = target.characterData.GetName();
            ShowQuestInfo(questDictionary[0]);
        }
        else if (target != null && target.objectId == 2)
        {
            NpcName = target.characterData.GetName();
            ShowQuestInfo(questDictionary[1]);
        }
        else
        {
            HideQuestInfo();
        }
*/    }

    void LoadQuests()
    {
        DirectoryInfo dir = new DirectoryInfo("Assets/Resources/data/Quests");
        FileInfo[] info = dir.GetFiles("*.txt");
        foreach(FileInfo f in info)
        {
            Quest newQuest = JsonUtility.FromJson<Quest>(Resources.Load<TextAsset>("data/Quests/" + f.Name.Replace(".txt","")).text);
            questDictionary.Add(newQuest.id, newQuest);
        }
        // Quest newQuest = JsonUtility.FromJson<Quest>(Resources.Load<TextAsset>("data/QuestData").text);
        // questDictionary.Add(newQuest.id, newQuest);
        Debug.Log("Quests Loaded: " + questDictionary.Count);
    }

    public void LoadQuest(int id)
    {
        Quest newQuest = JsonUtility.FromJson<Quest>(Resources.Load<TextAsset>("data/Quests/" + id.ToString("00") + "_Quest").text);
        if(!questDictionary.ContainsKey(newQuest.id))
            questDictionary.Add(newQuest.id, newQuest);
    }

    public void LoadQuestsFile() // Method for All Quests in one file
    {
        string[] questDataArray = MethodHelper.LoadResourceTextfile("data/QuestData").Split(';');
        foreach(string s in questDataArray)
        {
            Quest newQuest = JsonUtility.FromJson<Quest>(s);
            questDictionary.Add(newQuest.id, newQuest);
        }
        Debug.Log("Quest Size: " + questDictionary.Count);
    }
}

public class MethodHelper
{

    public static string LoadResourceTextfile(string path)
    {
        TextAsset targetFile = Resources.Load<TextAsset>(path);
        return targetFile.text;
    }

}

// temporary database
public class MonsterDatabase
{
    public static Dictionary<int, string> monsters = new Dictionary<int, string>()
    {
        {0, "Mummy"},
        {1, "Zombie"},
        {2, "Ghost"},
        {3, "Wild Boar"}
    };
}
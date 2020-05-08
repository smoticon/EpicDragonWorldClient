using UnityEngine;
using UnityEngine.UI;
using edw.CharacterStats;
using System.Collections.Generic;
using System;

public class Character : MonoBehaviour
{
    public static Character Instance { get; private set; }

    [Header("Character Stats")]
    public CharacterStat Stamina;
    public CharacterStat Strength;
    public CharacterStat Dexterity;
    public CharacterStat Intelect;

    public int playerLevel = 1;

    [Header("Public")]
    public Inventory Inventory;
    public EquipmentPanel EquipmentPanel;

    [Header("Serialize Field")]
    [SerializeField] StatPanel statPanel;
    [SerializeField] ItemTooltip itemTooltip;
    [SerializeField] ItemInfoTooltip itemInfoTooltip;
    [SerializeField] Image draggableItem;
    [SerializeField] InventoryManager inventoryManager;
    private BaseItemSlot dragItemSlot;

    // TODO: Quest system - in future move to single file QuestData
    public static Dictionary<int, ActiveQuest> activeQuests = new Dictionary<int, ActiveQuest>();
    public List<int> finishedQuest = new List<int>();
    public static Dictionary<int, MonsterKills> monstersKilled = new Dictionary<int, MonsterKills>();

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        inventoryManager.LoadInventory(this);

        statPanel.SetStats(Stamina, Strength, Dexterity, Intelect);
        statPanel.UpdateStatValues();

        // EVENTS SETUP:
        // Right Click
        Inventory.OnRightClickEvent += InventoryRightClick;
        EquipmentPanel.OnRightClickEvent += EquipmentPanelRightClick;

        // Point Enter
        Inventory.OnPointerEnterEvent += ShowTooltip;
        EquipmentPanel.OnPointerEnterEvent += ShowTooltip;

        // Pointer Exit
        Inventory.OnPointerExitEvent += HideTooltip;
        EquipmentPanel.OnPointerExitEvent += HideTooltip;

        // Begin Drag
        Inventory.OnBeginDragEvent += BeginDrag;
        EquipmentPanel.OnBeginDragEvent += BeginDrag;
        // End Drag
        Inventory.OnEndDragEvent += EndDrag;
        EquipmentPanel.OnEndDragEvent += EndDrag;
        // Drag
        Inventory.OnDragEvent += Drag;
        EquipmentPanel.OnDragEvent += Drag;
        // Drop
        Inventory.OnDropEvent += Drop;
        EquipmentPanel.OnDropEvent += Drop;

        inventoryManager.LoadEquipment(this);
    }

    private void InventoryRightClick(BaseItemSlot itemSlot)
    {
        dragItemSlot = itemSlot;

        if (itemSlot.Item is Item)
            itemInfoTooltip.ShowTooltip(this, itemSlot.Item);


 /*       if (itemSlot.Item is Item)
        {
            dragItemSlot.EnchantLvl = itemSlot.EnchantLvl;
            Equip((Item)itemSlot.Item);
        }
        // TODO if item is consumable
*/    }

    private void EquipmentPanelRightClick(BaseItemSlot itemSlot)
    {
        if(itemSlot.Item is Item)
        {
            Unequip(itemSlot.Item);
        }
    }

    private void ShowTooltip(BaseItemSlot itemSlot)
    {
       if(itemSlot.Item != null)
        {
            itemTooltip.ShowTooltip(itemSlot.Item);
            transform.position = Input.mousePosition;
        }
    }

    private void HideTooltip(BaseItemSlot itemSlot)
    {
        itemTooltip.HideTooltip();
    }

    private void BeginDrag(BaseItemSlot itemSlot)
    {
        if (itemSlot.Item != null)
        {
            dragItemSlot = itemSlot;
            draggableItem.sprite = itemSlot.Item.Icon;
            draggableItem.transform.position = Input.mousePosition;
            draggableItem.enabled = true;
        }
    }

    private void Drag(BaseItemSlot itemSlot)
    {
        if (draggableItem.enabled)
        {
            draggableItem.transform.position = Input.mousePosition;
        }
    }

    private void EndDrag(BaseItemSlot itemSlot)
    {
        dragItemSlot = null;
        draggableItem.gameObject.SetActive(false);
    }

    private void Drop(BaseItemSlot dropItemSlot)
    {
        if (draggableItem == null) return;

        if (dropItemSlot.CanAddStack(dragItemSlot.Item))
        {
            AddStacks(dropItemSlot);
        }
        if (dropItemSlot.CanReceiveItem(dragItemSlot.Item) && dragItemSlot.CanReceiveItem(dropItemSlot.Item))
        {
            SwapItems(dropItemSlot);
        }
    }

    private void SwapItems(BaseItemSlot dropItemSlot)
    {
        Item dragItem = dragItemSlot.Item as Item;
        Item dropItem = dropItemSlot.Item as Item;

        if (dropItemSlot is EquipmentSlot)
        {
            if (dragItem != null) dragItem.Equip(this);
            if (dropItem != null) dropItem.Unequip(this);
        }

        if (dragItemSlot is EquipmentSlot)
        {
            if (dragItem != null) dragItem.Unequip(this);
            if (dropItem != null) dropItem.Equip(this);
        }
        statPanel.UpdateStatValues();

        Item draggedItem = dragItemSlot.Item;
        int draggedItemAmount = dragItemSlot.Amount;
        int draggedItemEnchant = dragItemSlot.EnchantLvl;

        dragItemSlot.Item = dropItemSlot.Item;
        dragItemSlot.Amount = dropItemSlot.Amount;
        dragItemSlot.EnchantLvl = dropItemSlot.EnchantLvl;

        dropItemSlot.Item = draggedItem;
        dropItemSlot.Amount = draggedItemAmount;
        dropItemSlot.EnchantLvl = draggedItemEnchant;
    }

    private void AddStacks(BaseItemSlot dropItemSlot)
    {
        int numAddableStacks = dropItemSlot.Item.MaximumStacks - dropItemSlot.Amount;
        int stackToAdd = Mathf.Min(numAddableStacks, dragItemSlot.Amount);

        dropItemSlot.Amount += stackToAdd;
        dragItemSlot.Amount -= stackToAdd;
    }

    public void Equip(Item item)
    {
       foreach (ItemInfoHolder itemInfo in inventoryManager.itemsList)
        {
            // TODO search item after unique id
            if (item.itemId == itemInfo.GetItemId())
                item.EnchantLvl = itemInfo.GetEnchantLvl();
        }

        if (Inventory.RemoveItem(item))
        {
            Item previousItem;

            if (EquipmentPanel.AddItem(item, out previousItem))
            {
                if (previousItem != null)
                {
                    Inventory.AddItem(previousItem);
                    previousItem.Unequip(this);
                    statPanel.UpdateStatValues();
                }

                item.Equip(this);
                statPanel.UpdateStatValues();
                // Debug.Log("Equip: " + item.itemId);
                CharacterManager.Instance.EquipItem(WorldManager.Instance.activeCharacter, item.itemId);
            }
            else
            {
                Inventory.AddItem(item);
            }
        }
    }

    public void Unequip(Item item)
    {
        if(Inventory.CanAddItem(item) && EquipmentPanel.RemoveItem(item))
        {
            CharacterManager.Instance.UnEquipItem(WorldManager.Instance.activeCharacter, item.EquipmentType);
            item.Unequip(this);
            statPanel.UpdateStatValues();

            Inventory.AddItem(item);
        }
    }

    // Quest System

    public static void AddQuest(int id)
    {
        // If we already accepted this quest, we won't accept it again
        if (activeQuests.ContainsKey(id)) return;

        // Otherwise, we create a new ActiveQuest.
        Quest quest = QuestManager.instance.questDictionary[id]; // reference to our quest
        ActiveQuest newActiveQuest = new ActiveQuest();
        newActiveQuest.id = id;
        newActiveQuest.dateTaken = DateTime.Now.ToLongDateString();

        // If we need to kill monsters on this quest...
        if(quest.task.kills.Length > 0)
        {
            // set the kills of the new active quest as new array of the kills
            newActiveQuest.kills = new Quest.QuestKill[quest.task.kills.Length];
            //for every kill in our quest.task
            foreach(Quest.QuestKill questKill in quest.task.kills)
            {
                // set each quest kill to a new instance of questKill
                newActiveQuest.kills[questKill.id] = new Quest.QuestKill();
                // set the player current amount of kills of the new active quest based on the 
                if (!monstersKilled.ContainsKey(questKill.id)) monstersKilled.Add(questKill.id, new Character.MonsterKills());
                newActiveQuest.kills[questKill.id].initialAmount = monstersKilled[questKill.id].amount;
            }
        }

        if(quest.task.talkTo.Length > 0)
        {
            // TODO
            newActiveQuest.npcId = quest.task.talkTo[0];
            Debug.Log("Talk to " + newActiveQuest.npcId);
        }
        activeQuests.Add(id, newActiveQuest);
    }

    // How many monsters[id] have we killed in total.
    public class MonsterKills
    {
        public int id;
        public int amount;
    }

    // Holds information specific to the instance of this ques.
    // Useful for repetable quests
    public class ActiveQuest
    {
        public int id; // Id of the quest taken
        public int npcId; // Id of Npc toTalk
        public string dateTaken;
        public Quest.QuestKill[] kills; // Holds the task monster ID and the amount of surrent monsters kill
    }
}

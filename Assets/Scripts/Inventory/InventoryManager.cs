using System.Collections;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }
    public ArrayList itemsList = new ArrayList();

    private bool isReady = false;

    [SerializeField] private Canvas _canvas;

    public bool IsDisplaying
    {
        get { return _canvas.enabled; }
        set { _canvas.enabled = value; }
    }

    void Start()
    {
        Instance = this;
        _canvas.enabled = false;
    }

    public void CharacterItems(ArrayList charItems)
    {
        foreach(ItemInfoHolder itemInfo in charItems)
        {
            itemInfo.GetItemId();
            itemInfo.IsEquipped();
            itemInfo.GetAmount();
            itemInfo.GetEnchantLvl();
            itemsList.Add(itemInfo);
        }
        
        isReady = true;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (InputManager.INVENTORY_DOWN)
            IsDisplaying = !IsDisplaying;
    }

    public void LoadInventory(Character character)
    {
        character.Inventory.Clear();
        
        while(!isReady)
        {
            // Wait to read items from server
        }
        Debug.Log("Player Item list: " + itemsList.Count);

        int _slot = 0;
        Debug.Log("Slots: " + character.Inventory.ItemSlots.Count);
        foreach (ItemInfoHolder itemInfo in itemsList)
        {
            ItemSlot itemSlot = character.Inventory.ItemSlots[_slot];
            itemSlot.Item = ItemData.GetItemInfo(itemInfo.GetItemId());
            itemSlot.Amount = itemInfo.GetAmount();
            itemSlot.isEquiped = itemInfo.IsEquipped();
            itemSlot.EnchantLvl = itemInfo.GetEnchantLvl();
            _slot++;

        }
    }

    public void LoadEquipment(Character character)
    {
        for (int i = 0; i < character.Inventory.ItemSlots.Count; i++)
        {
            ItemSlot itemSlot = character.Inventory.ItemSlots[i];
            if (itemSlot.isEquiped)
            {
//                CharacterManager.Instance.EquipItem(WorldManager.Instance.activeCharacter, itemSlot.Item.itemId);               
                character.Equip(itemSlot.Item);
            }
        }
    }
}

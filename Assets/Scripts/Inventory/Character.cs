using UnityEngine;
using UnityEngine.UI;
using edw.CharacterStats;

public class Character : MonoBehaviour
{
    public static Character Instance { get; private set; }

    [Header("Character Stats")]
    public CharacterStat Stamina;
    public CharacterStat Strength;
    public CharacterStat Dexterity;
    public CharacterStat Intelect;


    [Header("Public")]
    public Inventory Inventory;
    public EquipmentPanel EquipmentPanel;

    [Header("Serialize Field")]
    [SerializeField] StatPanel statPanel;
    [SerializeField] ItemTooltip itemTooltip;
    [SerializeField] Image draggableItem;
    [SerializeField] InventoryManager inventoryManager;
    private BaseItemSlot dragItemSlot;

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
    }

    private void InventoryRightClick(BaseItemSlot itemSlot)
    {
        dragItemSlot = itemSlot;
        if (itemSlot.Item is Item)
        {
            dragItemSlot.EnchantLvl = itemSlot.EnchantLvl;
            Equip((Item)itemSlot.Item);
        }
        // TODO if item is consumable
    }

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
        item.EnchantLvl = dragItemSlot.EnchantLvl;

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
                CharacterManager.Instance.EquipItem(WorldManager.Instance.activeCharacter, item.itemId);
                statPanel.UpdateStatValues();
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
}

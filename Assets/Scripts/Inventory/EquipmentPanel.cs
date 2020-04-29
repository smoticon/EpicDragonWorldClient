using System;
using UnityEngine;

public class EquipmentPanel : MonoBehaviour
{
    [SerializeField] Transform equipmentSlotsParent;
    public EquipmentSlot[] EquipmentSlots;

    public event Action<BaseItemSlot> OnPointerEnterEvent;
    public event Action<BaseItemSlot> OnPointerExitEvent;
    public event Action<BaseItemSlot> OnRightClickEvent;
    public event Action<BaseItemSlot> OnBeginDragEvent;
    public event Action<BaseItemSlot> OnEndDragEvent;
    public event Action<BaseItemSlot> OnDragEvent;
    public event Action<BaseItemSlot> OnDropEvent;

    public void Start()
    {
        for (int i = 0; i < EquipmentSlots.Length; i++)
        {
            EquipmentSlots[i].OnPointerEnterEvent += slot => OnPointerEnterEvent(slot);
            EquipmentSlots[i].OnPointerExitEvent += slot => OnPointerExitEvent(slot);
            EquipmentSlots[i].OnRightClickEvent += slot => OnRightClickEvent(slot);
            EquipmentSlots[i].OnBeginDragEvent += slot => OnBeginDragEvent(slot);
            EquipmentSlots[i].OnEndDragEvent += slot => OnEndDragEvent(slot);
            EquipmentSlots[i].OnDragEvent += slot => OnDragEvent(slot);
            EquipmentSlots[i].OnDropEvent += slot => OnDropEvent(slot);
        }
    }

    private void OnValidate()
    {
        EquipmentSlots = equipmentSlotsParent.GetComponentsInChildren<EquipmentSlot>();
    }

    public bool AddItem(Item item, out Item previousItem)
    {
        for (int i = 0; i < EquipmentSlots.Length; i++)
        {
            if(EquipmentSlots[i].EquipmentType == item.EquipmentType)
            {
                previousItem = EquipmentSlots[i].Item;
                EquipmentSlots[i].Item = item;
                EquipmentSlots[i].Amount = 1;
                EquipmentSlots[i].EnchantLvl = item.GetEnchantLvl();
                // TODO                EquipmentSlots[i].ItemLvl = item.GetItemLvl();
                return true;
            }
        }
        previousItem = null;
        return false;
    }

    public bool RemoveItem(Item item)
    {
        for (int i = 0; i < EquipmentSlots.Length; i++)
        {
            if(EquipmentSlots[i].Item == item)
            {
                EquipmentSlots[i].Item = null;
                EquipmentSlots[i].Amount = 0;
                EquipmentSlots[i].EnchantLvl = 0;
                EquipmentSlots[i].ItemLvl = 0;
                return true;
            }
        }
        return false;
    }
}

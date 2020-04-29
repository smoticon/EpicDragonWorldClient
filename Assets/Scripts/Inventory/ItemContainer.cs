using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemContainer : MonoBehaviour, IItemContainer
{
    public List<ItemSlot> ItemSlots;

    public event Action<BaseItemSlot> OnPointerEnterEvent;
    public event Action<BaseItemSlot> OnPointerExitEvent;
    public event Action<BaseItemSlot> OnRightClickEvent;
    public event Action<BaseItemSlot> OnBeginDragEvent;
    public event Action<BaseItemSlot> OnEndDragEvent;
    public event Action<BaseItemSlot> OnDragEvent;
    public event Action<BaseItemSlot> OnDropEvent;

    protected virtual void OnValidate()
    {
        GetComponentsInChildren(includeInactive: true, result: ItemSlots);
    }

    protected virtual void Awake()
    {

    }

    public virtual bool CanAddItem(Item item, int amount = 1)
    {
        int freeSpaces = 0;

        foreach (ItemSlot itemSlot in ItemSlots)
        {
            if (itemSlot.Item == null || itemSlot.Item.itemId == item.itemId)
            {
                freeSpaces += item.MaximumStacks - itemSlot.Amount;
            }
        }
        return freeSpaces >= amount;
    }

    public virtual bool AddItem(Item item)
    {
        for (int i = 0; i < ItemSlots.Count; i++)
        {
            if (ItemSlots[i].CanAddStack(item))
            {
                ItemSlots[i].Item = item;
                ItemSlots[i].Amount++;
                ItemSlots[i].EnchantLvl = item.GetEnchantLvl();
 // TODO               ItemSlots[i].ItemLvl = item.GetItemLvl();
                return true;
            }
        }

        for (int i = 0; i < ItemSlots.Count; i++)
        {
            if (ItemSlots[i].Item == null)
            {
                ItemSlots[i].Item = item;
                ItemSlots[i].Amount++;
                ItemSlots[i].EnchantLvl = item.GetEnchantLvl();
// TODO               ItemSlots[i].ItemLvl = item.GetItemLvl();
                return true;
            }
        }
        return false;
    }

    public virtual bool RemoveItem(Item item)
    {
        for (int i = 0; i < ItemSlots.Count; i++)
        {
            if (ItemSlots[i].Item == item)
            {
                ItemSlots[i].Amount--;
                ItemSlots[i].EnchantLvl = item.GetEnchantLvl();
// TODO               ItemSlots[i].ItemLvl = item.GetItemLvl();
                return true;
            }
        }
        return false;
    }

    public virtual Item RemoveItem(int itemID)
    {
        for (int i = 0; i < ItemSlots.Count; i++)
        {
            Item item = ItemSlots[i].Item;
            if (item != null && item.itemId == itemID)
            {
                ItemSlots[i].Amount--;
                ItemSlots[i].EnchantLvl = item.GetEnchantLvl();
// TODO               ItemSlots[i].ItemLvl = item.GetItemLvl();
                return item;
            }
        }
        return null;
    }

    public virtual int ItemCount(int itemID)
    {
        int number = 0;

        for (int i = 0; i < ItemSlots.Count; i++)
        {
            Item item = ItemSlots[i].Item;
            if (item != null && item.itemId == itemID)
            {
                number += ItemSlots[i].Amount;
            }
        }
        return number;
    }

    public void SlotEvents()
    {
        for (int i = 0; i < ItemSlots.Count; i++)
        {
            ItemSlots[i].OnPointerEnterEvent += slot => EventHelper(slot, OnPointerEnterEvent);
            ItemSlots[i].OnPointerExitEvent += slot => EventHelper(slot, OnPointerExitEvent);
            ItemSlots[i].OnRightClickEvent += slot => EventHelper(slot, OnRightClickEvent);
            ItemSlots[i].OnBeginDragEvent += slot => EventHelper(slot, OnBeginDragEvent);
            ItemSlots[i].OnEndDragEvent += slot => EventHelper(slot, OnEndDragEvent);
            ItemSlots[i].OnDragEvent += slot => EventHelper(slot, OnDragEvent);
            ItemSlots[i].OnDropEvent += slot => EventHelper(slot, OnDropEvent);
        }
    }

    private void EventHelper(BaseItemSlot itemSlot, Action<BaseItemSlot> action)
    {
        if (action != null)
            action(itemSlot);
    }

    public void Clear()
    {
        GetComponentsInChildren(includeInactive: true, result: ItemSlots);
        for (int i = 0; i < ItemSlots.Count; i++)
        {

            if (ItemSlots[i].Item != null && Application.isPlaying)
            {
                ItemSlots[i].Item.Destroy();
            }
            ItemSlots[i].Item = null;
            ItemSlots[i].Amount = 0;
            ItemSlots[i].EnchantLvl = 0;
            ItemSlots[i].ItemLvl = 0;

            ItemSlots[i].OnPointerEnterEvent += slot => EventHelper(slot, OnPointerEnterEvent);
            ItemSlots[i].OnPointerExitEvent += slot => EventHelper(slot, OnPointerExitEvent);
            ItemSlots[i].OnRightClickEvent += slot => EventHelper(slot, OnRightClickEvent);
            ItemSlots[i].OnBeginDragEvent += slot => EventHelper(slot, OnBeginDragEvent);
            ItemSlots[i].OnEndDragEvent += slot => EventHelper(slot, OnEndDragEvent);
            ItemSlots[i].OnDragEvent += slot => EventHelper(slot, OnDragEvent);
            ItemSlots[i].OnDropEvent += slot => EventHelper(slot, OnDropEvent);
        }
    }
}

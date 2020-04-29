using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BaseItemSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected Image itemImage;
    [SerializeField] protected Text amountText;
    [SerializeField] protected Text enchantLvlText;
    [SerializeField] protected Text itemLvlText;
    [SerializeField] protected Text rankText;

    public event Action<BaseItemSlot> OnPointerEnterEvent;
    public event Action<BaseItemSlot> OnPointerExitEvent;
    public event Action<BaseItemSlot> OnRightClickEvent;

    protected bool isPointerOver;

    protected Color normalColor = Color.white;
    protected Color disabledColor = new Color(1, 1, 1, 0);

    protected Item _item;
    public Item Item
    {
        get { return _item; }
        set
        {
            _item = value;
            if (_item == null && Amount != 0) Amount = 0;

            if (_item == null)
            {
                itemImage.sprite = null;
                itemImage.color = disabledColor;
            }
            else
            {
                itemImage.sprite = _item.Icon;
                itemImage.color = normalColor;
            }

            if (isPointerOver)
            {
                OnPointerExit(null);
                OnPointerEnter(null);
            }
        }
    }

    private int _amount;
    public int Amount
    {
        get { return _amount; }
        set
        {
            _amount = value;
            if (_amount < 0) _amount = 0;
            if (_amount == 0 && Item != null) Item = null;

            if (amountText != null)
            {
                amountText.enabled = _item != null && _amount > 1;
                if (amountText.enabled)
                {
                    amountText.text = _amount.ToString();
                }
            }
        }
    }

    private int _enchantLvl;
    public int EnchantLvl
    {
        get { return _enchantLvl; }
        set
        {
            _enchantLvl = value;
            if (_enchantLvl < 0) _enchantLvl = 0;

            if (enchantLvlText != null)
            {
                enchantLvlText.enabled = _item != null && _enchantLvl > 1;
                if (enchantLvlText.enabled)
                {
                    enchantLvlText.text = "+" + _enchantLvl.ToString();
                }
            }
        }
    }

    private int _itemLvl;
    public int ItemLvl
    {
        get { return _itemLvl; }
        set
        {
            _itemLvl = value;
            if (_itemLvl < 0) _itemLvl = 0;

            if(itemLvlText != null)
            {
                itemLvlText.enabled = _item != null && _itemLvl > 1;
                if (itemLvlText.enabled)
                {
                    itemLvlText.text = "Lv. " + _itemLvl.ToString();
                }
            }
        }
    }

    public virtual bool CanAddStack(Item item, int amount = 1)
    {
        return Item != null && Item.itemId == item.itemId;
    }

    public virtual bool CanReceiveItem(Item item)
    {
        return false;
    }

    protected virtual void OnValidate()
    {
        if (itemImage == null)
            itemImage = GetComponent<Image>();

        if (amountText == null)
            amountText = GetComponentInChildren<Text>();

        if (enchantLvlText == null)
            enchantLvlText = GetComponentInChildren<Text>();

        if (itemLvlText == null)
            itemLvlText = GetComponentInChildren<Text>();

        Item = _item;
        Amount = _amount;
        EnchantLvl = _enchantLvl;
    }

    protected virtual void OnDisable()
    {
        if (isPointerOver)
        {
            OnPointerExit(null);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("ITEM slot click");
        if (eventData != null && eventData.button == PointerEventData.InputButton.Right)
        {
            OnRightClickEvent?.Invoke(this);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isPointerOver = true;
        OnPointerEnterEvent?.Invoke(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerOver = false;

        OnPointerExitEvent?.Invoke(this);
    }
}

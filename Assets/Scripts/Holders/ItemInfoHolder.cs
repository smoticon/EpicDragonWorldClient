using UnityEngine;

public class ItemInfoHolder
{
    private readonly int itemId;
    private readonly int equipped;
    private readonly int amount;
    private readonly int enchantLvl;

    public ItemInfoHolder(int itemId, int equipped, int amount, int enchantLvl)
    {
        this.itemId = itemId;
        this.equipped = equipped;
        this.amount = amount;
        this.enchantLvl = enchantLvl;
    }

    public int GetItemId()
    {
        return itemId;
    }

    public int IsEquipped()
    {
        return equipped;
    }

    public int GetAmount()
    {
        return amount;
    }

    public int GetEnchantLvl()
    {
        return enchantLvl;
    }
}

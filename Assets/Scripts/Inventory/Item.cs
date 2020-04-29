using System.Text;
using UnityEngine;
using edw.CharacterStats;

public class Item
{
    public int itemId;
    public string ItemName;
    public string Description;
    public Sprite Icon;
    public EquipmentItemSlot EquipmentType;
    public ItemType ItemType;
    public bool Stackable;

    [Space]
    public int Stamina;
    public int Strength;
    public int Dexterity;
    public int Intelect;

    [Space]
    public int StaminaPercentBonus = 0;
    public int StrengthPercentBonus = 0;
    public int DexterityPercentBonus = 0;
    public int IntelectPercentBonus = 0;

    [Range(1, 999)]
    public int MaximumStacks = 1;

    public int EnchantLvl; // Enchant

    protected static readonly StringBuilder sb = new StringBuilder();

    public Item(int itemId, EquipmentItemSlot itemSlot, ItemType itemType, string itemName, string description, Sprite icon, bool stackable, int stamina, int strength, int dexterity, int intelect)
    {
        this.itemId = itemId;
        this.EquipmentType = itemSlot;
        this.ItemType = itemType;
        this.ItemName = itemName;
        this.Description = description;
        this.Icon = icon;
        this.Stackable = stackable;
        this.Stamina = stamina;
        this.Strength = strength;
        this.Dexterity = dexterity;
        this.Intelect = intelect;
    }

    public virtual Item GetCopy()
    {
        return this;
    }
    public virtual void Destroy()
    {

    }
    
    public virtual string GetItemType()
    {
        return EquipmentType.ToString();
    }

    public virtual string GetDescription()
    {
        return Description;
    }

    public virtual string GetStatsDescription()
    {
        sb.Length = 0;
        AddStat(Stamina, "Stamina");
        AddStat(Strength, "Strength");
        AddStat(Dexterity, "Dexterity");
        AddStat(Intelect, "Intelect");

        AddStat(StaminaPercentBonus, "Stamina", true);
        AddStat(StrengthPercentBonus, "Strength", true);
        AddStat(DexterityPercentBonus, "Dexterity", true);
        AddStat(IntelectPercentBonus, "Intelect", true);
        return sb.ToString();
    }

    public virtual int GetEnchantLvl()
    {
        return EnchantLvl;
    }

    public void Equip(Character c)
    {
        if (Stamina != 0)
            c.Stamina.AddModifier(new StatModifier(Stamina, StatModType.Flat, this));
        if (Strength != 0)
            c.Strength.AddModifier(new StatModifier(Strength, StatModType.Flat, this));
        if (Dexterity != 0)
            c.Dexterity.AddModifier(new StatModifier(Dexterity, StatModType.Flat, this));
        if (Intelect != 0)
            c.Intelect.AddModifier(new StatModifier(Intelect, StatModType.Flat, this));

        if (StaminaPercentBonus != 0)
            c.Stamina.AddModifier(new StatModifier(StaminaPercentBonus, StatModType.PercentMult, this));
        if (StrengthPercentBonus != 0)
            c.Strength.AddModifier(new StatModifier(StrengthPercentBonus, StatModType.PercentMult, this));
        if (DexterityPercentBonus != 0)
            c.Dexterity.AddModifier(new StatModifier(DexterityPercentBonus, StatModType.PercentMult, this));
        if (IntelectPercentBonus != 0)
            c.Intelect.AddModifier(new StatModifier(IntelectPercentBonus, StatModType.PercentMult, this));
    }

    public void Unequip(Character c)
    {
        c.Stamina.RemoveAllModifiersFromSource(this);
        c.Strength.RemoveAllModifiersFromSource(this);
        c.Dexterity.RemoveAllModifiersFromSource(this);
        c.Intelect.RemoveAllModifiersFromSource(this);
    }

    private void AddStat(float value, string statName, bool isPercent = false)
    {
        if (value != 0)
        {
            if (sb.Length > 0)
                sb.AppendLine();

            if (value > 0)
                sb.Append("+");

            if (isPercent)
            {
                sb.Append(value * 100);
                sb.Append("% ");
            }
            else
            {
                sb.Append(value);
                sb.Append(" ");
            }

            sb.Append(statName);
        }
    }
}

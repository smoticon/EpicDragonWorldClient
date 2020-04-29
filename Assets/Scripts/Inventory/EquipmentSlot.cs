public class EquipmentSlot : ItemSlot
{
    public EquipmentItemSlot EquipmentType;

    protected override void OnValidate()
    {
        base.OnValidate();
        gameObject.name = EquipmentType.ToString() + " Slot";
    }

    public override bool CanReceiveItem(Item item)
    {
        if (item == null) return true;

        Item equippableItem = item as Item;
        return equippableItem != null && equippableItem.EquipmentType == EquipmentType;
    }
}

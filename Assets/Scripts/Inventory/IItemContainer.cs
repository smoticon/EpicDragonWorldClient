public interface IItemContainer
{
    bool CanAddItem(Item item, int amount = 1);
    bool AddItem(Item item);

    Item RemoveItem(int itemID);
    bool RemoveItem(Item item);

    void Clear();

    int ItemCount(int itemID);
}
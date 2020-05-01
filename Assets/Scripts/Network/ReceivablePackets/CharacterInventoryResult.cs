using System.Collections;
using UnityEngine;

public class CharacterInventoryResult
{
    public static void Notify(ReceivablePacket packet)
    {
        // Read Data
        int listSize = packet.ReadInt();
        ArrayList itemList = new ArrayList(listSize);
        
        for (int i = 0; i < listSize; i++)
        {
            int itemId = packet.ReadInt();
            bool equiped = packet.ReadInt() == 1 ? true : false;
            int amount = packet.ReadInt();
            int enchant = packet.ReadInt();
            ItemInfoHolder itemData = new ItemInfoHolder(itemId, equiped, amount, enchant);
            itemList.Add(itemData);
        }

        InventoryManager.Instance.CharacterItems(itemList);
    }
}
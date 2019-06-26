using System.Collections.Generic;
using UnityEngine;

/**
 * Author: Pantelis Andrianakis
 * Date: June 22nd 2019
 */
public class ItemData : MonoBehaviour
{
    public static ItemData Instance { get; private set; }

    public GameObject[] itemPrefabs;
    private static readonly Dictionary<int, ItemHolder> ITEMS = new Dictionary<int, ItemHolder>();

    private void Start()
    {
        Instance = this;
        ITEMS.Add(1, new ItemHolder(1, ItemSlot.CHEST, ItemType.EQUIP, "Test Shirt", "Tank tops make great first items.", "MaleShirt2", "FemaleShirt2", -1, Vector3.zero, Vector3.zero, new Quaternion(), new Quaternion(), Vector3.one, Vector3.one, false, false, 0, 0, 0, 0));
        ITEMS.Add(2, new ItemHolder(2, ItemSlot.LEGS, ItemType.EQUIP, "Test Pants", "Some people call them jeans, others... just pants.", "MaleJeans", "FemalePants1", -1, Vector3.zero, Vector3.zero, new Quaternion(), new Quaternion(), Vector3.one, Vector3.one, false, false, 0, 0, 0, 0));
        ITEMS.Add(3, new ItemHolder(3, ItemSlot.FEET, ItemType.EQUIP, "Test Shoes", "Definitely not tall shoes.", "TallShoes_Black_Recipe", "FemaleTallShoes_Black", -1, Vector3.zero, Vector3.zero, new Quaternion(), new Quaternion(), Vector3.one, Vector3.one, false, false, 0, 0, 0, 0));
        ITEMS.Add(4, new ItemHolder(4, ItemSlot.RIGHT_HAND, ItemType.EQUIP, "Test Sword Right", "Sword of thunder.", "", "", 0, new Vector3(-0.09f, -0.24f, -0.09f), new Vector3(-0.09f, -0.24f, -0.09f), Quaternion.Euler(12.738f, -5.85f, -0f), Quaternion.Euler(12.738f, -5.85f, -0f), Vector3.one, Vector3.one, false, false, 0, 0, 0, 0));
        ITEMS.Add(5, new ItemHolder(5, ItemSlot.LEFT_HAND, ItemType.EQUIP, "Test Sword Left", "Sword of lightning.", "", "", 0, new Vector3(-0.1f, 0.3f, -0.102f), new Vector3(-0.1f, 0.3f, -0.102f), Quaternion.Euler(-12.738f, -5.85f, 182.189f), Quaternion.Euler(-12.738f, -5.85f, 182.189f), Vector3.one, Vector3.one, false, false, 0, 0, 0, 0));
    }

    public static ItemHolder GetItem(int id)
    {
        if (!ITEMS.ContainsKey(id))
        {
            return null;
        }
        return ITEMS[id];
    }
}

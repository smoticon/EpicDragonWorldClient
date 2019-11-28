using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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

        TextAsset data = Resources.Load("data/ItemData") as TextAsset;
        string[] lines = Regex.Split(data.text, "\r\n|\n\r|\n|\r");
        foreach (string line in lines)
        {
            if (line.StartsWith("#"))
            {
                continue;
            }
            string[] values = Regex.Split(line, ";");
            if (values.Length < 20)
            {
                continue;
            }

            int itemId = int.Parse(values[0]);
            ItemSlot itemSlot = (ItemSlot)Enum.Parse(typeof(ItemSlot), values[1]);
            ItemType itemType = (ItemType)Enum.Parse(typeof(ItemType), values[2]);
            string name = values[3];
            string description = values[4];
            string recipeMale = values[5];
            string recipeFemale = values[6];
            int prefabId = int.Parse(values[7]);
            string[] positionMaleSplit = Regex.Split(values[8], ",");
            Vector3 positionMale = new Vector3(float.Parse(positionMaleSplit[0]), float.Parse(positionMaleSplit[1]), float.Parse(positionMaleSplit[2]));
            string[] positionFemaleSplit = Regex.Split(values[9], ",");
            Vector3 positionFemale = new Vector3(float.Parse(positionFemaleSplit[0]), float.Parse(positionFemaleSplit[1]), float.Parse(positionFemaleSplit[2]));
            string[] rotationMaleSplit = Regex.Split(values[10], ",");
            Quaternion rotationMale = Quaternion.Euler(float.Parse(rotationMaleSplit[0]), float.Parse(rotationMaleSplit[1]), float.Parse(rotationMaleSplit[2]));
            string[] rotationFemaleSplit = Regex.Split(values[11], ",");
            Quaternion rotationFemale = Quaternion.Euler(float.Parse(rotationFemaleSplit[0]), float.Parse(rotationFemaleSplit[1]), float.Parse(rotationFemaleSplit[2]));
            string[] scaleMaleSplit = Regex.Split(values[12], ",");
            Vector3 scaleMale = new Vector3(float.Parse(scaleMaleSplit[0]), float.Parse(scaleMaleSplit[1]), float.Parse(scaleMaleSplit[2]));
            string[] scaleFemaleSplit = Regex.Split(values[13], ",");
            Vector3 scaleFemale = new Vector3(float.Parse(scaleFemaleSplit[0]), float.Parse(scaleFemaleSplit[1]), float.Parse(scaleFemaleSplit[2]));
            bool stackable = bool.Parse(values[14]);
            bool tradable = bool.Parse(values[15]);
            int stamina = int.Parse(values[16]);
            int strength = int.Parse(values[17]);
            int dexterity = int.Parse(values[18]);
            int intelect = int.Parse(values[19]);

            ITEMS.Add(itemId, new ItemHolder(itemId, itemSlot, itemType, name, description, recipeMale, recipeFemale, prefabId, positionMale, positionFemale, rotationMale, rotationFemale, scaleMale, scaleFemale, stackable, tradable, stamina, strength, dexterity, intelect));
        }
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

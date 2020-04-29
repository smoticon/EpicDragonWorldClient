﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
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
    public static Dictionary<int, Item> itemDB = new Dictionary<int, Item>();

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
            string[] values = line.Split(';');
            if (values.Length < 20)
            {
                continue;
            }

            int itemId = int.Parse(values[0]);
            EquipmentItemSlot itemSlot = (EquipmentItemSlot)Enum.Parse(typeof(EquipmentItemSlot), values[1]);
            ItemType itemType = (ItemType)Enum.Parse(typeof(ItemType), values[2]);
            string name = values[3];
            string description = values[4];
            string recipeMale = values[5];
            string recipeFemale = values[6];
            int prefabId = int.Parse(values[7]);
            string[] positionMaleSplit = values[8].Split(',');
            Vector3 positionMale = new Vector3(float.Parse(positionMaleSplit[0], CultureInfo.InvariantCulture), float.Parse(positionMaleSplit[1], CultureInfo.InvariantCulture), float.Parse(positionMaleSplit[2], CultureInfo.InvariantCulture));
            string[] positionFemaleSplit = values[9].Split(',');
            Vector3 positionFemale = new Vector3(float.Parse(positionFemaleSplit[0], CultureInfo.InvariantCulture), float.Parse(positionFemaleSplit[1], CultureInfo.InvariantCulture), float.Parse(positionFemaleSplit[2], CultureInfo.InvariantCulture));
            string[] rotationMaleSplit = values[10].Split(',');
            Quaternion rotationMale = Quaternion.Euler(float.Parse(rotationMaleSplit[0], CultureInfo.InvariantCulture), float.Parse(rotationMaleSplit[1], CultureInfo.InvariantCulture), float.Parse(rotationMaleSplit[2], CultureInfo.InvariantCulture));
            string[] rotationFemaleSplit = values[11].Split(',');
            Quaternion rotationFemale = Quaternion.Euler(float.Parse(rotationFemaleSplit[0], CultureInfo.InvariantCulture), float.Parse(rotationFemaleSplit[1], CultureInfo.InvariantCulture), float.Parse(rotationFemaleSplit[2], CultureInfo.InvariantCulture));
            string[] scaleMaleSplit = values[12].Split(',');
            Vector3 scaleMale = new Vector3(float.Parse(scaleMaleSplit[0], CultureInfo.InvariantCulture), float.Parse(scaleMaleSplit[1], CultureInfo.InvariantCulture), float.Parse(scaleMaleSplit[2], CultureInfo.InvariantCulture));
            string[] scaleFemaleSplit = values[13].Split(',');
            Vector3 scaleFemale = new Vector3(float.Parse(scaleFemaleSplit[0], CultureInfo.InvariantCulture), float.Parse(scaleFemaleSplit[1], CultureInfo.InvariantCulture), float.Parse(scaleFemaleSplit[2], CultureInfo.InvariantCulture));
            bool stackable = bool.Parse(values[14]);
            bool tradable = bool.Parse(values[15]);
            int stamina = int.Parse(values[16]);
            int strength = int.Parse(values[17]);
            int dexterity = int.Parse(values[18]);
            int intelect = int.Parse(values[19]);
            ITEMS.Add(itemId, new ItemHolder(itemId, itemSlot, itemType, name, description, recipeMale, recipeFemale, prefabId, positionMale, positionFemale, rotationMale, rotationFemale, scaleMale, scaleFemale, stackable, tradable, stamina, strength, dexterity, intelect));
            
            Sprite icon = Resources.Load<Sprite>("ItemIcons/Equipment/" + recipeFemale.Replace("_Recipe", ""));
            itemDB.Add(itemId, new Item(itemId, itemSlot, itemType, name, description, icon, stackable, stamina, strength, dexterity, intelect));
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

    public static Item GetItemInfo(int id)
    {
        if (!itemDB.ContainsKey(id))
        {
            return null;
        }
        return itemDB[id];
    }
}

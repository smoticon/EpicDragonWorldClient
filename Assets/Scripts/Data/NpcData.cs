using System.Collections.Generic;
using UnityEngine;

/**
 * Author: Pantelis Andrianakis
 * Date: November 28th 2019
 */
public class NpcData : MonoBehaviour
{
    public static NpcData Instance { get; private set; }

    private static readonly Dictionary<int, CharacterDataHolder> NPCS = new Dictionary<int, CharacterDataHolder>();

    private void Start()
    {
        Instance = this;

        //TODO: Create NPC costructor.
        CharacterDataHolder testNpc = new CharacterDataHolder();
        testNpc.SetName("Test NPC");
        testNpc.SetRace(1);
        testNpc.SetHeight(0.5f);
        testNpc.SetBelly(0.5f);
        testNpc.SetHairType(3);
        testNpc.SetHairColor(15150895);
        testNpc.SetSkinColor(15847869);
        testNpc.SetEyeColor(15150895);
        NPCS.Add(1, testNpc);
    }

    public static CharacterDataHolder GetNpc(int id)
    {
        if (!NPCS.ContainsKey(id))
        {
            return null;
        }
        return NPCS[id];
    }
}

using System.Collections;
using System.Collections.Generic;
using UMA.CharacterSystem;
using UnityEngine;

/**
 * Author: Pantelis Andrianakis
 * Date: December 27th 2018
 */
public class CharacterManager : MonoBehaviour
{
    public static CharacterManager Instance { get; private set; }

    public DynamicCharacterAvatar avatar;

    public List<string> hairModelsMale = new List<string>();
    public List<string> hairModelsFemale = new List<string>();

    void Start()
    {
        Instance = this;
    }

    public DynamicCharacterAvatar CreateCharacter(CharacterDataHolder characterData)
    {
        return CreateCharacter(characterData, characterData.GetX(), characterData.GetY(), characterData.GetZ(), characterData.GetHeading());
    }

    public DynamicCharacterAvatar CreateCharacter(CharacterDataHolder characterData, float posX, float posY, float posZ, float heading)
    {
        DynamicCharacterAvatar newAvatar = CreateCharacter(characterData, posX, posY, posZ);

        // Rotation.
        Quaternion curHeading = newAvatar.gameObject.transform.localRotation;
        Vector3 curvAngle = curHeading.eulerAngles;
        curvAngle.y = heading;
        curHeading.eulerAngles = curvAngle;
        newAvatar.gameObject.transform.localRotation = curHeading;

        // Return the new DynamicCharacterAvatar.
        return newAvatar;
    }

    public DynamicCharacterAvatar CreateCharacter(CharacterDataHolder characterData, float posX, float posY, float posZ)
    {
        Vector3 position = new Vector3(posX, posY, posZ);
        DynamicCharacterAvatar newAvatar = Instantiate(avatar, position, Quaternion.identity) as DynamicCharacterAvatar;
        newAvatar.BuildCharacter(); // Prevent UMA bone error.

        // Add a new Capsule Collider to prevent falling.
        CapsuleCollider capsuleCollider = newAvatar.gameObject.AddComponent<CapsuleCollider>();
        capsuleCollider.radius = 0.15f;
        capsuleCollider.height = 1.71f;
        capsuleCollider.center = new Vector3(0, 0.82f, 0);

        // Customize character.
        StartCoroutine(CustomizeCharacterAppearance(characterData, newAvatar));

        // Add AudioSource.
        newAvatar.gameObject.AddComponent<AudioSource>();

        // Return GameObject.
        return newAvatar;
    }

    public IEnumerator CustomizeCharacterAppearance(CharacterDataHolder characterData, DynamicCharacterAvatar newAvatar)
    {
        // Unfortunately UMA needs a small delay to initialize.
        yield return new WaitForSeconds(0.1f);

        // Customize character.
        int hairType = characterData.GetHairType();
        if (characterData.GetRace() == 0)
        {
            newAvatar.ChangeRace("HumanMaleDCS");
            if (hairType != 0)
            {
                newAvatar.SetSlot("Hair", hairModelsMale[characterData.GetHairType()]);
            }
        }
        if (characterData.GetRace() == 1)
        {
            newAvatar.ChangeRace("HumanFemaleDCS");
            if (hairType != 0)
            {
                newAvatar.SetSlot("Hair", hairModelsFemale[characterData.GetHairType()]);
            }
        }

        // Set Colors
        newAvatar.SetColor("Hair", Util.IntToColor(characterData.GetHairColor()));
        newAvatar.SetColor("Skin", Util.IntToColor(characterData.GetSkinColor()));
        newAvatar.SetColor("Eyes", Util.IntToColor(characterData.GetEyeColor()));
        newAvatar.UpdateColors(true);

        Dictionary<string, DnaSetter> dna = newAvatar.GetDNA();
        dna["height"].Set(characterData.GetHeight());
        dna["belly"].Set(characterData.GetBelly());
        newAvatar.BuildCharacter();
    }
}

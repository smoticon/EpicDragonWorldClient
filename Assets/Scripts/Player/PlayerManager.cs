using System;
using System.Collections;
using UnityEngine;

/**
 * @author Pantelis Andrianakis
 */
public class PlayerManager : MonoBehaviour
{
    // Player manager instance.
    public static PlayerManager instance;

    [HideInInspector]
    public String accountName;
    [HideInInspector]
    public ArrayList characterList;
    [HideInInspector]
    public CharacterDataHolder selectedCharacterData;

    void Start()
    {
        instance = this;
    }

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
}

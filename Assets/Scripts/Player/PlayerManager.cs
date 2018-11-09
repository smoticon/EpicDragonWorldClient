using System;
using System.Collections;
using UnityEngine;

/**
 * Author: Pantelis Andrianakis
 * Date: February 25th 2018
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
    [HideInInspector]
    public long selectedCharacterObjectId = 0;

    void Start()
    {
        instance = this;
    }

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
}

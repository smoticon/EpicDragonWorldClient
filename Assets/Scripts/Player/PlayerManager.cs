/*
 * This file is part of the Epic Dragon World project.
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
 * General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see <http://www.gnu.org/licenses/>.
 */
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

    void Start ()
    {
        instance = this;
    }

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
}

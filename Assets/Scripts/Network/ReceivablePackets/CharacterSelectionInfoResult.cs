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
using System.Collections;

/**
* @author Pantelis Andrianakis
*/
public class CharacterSelectionInfoResult
{
    public static void notify(ReceivablePacket packet)
    {
        // Get player list size.
        int listSize = packet.ReadByte();

        // Read the data.
        ArrayList characterList = new ArrayList(listSize);
        for (int i = 0; i < listSize; i++)
        {
            CharacterDataHolder characterData = new CharacterDataHolder();
            characterData.SetName(packet.ReadString());
            characterData.SetSlot((byte)packet.ReadByte());
            characterData.SetSelected(packet.ReadByte() == 1 ? true : false);
            characterData.SetClassId((byte)packet.ReadByte());
            characterData.SetLocationName(packet.ReadString());
            characterData.SetX(packet.ReadDouble());
            characterData.SetY(packet.ReadDouble());
            characterData.SetZ(packet.ReadDouble());
            characterData.SetHeading(packet.ReadInt());
            characterData.SetExperience(packet.ReadLong());
            characterData.SetHp(packet.ReadLong());
            characterData.SetMp(packet.ReadLong());
            characterData.SetAccessLevel((byte)packet.ReadByte());
            characterData.SetItemHead(packet.ReadInt());
            characterData.SetItemChest(packet.ReadInt());
            characterData.SetItemGloves(packet.ReadInt());
            characterData.SetItemLegs(packet.ReadInt());
            characterData.SetItemBoots(packet.ReadInt());
            characterData.SetItemRightHand(packet.ReadInt());
            characterData.SetItemLeftHand(packet.ReadInt());
            characterList.Add(characterData);
        }

        // Send the data.
        PlayerManager.instance.characterList = characterList;

        // Enable player selection.
        CharacterSelectionManager.instance.waitingServer = false;
    }
}

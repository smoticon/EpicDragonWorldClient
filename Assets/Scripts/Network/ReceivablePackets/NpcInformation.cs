/**
 * Author: Pantelis Andrianakis
 * Date: November 28th 2019
 */
public class NpcInformation
{
    public static void Notify(ReceivablePacket packet)
    {
        long objectId = packet.ReadLong();
        CharacterDataHolder npcTemplate = NpcData.GetNpc(packet.ReadInt());
        CharacterDataHolder characterData = new CharacterDataHolder();
        characterData.SetName(npcTemplate.GetName());
        characterData.SetRace(npcTemplate.GetRace());
        characterData.SetHeight(npcTemplate.GetHeight());
        characterData.SetBelly(npcTemplate.GetBelly()); ;
        characterData.SetHairType(npcTemplate.GetHairType());
        characterData.SetHairColor(npcTemplate.GetHairColor());
        characterData.SetSkinColor(npcTemplate.GetSkinColor());
        characterData.SetEyeColor(npcTemplate.GetEyeColor());
        characterData.SetHeadItem(npcTemplate.GetHeadItem());
        characterData.SetChestItem(npcTemplate.GetChestItem());
        characterData.SetLegsItem(npcTemplate.GetLegsItem());
        characterData.SetHandsItem(npcTemplate.GetHandsItem());
        characterData.SetFeetItem(npcTemplate.GetFeetItem());
        characterData.SetLeftHandItem(npcTemplate.GetLeftHandItem());
        characterData.SetRightHandItem(npcTemplate.GetRightHandItem());
        characterData.SetX(packet.ReadFloat());
        characterData.SetY(packet.ReadFloat());
        characterData.SetZ(packet.ReadFloat());
        characterData.SetHeading(packet.ReadFloat());
        characterData.SetHp(packet.ReadLong());

        WorldManager.Instance.UpdateObject(objectId, characterData);
    }
}

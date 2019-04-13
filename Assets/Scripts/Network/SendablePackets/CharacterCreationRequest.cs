/**
 * Author: Pantelis Andrianakis
 * Date: December 29th 2017
 */
public class CharacterCreationRequest : SendablePacket
{
    public CharacterCreationRequest(CharacterDataHolder dataHolder)
    {
        WriteShort(3); // Packet id.
        WriteString(dataHolder.GetName());
        WriteByte(dataHolder.GetRace());
        WriteFloat(dataHolder.GetHeight());
        WriteFloat(dataHolder.GetBelly());
        WriteByte(dataHolder.GetHairType());
        WriteInt(dataHolder.GetHairColor());
        WriteInt(dataHolder.GetSkinColor());
        WriteInt(dataHolder.GetEyeColor());
    }
}

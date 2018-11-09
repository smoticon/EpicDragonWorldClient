/**
 * Author: Pantelis Andrianakis
 * Date: December 31st 2017
 */
public class CharacterSlotUpdate : SendablePacket
{
    public CharacterSlotUpdate(int oldSlotId, int newSlotId)
    {
        WriteShort(5); // Packet id.
        WriteByte(oldSlotId);
        WriteByte(newSlotId);
    }
}

/**
 * Author: Pantelis Andrianakis
 * Date: December 31st 2017
 */
public class CharacterSelectUpdate : SendablePacket
{
    public CharacterSelectUpdate(int slot)
    {
        WriteShort(6); // Packet id.
        WriteByte(slot);
    }
}

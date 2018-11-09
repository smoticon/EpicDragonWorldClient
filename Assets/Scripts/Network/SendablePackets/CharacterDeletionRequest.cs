/**
 * Author: Pantelis Andrianakis
 * Date: December 31st 2017
 */
public class CharacterDeletionRequest : SendablePacket
{
    public CharacterDeletionRequest(int slot)
    {
        WriteShort(4); // Packet id.
        WriteByte(slot);
    }
}

/**
* @author Pantelis Andrianakis
*/
public class CharacterSelectUpdate : SendablePacket
{
    public CharacterSelectUpdate(int slot)
    {
        WriteShort(6); // Packet id.
        WriteByte(slot);
    }
}

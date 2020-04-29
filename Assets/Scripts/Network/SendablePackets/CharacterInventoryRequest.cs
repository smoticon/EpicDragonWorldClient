public class CharacterInventoryRequest : SendablePacket
{
    public CharacterInventoryRequest(string player)
    {
        WriteShort(15); // Packet Id
        WriteString(player);
    }
}

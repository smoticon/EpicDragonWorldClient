/**
 * @author Pantelis Andrianakis
 */
public class CharacterCreationRequest : SendablePacket
{
    public CharacterCreationRequest(string name, int classId)
    {
        WriteShort(3); // Packet id.
        WriteString(name);
        WriteByte(classId);
    }
}

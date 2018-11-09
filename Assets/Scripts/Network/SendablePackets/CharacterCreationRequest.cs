/**
 * Author: Pantelis Andrianakis
 * Date: December 29th 2017
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

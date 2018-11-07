/**
 * @author Pantelis Andrianakis
 */
public class ObjectInfoRequest : SendablePacket
{
    public ObjectInfoRequest(long objectId)
    {
        WriteShort(9); // Packet id.
        WriteLong(objectId);
    }
}

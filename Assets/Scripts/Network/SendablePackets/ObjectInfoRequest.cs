/**
* @author Pantelis Andrianakis
*/
public class ObjectInfoRequest : SendablePacket
{
    public ObjectInfoRequest(int objectId)
    {
        WriteShort(9); // Packet id.
        WriteInt(objectId);
    }
}

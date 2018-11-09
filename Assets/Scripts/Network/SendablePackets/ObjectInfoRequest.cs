/**
 * Author: Pantelis Andrianakis
 * Date: June 11th 2018
 */
public class ObjectInfoRequest : SendablePacket
{
    public ObjectInfoRequest(long objectId)
    {
        WriteShort(9); // Packet id.
        WriteLong(objectId);
    }
}

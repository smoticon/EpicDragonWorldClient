/**
 * Author: Pantelis Andrianakis
 * Date: June 11th 2018
 */
public class ObjectInfoRequest : SendablePacket
{
    public ObjectInfoRequest(long objectId)
    {
        WriteShort(11); // Packet id.
        WriteLong(objectId);
    }
}

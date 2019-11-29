/**
 * Author: Pantelis Andrianakis
 * Date: November 29th 2019
 */
public class TargetUpdateRequest : SendablePacket
{
    public TargetUpdateRequest(long objectId)
    {
        WriteShort(14); // Packet id.
        WriteLong(objectId);
    }
}

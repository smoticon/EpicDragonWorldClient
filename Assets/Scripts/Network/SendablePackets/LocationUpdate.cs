/**
* @author Pantelis Andrianakis
*/
public class LocationUpdate : SendablePacket
{
    public LocationUpdate(float posX, float posY, float posZ)
    {
        WriteShort(8); // Packet id.
        WriteDouble(posX); // TODO: WriteFloat
        WriteDouble(posY); // TODO: WriteFloat
        WriteDouble(posZ); // TODO: WriteFloat
    }
}

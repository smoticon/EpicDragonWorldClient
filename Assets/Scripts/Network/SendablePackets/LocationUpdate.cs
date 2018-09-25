/**
* @author Pantelis Andrianakis
*/
public class LocationUpdate : SendablePacket
{
    public LocationUpdate(float posX, float posY, float posZ, float angleY, int animState, bool isWater)
    {
        WriteShort(8); // Packet id.
        WriteDouble(posX); // TODO: WriteFloat
        WriteDouble(posY); // TODO: WriteFloat
        WriteDouble(posZ); // TODO: WriteFloat
        WriteDouble(angleY); // TODO: WriteFloat
        WriteShort(animState);
        if(isWater)
        {
            WriteShort(1);
        }
        else
        {
            WriteShort(0);
        }
    }
}

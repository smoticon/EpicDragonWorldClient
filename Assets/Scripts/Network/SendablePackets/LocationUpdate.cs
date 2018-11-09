/**
 * Author: Pantelis Andrianakis
 * Date: June 11th 2018
 */
public class LocationUpdate : SendablePacket
{
    public LocationUpdate(float posX, float posY, float posZ, float heading, int animState, bool isInsideWater)
    {
        WriteShort(8); // Packet id.
        WriteDouble(posX); // TODO: WriteFloat
        WriteDouble(posY); // TODO: WriteFloat
        WriteDouble(posZ); // TODO: WriteFloat
        WriteDouble(heading); // TODO: WriteFloat
        WriteShort(animState);
        WriteByte(isInsideWater ? 1 : 0);
    }
}

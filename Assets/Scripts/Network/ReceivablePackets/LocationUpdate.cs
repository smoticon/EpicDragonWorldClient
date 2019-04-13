/**
 * Author: Pantelis Andrianakis
 * Date: June 11th 2018
 */
public class LocationUpdate
{
    public static void Notify(ReceivablePacket packet)
    {
        long objectId = packet.ReadLong();
        float posX = packet.ReadFloat();
        float posY = packet.ReadFloat();
        float posZ = packet.ReadFloat();
        float heading = packet.ReadFloat();

        WorldManager.Instance.MoveObject(objectId, posX, posY, posZ, heading);
    }
}

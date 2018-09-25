/**
* @author Pantelis Andrianakis
*/
public class MoveToLocation
{
    public static void notify(ReceivablePacket packet)
    {
        long objectId = packet.ReadLong();
        float posX = packet.ReadFloat();
        float posY = packet.ReadFloat();
        float posZ = packet.ReadFloat();
        float angleY = packet.ReadFloat();
        int animState = packet.ReadShort();
        int waterState = packet.ReadShort();
        WorldManager.instance.MoveObject(objectId, posX, posY, posZ, angleY, animState, waterState);
    }
}

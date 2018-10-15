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
        float heading = packet.ReadFloat();
        int animState = packet.ReadShort();
        int waterState = packet.ReadByte();
        WorldManager.instance.MoveObject(objectId, posX, posY, posZ, heading, animState, waterState);
    }
}

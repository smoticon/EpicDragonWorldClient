/**
 * @author Pantelis Andrianakis
 */
public class PlayerInformation
{
    public static void notify(ReceivablePacket packet)
    {
        long objectId = packet.ReadLong();
        int classId = packet.ReadShort();
        string name = packet.ReadString();
        float posX = packet.ReadFloat();
        float posY = packet.ReadFloat();
        float posZ = packet.ReadFloat();
        float heading = packet.ReadFloat();
        //TODO: Manage PlayerInformation

        WorldManager.instance.UpdateObject(objectId, classId, name, posX, posY, posZ, heading);
    }
}

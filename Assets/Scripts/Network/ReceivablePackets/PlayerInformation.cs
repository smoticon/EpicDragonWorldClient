/**
* @author Pantelis Andrianakis
*/
public class PlayerInformation
{
    public static void notify(ReceivablePacket packet)
    {
        long objectId = packet.ReadLong();
        int classId = packet.ReadShort();
        string playerName = packet.ReadString();
        float posX = packet.ReadFloat();
        float posY = packet.ReadFloat();
        float posZ = packet.ReadFloat();
        float heading = packet.ReadFloat();
        //TODO: Manage PlayerInformation

        WorldManager.instance.UpdateObject(objectId, classId, posX, posY, posZ, heading);
    }
}

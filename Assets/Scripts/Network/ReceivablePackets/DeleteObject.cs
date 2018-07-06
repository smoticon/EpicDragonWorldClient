/**
* @author Pantelis Andrianakis
*/
public class DeleteObject
{
    public static void notify(ReceivablePacket packet)
    {
        int objectId = packet.ReadInt();
        WorldManager.instance.DeleteObject(objectId);
    }
}

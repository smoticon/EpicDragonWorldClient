/**
 * @author Pantelis Andrianakis
 */
public class DeleteObject
{
    public static void notify(ReceivablePacket packet)
    {
        long objectId = packet.ReadLong();
        WorldManager.instance.DeleteObject(objectId);
    }
}

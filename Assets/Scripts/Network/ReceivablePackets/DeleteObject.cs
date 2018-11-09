/**
 * Author: Pantelis Andrianakis
 * Date: June 10th 2018
 */
public class DeleteObject
{
    public static void notify(ReceivablePacket packet)
    {
        long objectId = packet.ReadLong();
        WorldManager.instance.DeleteObject(objectId);
    }
}

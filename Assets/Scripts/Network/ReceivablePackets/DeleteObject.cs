/**
 * Author: Pantelis Andrianakis
 * Date: June 10th 2018
 */
public class DeleteObject
{
    public static void Notify(ReceivablePacket packet)
    {
        long objectId = packet.ReadLong();
        WorldManager.Instance.DeleteObject(objectId);
    }
}

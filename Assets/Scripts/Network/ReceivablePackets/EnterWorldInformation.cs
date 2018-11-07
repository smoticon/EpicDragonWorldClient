/**
 * @author Pantelis Andrianakis
 */
public class EnterWorldInformation
{
    public static void notify(ReceivablePacket packet)
    {
        PlayerManager.instance.selectedCharacterObjectId = packet.ReadLong();
    }
}

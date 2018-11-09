/**
 * Author: Pantelis Andrianakis
 * Date: May 18th 2018
 */
public class EnterWorldInformation
{
    public static void notify(ReceivablePacket packet)
    {
        PlayerManager.instance.selectedCharacterObjectId = packet.ReadLong();
    }
}

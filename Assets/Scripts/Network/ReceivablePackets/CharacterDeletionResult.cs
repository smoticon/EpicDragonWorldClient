/**
 * Author: Pantelis Andrianakis
 * Date: December 31st 2017
 */
public class CharacterDeletionResult
{
    public static void notify(ReceivablePacket packet)
    {
        CharacterSelectionManager.instance.waitingServer = false;
    }
}

/**
 * @author Pantelis Andrianakis
 */
public class CharacterDeletionResult
{
    public static void notify(ReceivablePacket packet)
    {
        CharacterSelectionManager.instance.waitingServer = false;
    }
}

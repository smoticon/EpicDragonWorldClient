/**
* @author Pantelis Andrianakis
*/
public class RecievablePacketManager
{
    public static void handle(ReceivablePacket packet)
    {
        switch (packet.ReadShort())
        {
            case 1:
                AccountAuthenticationResult.notify(packet);
                break;

            case 2:
                CharacterSelectionInfoResult.notify(packet);
                break;

            case 3:
                CharacterCreationResult.notify(packet);
                break;

            case 4:
                CharacterDeletionResult.notify(packet);
                break;

            case 5:
                EnterWorldInformation.notify(packet);
                break;

            case 6:
                PlayerInformation.notify(packet);
                break;

            case 7:
                DeleteObject.notify(packet);
                break;

            case 8:
                Logout.notify(packet);
                break;

            case 9:
                MoveToLocation.notify(packet);
                break;

            case 10:
                ChatResult.notify(packet);
                break;
        }
    }
}

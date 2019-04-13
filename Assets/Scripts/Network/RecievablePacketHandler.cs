/**
 * Author: Pantelis Andrianakis
 * Date: December 26th 2017
 */
public class RecievablePacketHandler
{
    public static void Handle(ReceivablePacket packet)
    {
        switch (packet.ReadShort())
        {
            case 1:
                AccountAuthenticationResult.Notify(packet);
                break;

            case 2:
                CharacterSelectionInfoResult.Notify(packet);
                break;

            case 3:
                CharacterCreationResult.Notify(packet);
                break;

            case 4:
                CharacterDeletionResult.Notify(packet);
                break;

            case 5:
                PlayerOptionsInformation.Notify(packet);
                break;

            case 6:
                PlayerInformation.Notify(packet);
                break;

            case 7:
                DeleteObject.Notify(packet);
                break;

            case 8:
                Logout.Notify(packet);
                break;

            case 9:
                LocationUpdate.Notify(packet);
                break;

            case 10:
                AnimatorUpdate.Notify(packet);
                break;

            case 11:
                ChatResult.Notify(packet);
                break;
        }
    }
}

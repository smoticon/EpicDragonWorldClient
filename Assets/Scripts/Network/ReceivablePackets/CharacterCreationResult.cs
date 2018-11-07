/**
 * @author Pantelis Andrianakis
 */
public class CharacterCreationResult
{
    public static void notify(ReceivablePacket packet)
    {
        CharacterCreationManager.instance.creationResult = packet.ReadByte();
    }
}

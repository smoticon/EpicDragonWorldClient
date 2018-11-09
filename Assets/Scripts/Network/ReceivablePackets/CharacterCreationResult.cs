/**
 * Author: Pantelis Andrianakis
 * Date: December 26th 2017
 */
public class CharacterCreationResult
{
    public static void notify(ReceivablePacket packet)
    {
        CharacterCreationManager.instance.creationResult = packet.ReadByte();
    }
}

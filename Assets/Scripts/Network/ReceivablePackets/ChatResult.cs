/**
* @author Pantelis Andrianakis
*/
public class ChatResult
{
    public static void notify(ReceivablePacket packet)
    {
        // Read the data.
        int chatType = packet.ReadByte(); // 0 system, 1 normal chat, 2 personal message
        string senderName = packet.ReadString();
        string message = packet.ReadString();

        // TODO: ChatManager.manageChat(chatType, senderName, message);
    }
}

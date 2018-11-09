/**
 * Author: Pantelis Andrianakis
 * Date: August 19th 2018
 */
public class ChatResult
{
    public static void notify(ReceivablePacket packet)
    {
        // Read the data.
        int chatType = packet.ReadByte(); // 0 system, 1 normal chat, 2 personal message
        string senderName = packet.ReadString();
        string message = packet.ReadString();

        // Send the message.
        // TODO: Setting to show time.
        // ChatBoxManager.instance.SendMessageToChat(DateTime.Now.ToString("HH:mm:ss tt") + " " + senderName + ": " + message, chatType);
        ChatBoxManager.instance.SendMessageToChat(senderName + ": " + message, chatType);
    }
}

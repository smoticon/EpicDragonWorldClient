/**
 * Author: Pantelis Andrianakis
 * Date: August 19th 2018
 */
public class ChatRequest : SendablePacket
{
    public ChatRequest(string message)
    {
        WriteShort(10); // Packet id.
        WriteString(message);
    }
}

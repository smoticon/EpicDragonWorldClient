/**
 * @author Pantelis Andrianakis
 */
public class ChatRequest : SendablePacket
{
    public ChatRequest(string message)
    {
        WriteShort(10); // Packet id.
        WriteString(message);
    }
}

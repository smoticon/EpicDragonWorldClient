/**
 * Author: Pantelis Andrianakis
 * Date: February 3rd 2019
 */
public class ExitWorldRequest : SendablePacket
{
    public ExitWorldRequest()
    {
        WriteShort(8); // Packet id.
    }
}

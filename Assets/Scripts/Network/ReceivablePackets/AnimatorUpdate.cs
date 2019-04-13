/**
 * Author: Pantelis Andrianakis
 * Date: June 11th 2018
 */
public class AnimatorUpdate
{
    public static void Notify(ReceivablePacket packet)
    {
        // Read data.
        long objectId = packet.ReadLong();
        float velocityX = packet.ReadFloat();
        float velocityZ = packet.ReadFloat();
        bool triggerJump = packet.ReadByte() == 1;
        bool isInWater = packet.ReadByte() == 1;
        bool isGrounded = packet.ReadByte() == 1;

        WorldManager.Instance.AnimateObject(objectId, velocityX, velocityZ, triggerJump, isInWater, isGrounded);
    }
}

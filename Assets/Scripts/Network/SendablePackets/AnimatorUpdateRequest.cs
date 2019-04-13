/**
 * Author: Pantelis Andrianakis
 * Date: February 4th 2019
 */
public class AnimatorUpdateRequest : SendablePacket
{
    public AnimatorUpdateRequest(float velocityX, float velocityZ, bool triggerJump, bool isInWater, bool isGrounded)
    {
        WriteShort(10); // Packet id.
        WriteFloat(velocityX);
        WriteFloat(velocityZ);
        WriteByte(triggerJump ? 1 : 0);
        WriteByte(isInWater ? 1 : 0);
        WriteByte(isGrounded ? 1 : 0);
    }
}

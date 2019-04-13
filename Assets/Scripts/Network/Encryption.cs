using System.Security.Cryptography;

/**
 * AES Rijndael encryption.
 * Author: Pantelis Andrianakis
 * Date: December 23rd 2017
 */
class Encryption
{
    public static byte[] Encrypt(byte[] bytes)
    {
        RijndaelManaged cipher = new RijndaelManaged();
        return cipher.CreateEncryptor(EncryptionConfigurations.SECRET_KEYWORD, EncryptionConfigurations.PRIVATE_PASSWORD).TransformFinalBlock(bytes, 0, bytes.Length);
    }

    public static byte[] Decrypt(byte[] bytes)
    {
        RijndaelManaged cipher = new RijndaelManaged();
        return cipher.CreateDecryptor(EncryptionConfigurations.SECRET_KEYWORD, EncryptionConfigurations.PRIVATE_PASSWORD).TransformFinalBlock(bytes, 0, bytes.Length);
    }
}
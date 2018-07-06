using System.Security.Cryptography;

/**
* @author Pantelis Andrianakis
*/
public class SHA256Generator
{
    public static string Calculate(string input)
    {
        // Calculate SHA256 hash from input.
        SHA256 sha256 = SHA256Managed.Create();
        byte[] hash = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(input));

        // Convert byte array to hex string.
        string result = "";
        foreach (byte b in hash)
        {
            result += b.ToString("x2");
        }

        // Return the result.
        return result;
    }
}

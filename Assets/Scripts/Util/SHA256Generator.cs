using System.Security.Cryptography;
using System.Text;

/**
 * @author Pantelis Andrianakis
 */
public class SHA256Generator
{
    public static string Calculate(string input)
    {
        // Calculate SHA256 hash from input.
        SHA256 sha256 = SHA256.Create();
        byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));

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

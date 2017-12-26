/*
 * This file is part of the Epic Dragon World project.
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
 * General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see <http://www.gnu.org/licenses/>.
 */
using System.Text;
using System.Security.Cryptography;

/**
 * AES Rijndael encryption.
 * @author Pantelis Andrianakis
 */
public class Encryption
{
    // Secret keyword.
    private static readonly string PASSWORD = "SECRET_KEYWORD";
    // 16-byte private password.
    private static readonly byte[] IV = Encoding.UTF8.GetBytes("0123456789012345");

    private static readonly byte[] key = new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(PASSWORD));
    private static readonly RijndaelManaged cipher = new RijndaelManaged();

    public static byte[] Encrypt(byte[] bytes)
    {
        return cipher.CreateEncryptor(key, IV).TransformFinalBlock(bytes, 0, bytes.Length);
    }

    public static byte[] Decrypt(byte[] bytes)
    {
        return cipher.CreateDecryptor(key, IV).TransformFinalBlock(bytes, 0, bytes.Length);
    }
}
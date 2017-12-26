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
using System.Security.Cryptography;

/**
* @author Pantelis Andrianakis
*/
public class MD5Generator
{
    public static string Calculate(string input)
    {
        // Calculate MD5 hash from input.
        MD5 md5 = MD5.Create();
        byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
        byte[] hash = md5.ComputeHash(inputBytes);

        // Convert byte array to hex string.
        string result = "";
        for (int i = 0; i < hash.Length; i++)
        {
            result += hash[i].ToString("X2");
        }

        // Return the result.
        return result;
    }
}

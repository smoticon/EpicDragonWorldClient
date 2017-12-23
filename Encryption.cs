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
using Simias.Encryption;
using System.Text;

/**
 * @author Pantelis Andrianakis
 */
public class Encryption
{
    private static byte[] secretKey = Encoding.UTF8.GetBytes("SECRET_KEYWORD");

    public static byte[] encrypt(byte[] data)
    {
        Blowfish algorithm = new Blowfish(secretKey);
        algorithm.Encipher(data, data.Length);
        return data;
    }

    public static byte[] decrypt(byte[] data)
    {
        Blowfish algorithm = new Blowfish(secretKey);
        algorithm.Decipher(data, data.Length);
        return data;
    }
}

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
using System;
using System.IO;
using System.Text;

/**
 * @author Pantelis Andrianakis
 */
public class ReceivablePacket
{
    private MemoryStream memoryStream;

    public ReceivablePacket(byte[] bytes)
    {
        memoryStream = new MemoryStream(bytes);
    }

    public string ReadString()
    {
        return Encoding.UTF8.GetString(ReadBytes(memoryStream.ReadByte()));
    }

    public byte[] ReadBytes(int length)
    {
        byte[] result = new byte[length];
        for (int i = 0; i < length; i++)
        {
            result[i] = (byte)memoryStream.ReadByte();
        }
        return result;
    }

    public int ReadByte()
    {
        return memoryStream.ReadByte();
    }

    public int ReadShort()
    {
        byte[] byteArray = new byte[2];
        byteArray[0] = (byte)memoryStream.ReadByte();
        byteArray[1] = (byte)memoryStream.ReadByte();
        return BitConverter.ToInt16(byteArray, 0);
    }

    public int ReadInt()
    {
        byte[] byteArray = new byte[4];
        byteArray[0] = (byte)memoryStream.ReadByte();
        byteArray[1] = (byte)memoryStream.ReadByte();
        byteArray[2] = (byte)memoryStream.ReadByte();
        byteArray[3] = (byte)memoryStream.ReadByte();
        return BitConverter.ToInt32(byteArray, 0);
    }

    public long ReadLong()
    {
        byte[] byteArray = new byte[8];
        byteArray[0] = (byte)memoryStream.ReadByte();
        byteArray[1] = (byte)memoryStream.ReadByte();
        byteArray[2] = (byte)memoryStream.ReadByte();
        byteArray[3] = (byte)memoryStream.ReadByte();
        byteArray[4] = (byte)memoryStream.ReadByte();
        byteArray[5] = (byte)memoryStream.ReadByte();
        byteArray[6] = (byte)memoryStream.ReadByte();
        byteArray[7] = (byte)memoryStream.ReadByte();
        return BitConverter.ToInt64(byteArray, 0);
    }

    public float ReadFloat()
    {
        byte[] byteArray = new byte[4];
        byteArray[0] = (byte)memoryStream.ReadByte();
        byteArray[1] = (byte)memoryStream.ReadByte();
        byteArray[2] = (byte)memoryStream.ReadByte();
        byteArray[3] = (byte)memoryStream.ReadByte();
        return BitConverter.ToSingle(byteArray, 0);
    }

    public double ReadDouble()
    {
        byte[] byteArray = new byte[8];
        byteArray[0] = (byte)memoryStream.ReadByte();
        byteArray[1] = (byte)memoryStream.ReadByte();
        byteArray[2] = (byte)memoryStream.ReadByte();
        byteArray[3] = (byte)memoryStream.ReadByte();
        byteArray[4] = (byte)memoryStream.ReadByte();
        byteArray[5] = (byte)memoryStream.ReadByte();
        byteArray[6] = (byte)memoryStream.ReadByte();
        byteArray[7] = (byte)memoryStream.ReadByte();
        return BitConverter.ToDouble(byteArray, 0);
    }
}

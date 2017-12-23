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
public class SendablePacket
{
    private MemoryStream memoryStream;

    public SendablePacket()
    {
        memoryStream = new MemoryStream();
    }

    public void WriteString(String value)
    {
        if (value != null)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(value);
            WriteByte(byteArray.Length);
            WriteBytes(byteArray);
        }
        else
        {
            memoryStream.WriteByte(0);
        }
    }

    public void WriteBytes(byte[] byteArray)
    {
        for (int i = 0; i < byteArray.Length; i++)
        {
            memoryStream.WriteByte(byteArray[i]);
        }
    }

    public void WriteByte(int value)
    {
        memoryStream.WriteByte((byte)value);
    }

    public void WriteShort(int value)
    {
        memoryStream.WriteByte((byte)value);
        memoryStream.WriteByte((byte)(value >> 8));
    }

    public void WriteInt(int value)
    {
        memoryStream.WriteByte((byte)value);
        memoryStream.WriteByte((byte)(value >> 8));
        memoryStream.WriteByte((byte)(value >> 16));
        memoryStream.WriteByte((byte)(value >> 24));
    }

    public void WriteLong(long value)
    {
        memoryStream.WriteByte((byte)value);
        memoryStream.WriteByte((byte)(value >> 8));
        memoryStream.WriteByte((byte)(value >> 16));
        memoryStream.WriteByte((byte)(value >> 24));
        memoryStream.WriteByte((byte)(value >> 32));
        memoryStream.WriteByte((byte)(value >> 40));
        memoryStream.WriteByte((byte)(value >> 48));
        memoryStream.WriteByte((byte)(value >> 56));
    }

    public void WriteDouble(double dvalue)
    {
        long value = BitConverter.DoubleToInt64Bits(dvalue);
        memoryStream.WriteByte((byte)value);
        memoryStream.WriteByte((byte)(value >> 8));
        memoryStream.WriteByte((byte)(value >> 16));
        memoryStream.WriteByte((byte)(value >> 24));
        memoryStream.WriteByte((byte)(value >> 32));
        memoryStream.WriteByte((byte)(value >> 40));
        memoryStream.WriteByte((byte)(value >> 48));
        memoryStream.WriteByte((byte)(value >> 56));
    }

    public byte[] GetSendableBytes()
    {
        return memoryStream.ToArray();
    }
}

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
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

/**
 * @author Pantelis Andrianakis
 */
public class NetworkManager : MonoBehaviour
{
    // Network manager instance.
    public static NetworkManager instance;

    // Connection settings.
    string serverIP = "127.0.0.1";
    int serverPort = 5055;
    int connectionTimeOut = 5000;

    // For socket read.
    Thread readThread;
    bool readThreadStarted = false;

    // For socket write.
    Socket socket;
    bool socketConnected = false;

    private void Start()
    {
        instance = this;
    }

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    private void OnApplicationQuit()
    {
        DisconnectFromServer();
    }

    public void DisconnectFromServer()
    {
        if (socket != null && socket.Connected)
        {
            socket.Close();
        }
        socketConnected = false;
        readThreadStarted = false;
    }

    // Best to call this only once per login attempt.
    public bool ConnectToServer()
    {
        if (socketConnected = false || socket == null || !socket.Connected)
        {
            ConnectSocket();
        }
        return SocketConnected();
    }

    private void ConnectSocket()
    {
        try
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IAsyncResult result = socket.BeginConnect(serverIP, serverPort, null, null);
            bool success = result.AsyncWaitHandle.WaitOne(connectionTimeOut, true);

            if (!success)
            {
                socketConnected = false;
                socket.Close();
            }
            else
            {
                if (socket.Connected)
                {
                    socketConnected = true;
                    // Start Receive thread.
                    readThreadStarted = true;
                    readThread = new Thread(new ThreadStart(ChannelRead));
                    readThread.Start();
                }
                else
                {
                    socketConnected = false;
                    socket.Close();
                }
            }
        }
        catch (SocketException se)
        {
            socketConnected = false;
            readThreadStarted = false;
        }
    }

    private void ChannelRead()
    {
        byte[] bufferLength = new byte[2]; // We use 2 bytes for short value.
        byte[] bufferData;
        short length; // Since we use short value, max length should be 32767.

        while (readThreadStarted)
        {
            if (socket.Receive(bufferLength) > 0)
            {
                // Get packet data length.
                length = BitConverter.ToInt16(bufferLength, 0);
                bufferData = new byte[length];

                // Get packet data.
                socket.Receive(bufferData);

                // Handle packet.
                RecievablePacketManager.handle(new ReceivablePacket(Encryption.Decrypt(bufferData)));
            }
        }
    }

    public void ChannelSend(SendablePacket packet)
    {
        if (SocketConnected())
        {
            socket.Send(Encryption.Encrypt(packet.GetSendableBytes()));
        }
        else // Connection closed.
        {
            DisconnectFromServer();
            // Go to login screen.
            SceneFader.Fade("LoginScreen", Color.white, 0.5f);
        }
    }

    private bool SocketConnected()
    {
        // return !(socket.Poll(1000, SelectMode.SelectRead) && socket.Available == 0);
        return socketConnected && socket != null && socket.Connected;
    }
}
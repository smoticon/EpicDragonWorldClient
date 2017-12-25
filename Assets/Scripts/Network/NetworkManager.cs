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

    void Start()
    {
        instance = this;
    }

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    void OnApplicationQuit()
    {
        if (socket != null)
        {
            if (socket.Connected)
            {
                socket.Close();
                socketConnected = false;
                readThreadStarted = false;
            }
            Debug.Log("Application ending after " + Time.time + " seconds.");
        }
    }

    // Best to call this only once per login attempt.
    public bool connectToServer()
    {
        if (socketConnected = false || socket == null || !socket.Connected)
        {
            connectSocket();
        }
        return SocketConnected();
    }

    private void connectSocket()
    {
        try
        {
            Debug.Log("Trying to connect.");
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            System.IAsyncResult result = socket.BeginConnect(serverIP, serverPort, null, null);
            bool success = result.AsyncWaitHandle.WaitOne(connectionTimeOut, true);

            if (!success)
            {
                socketConnected = false;
                socket.Close();
                Debug.Log("Failed to connect.");
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
                    Debug.Log("Connected.");
                }
                else
                {
                    socketConnected = false;
                    socket.Close();
                    Debug.Log("Failed to connect.");
                }
            }
        }
        catch (SocketException se)
        {
            socketConnected = false;
            readThreadStarted = false;
            Debug.Log("SocketException :" + se.SocketErrorCode);
        }
    }

    private void ChannelRead()
    {
        byte[] bytes = new byte[1024 * 1024];
        int len = 0;

        while (readThreadStarted)
        {
            len = socket.Receive(bytes);
            if (len > 0)
            {
                ReceivablePacket packet = new ReceivablePacket(Encryption.decrypt(bytes));
                // TODO: Handle message.
            }
        }
    }

    public void ChannelSend(byte[] bytes)
    {
        if (SocketConnected()) // Connection closed.
        {
            socketConnected = false;
            readThreadStarted = false;
            // TODO: Manage this in a better way.
            Application.Quit(); // Close the client.
        }
        else
        {
            socket.Send(Encryption.encrypt(bytes));
        }
    }

    private bool SocketConnected()
    {
        // return !(socket.Poll(1000, SelectMode.SelectRead) && socket.Available == 0);
        return socketConnected && socket != null && socket.Connected;
    }
}
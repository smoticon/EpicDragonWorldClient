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
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEditor;
using UnityEngine;

/**
 * @author Pantelis Andrianakis
 */
public class NetworkHandler : MonoBehaviour
{
    // For socket send.
    Socket socket;
    string serverIP = "127.0.0.1";
    int serverPort = 5055;
    int connectionTimeOut = 5000;
    bool socketInitialized = false;
    bool socketError = false;

    // For socket receive.
    Thread receiveThread;
    bool receiveThreadStarted = false;
    string messageReceived = "";
    byte[] bytes = new byte[1024];
    
    void OnApplicationQuit()
    {
        if (socket != null)
        {
            if (socket.Connected)
            {
                socket.Close();
                receiveThreadStarted = false;
            }
            Debug.Log("Application ending after " + Time.time + " seconds.");
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    // Use this for initialization.
    void Start()
    {
        if (!socketInitialized)
        {
            socketInitialized = true;
            InitSocket();

            if (socketError)
            {
                // Send a popup window notification.
                EditorUtility.DisplayDialog("Network error!", "Could not connect to the server.", "OK");

                // Application.Quit() does not work in the editor so UnityEditor.EditorApplication.isPlaying need to be set to false to end the game.
                #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
                #else
                    Application.Quit(); // Quit the game.
                #endif
            }
        }
    }

    void InitSocket()
    {
        try
        {
            socketError = false;
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Debug.Log("Trying to connect.");
            System.IAsyncResult result = socket.BeginConnect(serverIP, serverPort, null, null);
            bool success = result.AsyncWaitHandle.WaitOne(connectionTimeOut, true);

            if (!success)
            {
                socketError = true;
                Debug.Log("Failed to connect.");
                socket.Close();
            }
            else
            {
                if (socket.Connected)
                {
                    Debug.Log("Connected.");

                    // Start Receive thread.
                    receiveThreadStarted = true;
                    receiveThread = new Thread(new ThreadStart(receiveHandler));
                    receiveThread.Start();
                }
                else
                {
                    socketError = true;
                    Debug.Log("Failed to connect.");
                    socket.Close();
                }
            }
        }
        catch (SocketException se)
        {
            socketError = true;
            Debug.Log("SocketException :" + se.SocketErrorCode);
        }
    }

    public void Send()
    {
        if (socket != null && socket.Connected)
        {
            socket.Send(Encoding.UTF8.GetBytes("TEST" + "\r\n"));
        }
    }

    void receiveHandler()
    {
        int len = 0;
        while (receiveThreadStarted)
        {
            len = socket.Receive(bytes);
            if (len > 0)
            {
                messageReceived = Encoding.UTF8.GetString(bytes, 0, len);
                if (messageReceived.Contains("\n"))
                {
                    string[] split = messageReceived.Split(new string[] {"\n"}, System.StringSplitOptions.None);
                    for (int i = 0; i < split.Length - 1; i++)
                    {
                        Debug.Log("Recieved " + split[i]);
                    }
                }
            }
        }
    }
}
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
    ThreadStart ts;
    Thread socketThread;
    bool socketThreadStarted = false;
    bool socketThreadError = false;
    int connectionTimeOut = 5000;

    // For socket receive.
    ThreadStart ts2;
    Thread receiveThread;
    bool receiveThreadStarted = false;
    string messageReceived = "";
    int zeroByteCount = 0;
    List<string> receiveMsgList = new List<string>();
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
        if (!socketThreadStarted)
        {
            socketThreadStarted = true;
            ts = new ThreadStart(openSocket);
            socketThread = new Thread(ts);
            socketThread.Start();
            socketThread.Join(); // Wait until it ends, we need socketThreadError to be updated.

            if (socketThreadError)
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

    // Update is called once per frame.
    void Update()
    {
    }

    void openSocket()
    {
        try
        {
            socketThreadError = false;
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Debug.Log("Trying to connect.");
            System.IAsyncResult result = socket.BeginConnect(serverIP, serverPort, null, null);
            bool success = result.AsyncWaitHandle.WaitOne(connectionTimeOut, true);

            if (!success)
            {
                socketThreadError = true;
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
                    ts2 = new ThreadStart(receiveHandler);
                    receiveThread = new Thread(ts2);
                    receiveThread.Start();
                }
                else
                {
                    socketThreadError = true;
                    Debug.Log("Failed to connect.");
                    socket.Close();
                }
            }
        }
        catch (SocketException se)
        {
            socketThreadError = true;
            Debug.Log("SocketException :" + se.SocketErrorCode);
        }
    }

    void receiveHandler()
    {
        int bytesRec = 0;
        while (receiveThreadStarted)
        {
            if (bytesRec == 0)
            {
                zeroByteCount++;
                if (zeroByteCount > 20)
                {
                    // Connection closed.
                    receiveThreadStarted = false;
                }
            }
            bytesRec = socket.Receive(bytes);

            messageReceived = Encoding.ASCII.GetString(bytes, 0, bytesRec);

            if (messageReceived.Contains("\n"))
            {
                string[] split = messageReceived.Split(new string[] { "\n" }, System.StringSplitOptions.None);
                for (int i = 0; i < split.Length - 1; i++)
                {
                    receiveMsgList.Add(split[i]);
                    Debug.Log("Added " + split[i] + " into List.");
                }
            }
        }
    }
}
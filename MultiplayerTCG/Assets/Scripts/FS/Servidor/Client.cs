using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class Client : MonoBehaviour
{
    private TcpClient client;
    private NetworkStream stream;
    private string playerId;

    private MensagerReceptor mensagerReceptor;

    Queue<string> messageQueue = new Queue<string>();

    void Start()
    {
        client = new TcpClient();
        client.BeginConnect("127.0.0.1", 7777, new AsyncCallback(OnConnect), null);

        mensagerReceptor = GetComponent<MensagerReceptor>();
    }

    private void Update()
    {
        lock (messageQueue)
        {
            while (messageQueue.Count > 0)
            {
                string message = messageQueue.Dequeue();
                mensagerReceptor = FindObjectOfType<MensagerReceptor>();

                mensagerReceptor.ReciveMensage(message);
            }
        }
    }

    void OnConnect(IAsyncResult ar)
    {
        if (client.Connected)
        {
            Debug.Log("Connected to server");
            stream = client.GetStream();
            byte[] buffer = new byte[1024];
            stream.BeginRead(buffer, 0, buffer.Length, new AsyncCallback(OnDataReceived), buffer);
        }
    }

    public void SendMessageToServer(string message)
    {
        if (client.Connected)
        {
            byte[] requestData = Encoding.ASCII.GetBytes(message);
            stream.BeginWrite(requestData, 0, requestData.Length, null, null);
        }
    }

    void OnDataReceived(IAsyncResult ar)
    {
        
        byte[] buffer = (byte[])ar.AsyncState;
        int bytesRead = stream.EndRead(ar);
        string response = Encoding.ASCII.GetString(buffer, 0, bytesRead);
        if (response != "")
        {
            Debug.Log("Received from server: " + response);
        }

        lock (messageQueue)
        {
            messageQueue.Enqueue(response);
        }

        // Continuar lendo dados do servidor
        stream.BeginRead(buffer, 0, buffer.Length, new AsyncCallback(OnDataReceived), buffer);
    }

    private void OnDisable()
    {
        DisconnectFromServer();
    }

    void OnApplicationQuit()
    {
        DisconnectFromServer();
    }

    public void DisconnectFromServer()
    {
        if (client != null)
        {
            try
            {
                NetworkStream stream = client.GetStream();
                byte[] disconnectMessage = Encoding.ASCII.GetBytes("Disconnect");
                stream.Write(disconnectMessage, 0, disconnectMessage.Length);
                stream.Flush(); // Ensure the message is sent before closing
            }
            catch (Exception e)
            {
                Debug.Log("Error sending disconnect message: " + e.Message);
            }
            finally
            {
                client.Close();
                Debug.Log("Client connection closed.");
            }
        }
    }
}

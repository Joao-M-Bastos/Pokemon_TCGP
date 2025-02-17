using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

public class Server : MonoBehaviour
{
    private TcpListener server;
    private TcpClient player1;
    private TcpClient player2;

    void Start()
    {
        server = new TcpListener(IPAddress.Any, 7777);
        server.Start();
        Debug.Log("Server started on port 7777");
        server.BeginAcceptTcpClient(new AsyncCallback(OnClientConnect), null);
    }

    void OnClientConnect(IAsyncResult ar)
    {
        TcpClient client = server.EndAcceptTcpClient(ar);
        Debug.Log("Client connected");

        if (player1 == null)
        {
            player1 = client;
            Debug.Log("Player 1 connected");
            //SendMessageToClient(player1, "Player 1");

            
        }
        else if (player2 == null)
        {
            player2 = client;
            Debug.Log("Player 2 connected");
            //SendMessageToClient(player2, "Player 2");

            StartGame();
        }

        // Continuar aceitando mais clientes
        server.BeginAcceptTcpClient(new AsyncCallback(OnClientConnect), null);

        // leitura de dados do cliente
        NetworkStream stream = client.GetStream();
        byte[] buffer = new byte[1024];
        stream.BeginRead(buffer, 0, buffer.Length, new AsyncCallback(OnDataReceived), new { client, buffer });
    }

    private void StartGame()
    {
        int coinFlip = Random.Range(0,2);
        
        if (coinFlip == 0)
        {
            SendMessageToClient(player1, "StartTurn:0");
            SendMessageToClient(player2, "StartTurn:1");
        }
        else
        {
            SendMessageToClient(player1, "StartTurn:1");
            SendMessageToClient(player2, "StartTurn:0");
        }
    }

    void OnDataReceived(IAsyncResult ar)
    {
        var state = (dynamic)ar.AsyncState;
        TcpClient client = state.client;
        byte[] buffer = state.buffer;

        NetworkStream stream = client.GetStream();
        int bytesRead = stream.EndRead(ar);
        string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
        

        if (message != ""){
            Debug.Log("Sending message to " + client + ": " + message);
        }

        // Enviar a mensagem para o outro jogador
        if (client == player1 && player2 != null)
        {
            
            SendMessageToClient(player2, message);
        }
        else if (client == player2 && player1 != null)
        {
            SendMessageToClient(player1, message);
        }

        // Continuar lendo dados do cliente
        stream.BeginRead(buffer, 0, buffer.Length, new AsyncCallback(OnDataReceived), new { client, buffer });
    }

    void SendMessageToClient(TcpClient client, string message)
    {
        NetworkStream stream = client.GetStream();
        byte[] responseData = Encoding.ASCII.GetBytes(message);
        stream.Write(responseData, 0, responseData.Length);
    }

    void OnApplicationQuit()
    {
        server.Stop();
    }
}

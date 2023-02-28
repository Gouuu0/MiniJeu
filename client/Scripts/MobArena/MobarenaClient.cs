using Godot;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class MobarenaClient : Node
{
    public static IPAddress ip = IPAddress.Parse("127.0.0.1");
    public static TcpClient tcpClient = new TcpClient();
    public static NetworkStream stream;
    public static System.Threading.Thread listeningThread;

    public override void _Ready()
    {
        tcpClient.Connect(ip, 6700);

        stream = tcpClient.GetStream();

        listeningThread = new System.Threading.Thread(new ThreadStart(ReceiveMessage));
        listeningThread.Start();
    }

    public static void SendMessage(string pMessage)
    {
        byte[] lMessage = Encoding.ASCII.GetBytes(pMessage);
        stream.Write(lMessage, 0, lMessage.Length);
    }

    void ReceiveMessage()
    {
        while (true)
        {
            byte[] lResponseBuffer = new byte[1024];
            int lResponseLength = stream.Read(lResponseBuffer, 0, lResponseBuffer.Length);
            if (lResponseLength != 0)
            {
                string lResponse = Encoding.ASCII.GetString(lResponseBuffer, 0, lResponseLength);

                switch (lResponse)
                {
                    case "HIDEBUTTON":
                        break;
                    case "HEHEHEHAW":
                        break;
                    default:
                        break;
                }
            }
        }
        
    }

    public override void _Notification(int what)
    {
        if (what == MainLoop.NotificationWmQuitRequest)
        {
            SendMessage("player_disconnect");
            listeningThread.Abort();
            stream.Close();
            tcpClient.Close();
        }
    }
}
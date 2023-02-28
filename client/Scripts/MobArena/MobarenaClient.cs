using Godot;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class MobarenaClient : Node2D
{
    [Export] PackedScene otherPlayerScene;
    [Export] NodePath otherPlayerContainerPath;
    Node2D otherPlayerContainer;

    public static IPAddress ip = IPAddress.Parse("25.64.184.155");
    public static TcpClient tcpClient = new TcpClient();
    public static NetworkStream stream;
    public static System.Threading.Thread listeningThread;
    public static System.Threading.Thread positionThread;
    public static int id = -1;

    public static List<MobArenaOtherPlayer> otherPlayerList = new List<MobArenaOtherPlayer>(); 

    public static bool isConnected = false;

    public override void _Ready()
    {
        tcpClient.Connect(ip, 6700);

        stream = tcpClient.GetStream();

        listeningThread = new System.Threading.Thread(new ThreadStart(ReceiveMessage));
        listeningThread.Start();

        otherPlayerContainer = GetNode<Node2D>(otherPlayerContainerPath);

        SendMessage("JOIN");
    }

    public static void SendMessage(string pMessage)
    {
        byte[] lMessage = Encoding.ASCII.GetBytes(pMessage+"/");
        stream.Write(lMessage, 0, lMessage.Length);
    }

    void ReceiveMessage()
    {
        while (true)
        {
            byte[] lResponseBuffer = new byte[4096];
            int lResponseLength = stream.Read(lResponseBuffer, 0, lResponseBuffer.Length);
            if (lResponseLength != 0)
            {
                string lResponse = Encoding.ASCII.GetString(lResponseBuffer, 0, lResponseLength);
                string[] lArgsResponse = lResponse.Split(":");

                switch (lArgsResponse[0])
                {
                    case "CONNECTED":
                        isConnected = true;
                        id = lArgsResponse[1].ToInt();
                        positionThread = new System.Threading.Thread(new ThreadStart(SendInfo));
                        positionThread.Start();
                        break;
                    case "PLAYERS":
                        for (int i = 1; i < lArgsResponse.Length; i++)
                        {
                            string[] lPlayerData = lArgsResponse[i].Split("|");
                            int playerId = lPlayerData[0].ToInt();

                            if(playerId != id)
                            {
                                Vector2 playerPos = new Vector2(lPlayerData[1].ToFloat(), lPlayerData[2].ToFloat());
                                float playerRot = lPlayerData[3].ToFloat();

                                MobArenaOtherPlayer lPlayer = ReturnIfExistInList(playerId);

                                if (lPlayer == null)
                                {
                                    lPlayer = (MobArenaOtherPlayer)otherPlayerScene.Instance();
                                    otherPlayerContainer.AddChild(lPlayer);
                                    lPlayer.id = playerId;
                                    otherPlayerList.Add(lPlayer);
                                }

                                lPlayer.moveTween.StopAll();
                                lPlayer.moveTween.InterpolateProperty(lPlayer, "position", lPlayer.Position, playerPos, .25f);
                                lPlayer.moveTween.Start();
                                lPlayer.RotationDegrees = playerRot;
                            }
                        }
                        break;
                    case "PLAYERDELETE":
                        MobArenaOtherPlayer lPlayerToDelete = ReturnIfExistInList(lArgsResponse[1].ToInt());
                        otherPlayerList.Remove(lPlayerToDelete);
                        lPlayerToDelete.QueueFree();
                        break;
                    default:
                        break;
                }
            }
        }
    }

    MobArenaOtherPlayer ReturnIfExistInList(int pId)
    {
        foreach (MobArenaOtherPlayer item in otherPlayerList)
        {
            if (item.id == pId)
                return item;
        }

        return null;
    }

    void SendInfo()
    {
        while (true)
        {
            SendMessage("POS:" + MobArenaPlayer.Instance.Position.x.ToString() + ":" + MobArenaPlayer.Instance.Position.y.ToString() + ":" + MobArenaPlayer.Instance.RotationDegrees.ToString());
            System.Threading.Thread.Sleep(50);
        }
    }

    public override void _Notification(int what)
    {
        if (what == MainLoop.NotificationWmQuitRequest)
        {
            SendMessage("DISCONNECT");
            listeningThread.Abort();
            positionThread.Abort();
            stream.Close();
            tcpClient.Close();
        }
    }
}
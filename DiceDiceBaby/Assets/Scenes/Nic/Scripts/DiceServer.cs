using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System;
using System.IO;
using System.Text;

public class DiceServer
{
    public string address;
    public Dictionary<int, DicePlayer> players;
    LobbyMaster lobby;

    TcpListener server = null;

    List<int> availableCharacters;

    public DiceServer(NetworkDiscovery discovery, LobbyMaster l)
    {
        availableCharacters = new List<int>(new int[]{ 0, 1, 2, 3});
        players = new Dictionary<int, DicePlayer>();
        lobby = l;
        
        NetworkServer.Listen(4444);
        NetworkServer.RegisterHandler(MsgType.Connect, OnConnected);
        NetworkServer.RegisterHandler(DiceMsg.Ready, OnReady);
        NetworkServer.RegisterHandler(DiceMsg.DraftPick, OnDraftPick);

        //NetworkServer.RegisterHandler(DiceMsg.Chat, OnChat);

        address = LocalIPAddress();
        Debug.Log("host ip: " + address);       
    }

    public string LocalIPAddress()
    {
        IPHostEntry host;
        string localIP = "";
        host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (IPAddress ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                localIP = ip.ToString();
                break;
            }
        }
        return localIP;
    }

    public void OnConnected(NetworkMessage netMsg)
    {
        Debug.Log("something connected to server");
        Debug.Log("channel id: " + netMsg.channelId);

        if (players.Count < 2)
        {
            Debug.Log("Adding a player to the game");
            // create new dice player
            var newPlayer = new DicePlayer();
            newPlayer.playerNum = players.Count + 1;
            newPlayer.connectionId = netMsg.conn.connectionId;
            newPlayer.avatar = TAGD.ChooseRandom(availableCharacters.ToArray());
            availableCharacters.Remove(newPlayer.avatar);
            players.Add(newPlayer.playerNum, newPlayer);

            NetworkServer.SendToClient(netMsg.conn.connectionId, DiceMsg.Welcome,
                new WelcomeMsg()
                {
                    totalPlayers = players.Count,
                    you = newPlayer,
                    them = OtherPlayer(newPlayer.playerNum)
                });
        }
        else
        {
            Debug.Log("Battle already full");
            NetworkServer.SendToClient(netMsg.conn.connectionId, DiceMsg.Welcome,
                new WelcomeMsg()
                {
                    totalPlayers = -1
                });
        }

        var lobbyMsg = new LobbyMsg()
        {
            allPlayers = (new List<DicePlayer>(players.Values)).ToArray()
        };
        NetworkServer.SendToAll(DiceMsg.Lobby, lobbyMsg);
    }

    DicePlayer OtherPlayer(int num)
    {
        if (num == 1 && players.ContainsKey(2))
            return players[2];

        if (num == 2 && players.ContainsKey(1))
            return players[1];

        return new DicePlayer { playerNum = -1 };
    }

    void OnReady(NetworkMessage netMsg)
    {
        var msg = netMsg.ReadMessage<ReadyMsg>();
        if(players.ContainsKey(msg.fromPlayer))
        {
            var p = players[msg.fromPlayer];
            p.ready = msg.ready;
            players[msg.fromPlayer] = p;

            var lobbyMsg = new LobbyMsg()
            {
                allPlayers = (new List<DicePlayer>(players.Values)).ToArray()
            };
            Debug.Log("got a ready msg from "+p.playerNum);
            Debug.Log("is ready: " + players[msg.fromPlayer].ready);
            NetworkServer.SendToAll(DiceMsg.Lobby, lobbyMsg);

            if(AllReady())
            {
                lobby.GenerateDice();
            }
        }
    }

    void OnDraftPick(NetworkMessage netMsg)
    {
        var msg = netMsg.ReadMessage<DraftPickMsg>();

        var enemyConnectionId = GetOtherPlayer(msg.fromPlayer).connectionId;

        NetworkServer.SendToClient(enemyConnectionId, DiceMsg.DraftPick,
                new DraftPickMsg()
                {
                    dice = msg.dice
                });

    }

    DicePlayer GetOtherPlayer(int from)
    {
        if (from == 1)
            return players[2];
        else if (from == 2)
            return players[1];
        else
            Debug.LogError("No other playter!!!");

        return new DicePlayer { playerNum = -1 };
    }

    bool AllReady()
    {
        if (players.ContainsKey(1) && players[1].ready
            && players.ContainsKey(2) && players[2].ready)
            return true;
        return false;
    }

    public void SendDicePool(string diceData, int recievingPlayer)
    {
        var playerToGet = players[recievingPlayer];
        NetworkServer.SendToClient(playerToGet.connectionId, DiceMsg.DicePool,
                new DicePoolMsg()
                {
                    diceData = diceData
                });
    }

    public void SendDiceTurn(int whosTurn)
    {
        NetworkServer.SendToAll(DiceMsg.DraftTurn, new DiceDraftTurnMsg()
        {
            whosTurn = whosTurn
        });
    }


    public void OnChat(NetworkMessage netMsg)
    {
        NetworkServer.SendToAll(DiceMsg.Chat, netMsg.ReadMessage<ChatMsg>());
    }
}

[System.Serializable]
public struct DicePlayer
{
    public int playerNum;
    public int avatar;
    public bool ready;
    public int connectionId;
}




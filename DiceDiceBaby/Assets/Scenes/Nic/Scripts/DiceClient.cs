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

public class DiceClient
{
    public string address;

    NetworkClient client;
    LobbyMaster lobby;

    public int playerNum;
    public int avatarNum;

    public DiceClient(string serverIP, LobbyMaster l, bool local)
    {
        lobby = l;
        if (local)
        {
            Debug.Log("local server");
            client = ClientScene.ConnectLocalServer();
        }
        else
        {
            client = new NetworkClient();
        }

        client.RegisterHandler(MsgType.Connect, OnConnected);
        client.RegisterHandler(DiceMsg.Welcome, OnWelcome);
        client.RegisterHandler(DiceMsg.Lobby, OnLobby);

        client.RegisterHandler(DiceMsg.DicePool, OnDicePool);
        client.RegisterHandler(DiceMsg.DraftTurn, OnDraftTurn);
        client.RegisterHandler(DiceMsg.DraftPick, OnEnemyDraftPick);

        client.RegisterHandler(DiceMsg.Mana, OnEnemyMana);
        client.RegisterHandler(DiceMsg.Spell, OnEnemySpell);
        client.RegisterHandler(DiceMsg.PlayerInfo, OnEnemyInfo);

        Debug.Log("trying to connect to "+serverIP);
        client.Connect(serverIP, 4444);
    }

    public void OnConnected(NetworkMessage netMsg)
    {
        Debug.Log("you connected successfully");
        //chats.AddMessage(0, "Client: on connected");
    }

    public void OnWelcome(NetworkMessage netMsg)
    {
        var msg = netMsg.ReadMessage<WelcomeMsg>();
        Debug.Log("welcome to the server");

        if(msg.totalPlayers == -1)
        {
            Debug.Log("Battle is already full, sending you back to lobby");
            lobby.ShowTitle();
        }
        else
        {
            playerNum = msg.you.playerNum;
            avatarNum = msg.you.avatar;

            var theirAvatar = -1;

            if (msg.them.playerNum != -1)
                theirAvatar = msg.them.avatar;

            lobby.ShowAvatars(avatarNum, theirAvatar);
            lobby.CheckGameStatus(msg.you, msg.them);
            lobby.BattleJoined();
        }        
    }

    public void ReadyUp()
    {
        client.Send(DiceMsg.Ready, new ReadyMsg() { fromPlayer = playerNum, ready = true });
    }

    public void SendDicePick(string dice)
    {
        client.Send(DiceMsg.DraftPick, new DraftPickMsg() { fromPlayer = playerNum, dice = dice });
    }

    public void SendMyMana(string manaData)
    {
        client.Send(DiceMsg.Mana, new ManaMsg() { fromPlayer = playerNum, mana = manaData });
    }

    public void SendSpell(string spellData)
    {
        client.Send(DiceMsg.Spell, new SpellMsg() { fromPlayer = playerNum, spell = spellData });
    }

    public void SendMyInfo(string playerInfo)
    {
        client.Send(DiceMsg.PlayerInfo, new PlayerInfoMsg() { fromPlayer = playerNum, info = playerInfo });
    }

    // responses handlers

    public void OnLobby(NetworkMessage netMsg)
    {
        var msg = netMsg.ReadMessage<LobbyMsg>();
        Debug.Log("got lobby msg");

        if(msg.allPlayers.Length == 2)
        {
            DicePlayer you, them;
            you = You(msg.allPlayers);
            them = Them(msg.allPlayers);

            lobby.ShowAvatars(you.avatar, them.avatar);
            lobby.CheckGameStatus(you, them);
        }
        else
        {
            lobby.statusText.text = "Waiting for another player to join";
        }

    }

    void OnDicePool(NetworkMessage netMsg)
    {
        var msg = netMsg.ReadMessage<DicePoolMsg>();
        lobby.SetDicePool(msg.diceData);
    }

    public void OnDraftTurn(NetworkMessage netMsg)
    {
        var msg = netMsg.ReadMessage<DiceDraftTurnMsg>();
        lobby.SetDiceTurn(msg.whosTurn);
    }

    public void OnEnemyDraftPick(NetworkMessage netMsg)
    {
        var msg = netMsg.ReadMessage<DraftPickMsg>();
        lobby.EnemyPicked(msg.dice);
    }

    public void OnEnemyMana(NetworkMessage netMsg)
    {
        var msg = netMsg.ReadMessage<ManaMsg>();
        lobby.LearnEnemyMana(msg.mana);
    }

    public void OnEnemySpell(NetworkMessage netMsg)
    {
        var msg = netMsg.ReadMessage<SpellMsg>();
        lobby.EnemyCastSpell(msg.spell);
    }

    public void OnEnemyInfo(NetworkMessage netMsg)
    {
        var msg = netMsg.ReadMessage<PlayerInfoMsg>();
        lobby.EnemyInfo(msg.info);
    }

    DicePlayer You(DicePlayer[] all)
    {
        foreach (var p in all)
            if (p.playerNum == playerNum)
                return p;

        Debug.LogError("You arent in the players list!");
        return new DicePlayer();
    }

    DicePlayer Them(DicePlayer[] all)
    {
        foreach (var p in all)
            if (p.playerNum != playerNum)
                return p;

        Debug.LogError("Your opponent isn't in the players list!");
        return new DicePlayer();
    }

    /*
    public void OnChat(NetworkMessage netMsg)
    {
        var msg = netMsg.ReadMessage<ChatMsg>();
        Debug.Log("got chat");
    }

    public void SendChat(string msg)
    {
        client.Send(DiceMsg.Chat, new ChatMsg() { fromPlayer = playerNum, msg = msg });
    }
    */
}


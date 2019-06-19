using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CustomMsgID
{
    public static short Something = 1000;
    public static short PlayerJoin = 1001;
    public static short BattleBegin = 1002;
    public static short Spell = 1003;

};

public class SomethingMessage : MessageBase
{
    public string whatever;
}

public class LobbyMaster : MonoBehaviour
{
    public Text statusText;
    public Button readyButt;

    public bool serverUp;
    NetworkClient client;

    void Start()
    {
        readyButt.enabled = false;
        readyButt.GetComponentInChildren<Text>().text = "Waiting for Opponent";

        statusText.text = "Deciding what to do...";

        if (GameInfo.netRole == NetRole.hostPlayer)
        {
            // host a game
            statusText.text = "hosting a game";
            SetupServer();
            SetupLocalClient();
        }
        else if (GameInfo.netRole == NetRole.player)
        {
            // search for a game
            statusText.text = "looking for a game to join";
            SetupClient();
        }

    }

    void Update()
    {
        if(client != null && Input.GetKeyDown(KeyCode.Space))
        {
            var msg = new SomethingMessage()
            {
                whatever = "yo yo bro: " + Time.time
            };
            
            client.Send(CustomMsgID.Something, msg);
        }
    }

    // Create a server and listen on a port
    public void SetupServer()
    {
        NetworkServer.Listen(4444);
        NetworkServer.RegisterHandler(CustomMsgID.Something, ServerMessage);
        GetComponent<NetworkDiscovery>().Initialize();
        GetComponent<NetworkDiscovery>().StartAsServer();

        statusText.text = "Host running";
    }

    public void ServerMessage(NetworkMessage msg)
    {
        Debug.Log("server got message");
        NetworkServer.SendToAll(CustomMsgID.Something, msg.ReadMessage<SomethingMessage>());
    }

    // Create a client and connect to the server port
    public void SetupClient()
    {
        GetComponent<NetworkDiscovery>().Initialize();
        GetComponent<NetworkDiscovery>().StartAsClient();

        statusText.text = "looking for hosts";
    }

    public void ConnectClient(string serverIP)
    {
        client = new NetworkClient();
        client.RegisterHandler(MsgType.Connect, OnConnected);
        client.RegisterHandler(CustomMsgID.Something, OnCommand);
        client.Connect(serverIP, 4444);

        statusText.text = "connected to host";

        GetComponent<NetworkDiscovery>().StopAllCoroutines();
        GetComponent<NetworkDiscovery>().StopBroadcast();
    }

    // Create a local client and connect to the local server
    public void SetupLocalClient()
    {
        client = ClientScene.ConnectLocalServer();
        client.RegisterHandler(MsgType.Connect, OnConnected);
        client.RegisterHandler(CustomMsgID.Something, OnCommand);
    }

    public void OnConnected(NetworkMessage netMsg)
    {
        Debug.Log("Connected to server");

        statusText.text = "connection complete";
        

    }

    public void OnCommand(NetworkMessage netMsg)
    {
        Debug.Log("GotMessage");
        var msg = netMsg.ReadMessage<SomethingMessage>();
        Debug.Log(msg.whatever);
    }
}

public class MyNetworkDiscovery : NetworkDiscovery
{
    public override void OnReceivedBroadcast(string fromAddress, string data)
    {
        Debug.Log("Received broadcast from: " + fromAddress + " with the data: " + data);
    }
}

using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class OverriddenNetworkDiscovery : NetworkDiscovery
{
    public override void OnReceivedBroadcast(string fromAddress, string data)
    {
        GetComponent<LobbyMaster>().ConnectClient(fromAddress);
        Debug.Log("Received broadcast from: " + fromAddress + " with the data: " + data);
    }
}
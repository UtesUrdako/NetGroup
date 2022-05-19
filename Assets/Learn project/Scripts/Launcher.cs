using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks
{
    public static bool isConnectedToMaster;
    /// <summary>
    /// This client's version number. Users are separated from each other bygameVersion (which allows you to make breaking changes).
    /// </summary>
    string gameVersion = "1";
    /// <summary>
    /// MonoBehaviour method called on GameObject by Unity during earlyinitialization phase.
    /// </summary>
    void Awake()
    {
        // #Critical
        // this makes sure we can use PhotonNetwork.LoadLevel() on the master clientand all clients in the same room sync their level automatically
        PhotonNetwork.AutomaticallySyncScene = false;
    }
    /// <summary>
    /// MonoBehaviour method called on GameObject by Unity during initializationphase.
    /// </summary>
    void Start()
    {
        //Connect();
    }
    /// <summary>
    /// Start the connection process.
    /// - If already connected, we attempt joining a random room
    /// - if not yet connected, Connect this application instance to Photon CloudNetwork
    /// </summary>
    public void Connect()
    {
        // #Critical, we must first and foremost connect to Photon OnlineServer.
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.GameVersion = gameVersion;
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        PhotonNetwork.JoinRandomOrCreateRoom();
        isConnectedToMaster = true;
        Debug.Log($"Successful connected to master! Region: {PhotonNetwork.CloudRegion}, ping: {PhotonNetwork.GetPing()}");
        
        if (PhotonNetwork.IsConnected)
        {
            PhotonPeer.RegisterType(typeof(Vector2Int), 100, SerializeVector2Int, DeserializeVector2Int);
        }
    }
    
    public static object DeserializeVector2Int(byte[] data)
    {
        Vector2Int result = new Vector2Int();

        result.x = BitConverter.ToInt32(data, 0);
        result.y = BitConverter.ToInt32(data, 4);

        return result;
    }

    public static byte[] SerializeVector2Int(object obj)
    {
        Vector2Int vector = (Vector2Int)obj;
        byte[] result = new byte[8];

        BitConverter.GetBytes(vector.x).CopyTo(result, 0);
        BitConverter.GetBytes(vector.y).CopyTo(result, 4);

        return result;
    }
}

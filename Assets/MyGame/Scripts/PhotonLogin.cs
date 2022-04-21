using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class PhotonLogin : MonoBehaviourPunCallbacks
{
    string gameVersion = "1";     

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;    
    }
     void Start()
    {
        Connect();
    }

    public void Connect()
    {
        if (PhotonNetwork.IsConnected)
            PhotonNetwork.JoinRandomRoom();
        else
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
        }

    }

    public override void OnConnected()
    {
        Debug.Log($"Photon callbacks | Congratulation, you made successful API call! {PhotonNetwork.CloudRegion }, ping: {PhotonNetwork.GetPing()}");
    }
}

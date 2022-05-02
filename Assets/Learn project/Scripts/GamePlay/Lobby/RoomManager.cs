using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class RoomManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private RoomPlayerItem _roomPlayerPrefab;
    [SerializeField] private Transform _contentPlayer;

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.Log("Joined lobby");
        PhotonNetwork.JoinRandomOrCreateRoom();
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("Joined room");
        var players = PhotonNetwork.PlayerList;
        foreach (var player in players)
        {
            var playerItem = Instantiate(_roomPlayerPrefab, _contentPlayer);
            playerItem.SetPlayerName(player.NickName);
        }
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        Debug.Log("Created room");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        Debug.LogError($"Create room failed. code: {returnCode},\nmessage: {message}");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        Debug.LogError($"Joined room failed. code: {returnCode},\nmessage: {message}");
    }
}

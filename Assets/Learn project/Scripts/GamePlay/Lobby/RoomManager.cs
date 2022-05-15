using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoomManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private RoomPlayerItem _roomPlayerPrefab;
    [SerializeField] private Transform _contentPlayer;
    [SerializeField] private Button _startGame;

    private Dictionary<string, RoomPlayerItem> _roomPlayers = new Dictionary<string, RoomPlayerItem>();

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("Joined room");
        var players = PhotonNetwork.PlayerList;
        foreach (var player in players)
        {
            var playerItem = Instantiate(_roomPlayerPrefab, _contentPlayer);
            playerItem.SetPlayerName(player.NickName);
            _roomPlayers.Add(player.NickName, playerItem);
        }
        
        _startGame.onClick.AddListener(() => PhotonNetwork.LoadLevel(2));
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);

        if (!_roomPlayers.ContainsKey(newPlayer.NickName))
        {
            var playerItem = Instantiate(_roomPlayerPrefab, _contentPlayer);
            playerItem.SetPlayerName(newPlayer.NickName);
            _roomPlayers.Add(newPlayer.NickName, playerItem);
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);

        var nickName = otherPlayer.NickName;
        if (_roomPlayers.ContainsKey(nickName))
        {
            Destroy(_roomPlayers[nickName].gameObject);
            _roomPlayers.Remove(nickName);
        }
    }
}

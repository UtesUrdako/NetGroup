using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Photon.Pun;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;

    private void Start()
    {
        var number = PhotonNetwork.LocalPlayer.ActorNumber;
        var player = PhotonNetwork.Instantiate(_playerPrefab.name, transform.GetChild(number).position, Quaternion.identity);

        _virtualCamera.LookAt = player.transform;
        _virtualCamera.Follow = player.transform;
    }
}

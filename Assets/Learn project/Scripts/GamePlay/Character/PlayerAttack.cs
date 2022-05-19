using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class PlayerAttack : MonoBehaviourPunCallbacks, IOnEventCallback
{
    private Animator m_Animator;
    private Camera _camera;
    private float _cooldownTime = 3f;
    private bool _isCooldown;

    private int _health = 100;

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
        _camera = Camera.main;
    }

    void Update()
    {
        if (PhotonNetwork.IsConnected && !photonView.IsMine)
            return;
        
        if (Input.GetMouseButtonDown(0) && !_isCooldown)
        {
            _isCooldown = true;
            m_Animator.SetTrigger("Attack");
            Invoke(nameof(Cooldown), _cooldownTime);
            
            if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out var hit))
            {
                if (hit.collider.TryGetComponent(out PhotonView view) && !view.IsMine)
                    SetDamage(view.ViewID);
            }
        }
    }

    private void Cooldown() => _isCooldown = false;

    private void SetDamage(int idPlayer)
    {
        RaiseEventOptions options = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        SendOptions sendOptions = new SendOptions { Reliability = true };

        Vector2Int send = new Vector2Int(idPlayer, 20);
        PhotonNetwork.RaiseEvent(0, send, options, sendOptions);
    }

    public void OnEvent(EventData photonEvent)
    {
        switch (photonEvent.Code)
        {
            case 0:
            {
                var result = (Vector2Int) photonEvent.CustomData;
                if (photonView.IsMine && photonView.ViewID == result.x)
                {
                    _health -= result.y;
                    if (_health <= 0)
                    {
                        RaiseEventOptions options = new RaiseEventOptions {Receivers = ReceiverGroup.Others};
                        SendOptions sendOptions = new SendOptions {Reliability = true};

                        //Vector2Int send = new Vector2Int(photonView.ViewID, 20);
                        PhotonNetwork.RaiseEvent(1, photonView.ViewID, options, sendOptions);
                        
                        PhotonNetwork.LeaveRoom();
                    }
                }
            }
                break;
            case 1:
            {
                var result = (int) photonEvent.CustomData;
                if (photonView.IsMine && photonView.ViewID != result)
                {
                    PlayFabClientAPI.UpdateCharacterStatistics(new UpdateCharacterStatisticsRequest
                    {
                        CharacterId = CharacterManager._playCharacterName,
                        CharacterStatistics = new Dictionary<string, int>
                        {
                            {"Level", 1},
                            {"Exp", 1000},
                            {"Gold", 100}
                        }
                    }, result =>
                    {
                        PhotonNetwork.LeaveRoom();
                    }, Debug.LogError);
                }
            }
                break;
        }
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        PhotonNetwork.LoadLevel(0);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerAttack : MonoBehaviourPun
{
    private Animator m_Animator;
    private float _cooldownTime = 3f;
    private bool _isCooldown;

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
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
        }
    }

    private void Cooldown() => _isCooldown = false;
}

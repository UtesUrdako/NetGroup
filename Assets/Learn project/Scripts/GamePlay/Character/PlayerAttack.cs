using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
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
        if (Input.GetMouseButtonDown(0) && !_isCooldown)
        {
            _isCooldown = true;
            m_Animator.SetTrigger("Attack");
            Invoke(nameof(Cooldown), _cooldownTime);
        }
    }

    private void Cooldown() => _isCooldown = false;
}

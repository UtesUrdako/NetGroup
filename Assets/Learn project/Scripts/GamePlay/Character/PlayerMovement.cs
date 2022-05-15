using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerMovement : MonoBehaviourPun
{
    Vector3 m_Movement;
    Animator m_Animator;
    public float turnSpeed = 20f;
    Quaternion m_Rotation = Quaternion.identity;
    Rigidbody m_Rigidbody;

    AudioSource source;

    public float speed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (PhotonNetwork.IsConnected && !photonView.IsMine)
            return;
        
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        m_Movement.Set(horizontal, 0f, vertical);
        m_Movement.Normalize();

        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);

        bool isWalking = hasHorizontalInput || hasVerticalInput;
        m_Animator.SetBool("IsWalking", isWalking);

        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);
        m_Rotation = Quaternion.LookRotation(desiredForward);

        m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement * Time.fixedDeltaTime * speed);
    }

    private void OnAnimatorMove()
    {
        //m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement * m_Animator.deltaPosition.magnitude * speed);
        m_Rigidbody.MoveRotation(m_Rotation);
    }

    public void StepVoice()
    {
        //SoundLibrary.PlaySound(source, SoundLibrary.Sound.Step);
    }
}

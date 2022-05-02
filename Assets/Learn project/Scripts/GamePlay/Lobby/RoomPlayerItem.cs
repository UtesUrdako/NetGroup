using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomPlayerItem : MonoBehaviour
{
    [SerializeField] private Text _playerName;

    public void SetPlayerName(string playerName)
    {
        _playerName.text = playerName;
    }
}

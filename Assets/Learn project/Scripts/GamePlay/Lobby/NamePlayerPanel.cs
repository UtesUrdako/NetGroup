using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;

public class NamePlayerPanel : MonoBehaviour
{
    [SerializeField] private InputField _playerNameInputField;
    [SerializeField] private Button _savePlayerNameButton;

    public Action onSaveSucces;
    
    private string _playerName;

    private void Awake()
    {
        _playerNameInputField.onValueChanged.AddListener(SetName);
        _savePlayerNameButton.onClick.AddListener(SavePlayerName);
    }

    private void SetName(string newName)
    {
        _playerName = newName;
    }

    private void SavePlayerName()
    {
        _savePlayerNameButton.interactable = false;
        PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest {DisplayName = _playerName},
            result =>
            {
                PhotonNetwork.NickName = result.DisplayName;
                gameObject.SetActive(false);
                onSaveSucces?.Invoke();
            }, error =>
            {
                Debug.LogError(error);
                _savePlayerNameButton.interactable = true;
            });
    }
}

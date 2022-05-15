using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AccountManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _playerNameText;
    [SerializeField] private TMP_Text _playerCreateTimeText;
    [SerializeField] private Button _deletePlayerButton;
    [SerializeField] private NamePlayerPanel _namePlayerPanel;
    [SerializeField] private GameObject _roomPanel;
    
    void Start()
    {
        _deletePlayerButton.onClick.AddListener(PlayerPrefs.DeleteAll);
        //_namePlayerPanel.onSaveSucces
        //PlayFabClientAPI.GetPlayerProfile();
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(),
        result =>
        {
            Debug.Log($"PlayFab ID: {result.AccountInfo.PlayFabId}");

            _playerNameText.text = $"PlayFab ID: {result.AccountInfo.PlayFabId}";
            _playerCreateTimeText.text = $"Player was created at: {result.AccountInfo.Created.ToShortDateString()}";
            
            CheckPlayerName(result.AccountInfo.TitleInfo.DisplayName);
            
#if UNITY_ANDROID
            Social.localUser.Authenticate(resultSocial =>
            {
                if (resultSocial)
                    result.AccountInfo.GoogleInfo.GoogleId = Social.Active.localUser.id;
            });
#endif
        }, error =>
        {
            var errorMessage = error.GenerateErrorReport();
            Debug.LogError($"Something went wrong: {errorMessage}");
        });
    }
    
    private void CheckPlayerName(string name)
    {
        if (!string.IsNullOrEmpty(name))
        {
            PhotonNetwork.NickName = name;
            _roomPanel.SetActive(true);
            GetComponent<Launcher>().Connect();
        }
        else
        {
            _namePlayerPanel.gameObject.SetActive(true);
            _namePlayerPanel.onSaveSucces = () =>
            {
                _roomPanel.SetActive(true);
                GetComponent<Launcher>().Connect();
            };
        }
    }
}

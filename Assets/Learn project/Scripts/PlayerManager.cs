using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _playerNameText;
    [SerializeField] private TMP_Text _playerCreateTimeText;
    [SerializeField] private Button _deletePlayerButton;
    
    void Start()
    {
        _deletePlayerButton.onClick.AddListener(PlayerPrefs.DeleteAll);
        
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(),
        result =>
        {
            Debug.Log($"PlayFab ID: {result.AccountInfo.PlayFabId}");

            _playerNameText.text = $"PlayFab ID: {result.AccountInfo.PlayFabId}";
            _playerCreateTimeText.text = $"Player was created at: {result.AccountInfo.Created.ToShortDateString()}";
            
            // Social.localUser.Authenticate(resultSocial =>
            // {
            //     if (resultSocial)
            //         result.AccountInfo.GoogleInfo.GoogleId = Social.Active.localUser.id;
            // });
        }, error =>
        {
            var errorMessage = error.GenerateErrorReport();
            Debug.LogError($"Something went wrong: {errorMessage}");
        });
    }
}

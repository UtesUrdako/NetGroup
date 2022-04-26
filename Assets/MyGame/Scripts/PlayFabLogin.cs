using PlayFab;
using PlayFab.ClientModels;
using System;
using TMPro;
using UnityEngine;

public class PlayFabLogin : MonoBehaviour
{
    private string tempName;
    [SerializeField]
    GameObject obgForTextComponent;

    private void OnLoginSuccess(LoginResult result)
    { Debug.Log("Congratulation, you made successful API call!");
        obgForTextComponent.GetComponent<TMP_Text>().text = "Connected";
        obgForTextComponent.GetComponent<TMP_Text>().color = new Color32(8, 130, 0, 255);
        LetsTakeAboutPlayFabConnection();
    }
    private void OnLoginFailure(PlayFabError error)
    {
        var errorMessage = error.GenerateErrorReport();
        Debug.LogError($"Something went wrong: {errorMessage}");
        obgForTextComponent.GetComponent<TMP_Text>().text = "fail to connect";
        obgForTextComponent.GetComponent<TMP_Text>().color = new Color32(147, 23, 27, 255);
        LetsTakeAboutPlayFabConnection();
    }

    public void ConnectPlayFab()
    {
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
        {
            // if not we need to assign it to the appropriate variable manually
            //otherwise we can just remove this if statement at all
            PlayFabSettings.staticSettings.TitleId = "2BDAF";
        }

        //        var guid = Guid.NewGuid();
        var request = new LoginWithCustomIDRequest { CustomId = "GB//Lesson3", CreateAccount = true };
        if (!PlayFabClientAPI.IsClientLoggedIn())
        {
            var guid = Guid.NewGuid();
            PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
            tempName = request.CustomId;
        }
        else Debug.LogError("u already loggened  with: " + tempName);
    }

    public void DisconnectPlayFab()
    {
        PlayFabClientAPI.ForgetAllCredentials();
        Debug.LogError("PlayFabClientAPI.ForgetAllCredentials();");
        obgForTextComponent.GetComponent<TMP_Text>().text = "fail to connect";
        obgForTextComponent.GetComponent<TMP_Text>().color = new Color32(147, 23, 27, 255);
        LetsTakeAboutPlayFabConnection();
    }
    [SerializeField]
    string _username;
    [SerializeField]
    string _pass;
    public void SignIn()
    {
        if (!PlayFabClientAPI.IsClientLoggedIn())
        {
            PlayFabClientAPI.LoginWithPlayFab(
                new LoginWithPlayFabRequest
            {
                Username = _username, 
                Password = _pass,
            }, result =>
            {
                Debug.Log($"Success: {_username}");
                obgForTextComponent.GetComponent<TMP_Text>().text = "Connected";
                obgForTextComponent.GetComponent<TMP_Text>().color = new Color32(8, 130, 0, 255);
                LetsTakeAboutPlayFabConnection();
            }, error =>
           {
               Debug.LogError($"Fail: {error.ErrorMessage}");
               obgForTextComponent.GetComponent<TMP_Text>().text = "fail to connect";
               obgForTextComponent.GetComponent<TMP_Text>().color = new Color32(147, 23, 27, 255);
               LetsTakeAboutPlayFabConnection();
           },
           tempName = _username);
        }
        else Debug.LogError("u already loggened with: " + tempName);
    }









    [SerializeField] private GameObject _nameLabel;
    [SerializeField] private GameObject _emailLabel;
    [SerializeField] private GameObject _createdInfoLabel;
    private void LetsTakeAboutPlayFabConnection()
    {
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), OnGetAccountInfo, OnFailure);
    }
    private void OnGetAccountInfo(GetAccountInfoResult result)
    {
        _nameLabel.GetComponent<TMP_Text>().text = $"{result.AccountInfo.PlayFabId}";
        _emailLabel.GetComponent<TMP_Text>().text = $"{result.AccountInfo.Username}";
        _createdInfoLabel.GetComponent<TMP_Text>().text = $"{result.AccountInfo.Created}";
    }
    private void OnFailure(PlayFabError error)
    {
        var errorMessage = error.GenerateErrorReport();
        Debug.LogError("something went wrong: " + errorMessage);
    }

}

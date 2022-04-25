using System;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayFabLogin : MonoBehaviour
{
     private const string AutorizationIdKey = "Autorization_Id_Key";

     private string _guid; 
     
     public void Start()
     {
          if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
               PlayFabSettings.staticSettings.TitleId = "12D7F";

          var isNeedCreate = PlayerPrefs.HasKey(AutorizationIdKey);
          _guid = PlayerPrefs.GetString(AutorizationIdKey, Guid.NewGuid().ToString());
          
          var request = new LoginWithCustomIDRequest { CustomId = _guid, CreateAccount = !isNeedCreate };
          
          PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
     }
     private void OnLoginSuccess(LoginResult result)
     {
          Debug.Log($"Congratulations, you made successful API call! ID = {_guid}");
          PlayerPrefs.SetString(AutorizationIdKey, _guid);
          
          SceneManager.LoadScene("Lobby");
     }
     
     private void OnLoginFailure(PlayFabError error)
     {
          var errorMessage = error.GenerateErrorReport();
          Debug.LogError($"Something went wrong: {errorMessage}");
     }
    
}

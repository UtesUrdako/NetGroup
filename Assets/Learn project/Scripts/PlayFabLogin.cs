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
          // Here we need to check whether TitleId property is configured in settings or not
          if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
          {
               /*
               * If not we need to assign it to the appropriate variable manually
               * Otherwise we can just remove this if statement at all
               */
               PlayFabSettings.staticSettings.TitleId = "12D7F";
          }

          var isNeedCreate = PlayerPrefs.HasKey(AutorizationIdKey);
          _guid = PlayerPrefs.GetString(AutorizationIdKey, Guid.NewGuid().ToString());
          var request = new LoginWithCustomIDRequest { CustomId = _guid, CreateAccount = !isNeedCreate };
          PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
     }
     private void OnLoginSuccess(LoginResult result)
     {
          Debug.Log($"Congratulations, you made successful API call! ID = {_guid}");
          PlayerPrefs.SetString(AutorizationIdKey, _guid);
          
          PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(),
          result =>
          {
               Debug.Log($"PlayFab ID: {result.AccountInfo.PlayFabId}");
               Social.localUser.Authenticate(resultSocial =>
               {
                    if (resultSocial)
                         result.AccountInfo.GoogleInfo.GoogleId = Social.Active.localUser.id;
               });
          }, error =>
          {
               var errorMessage = error.GenerateErrorReport();
               Debug.LogError($"Something went wrong: {errorMessage}");
          });
          SceneManager.LoadScene(1);
     }
     
     private void OnLoginFailure(PlayFabError error)
     {
          var errorMessage = error.GenerateErrorReport();
          Debug.LogError($"Something went wrong: {errorMessage}");
     }
    
}

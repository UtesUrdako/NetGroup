using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class PlayFabLogin : MonoBehaviour
{
    // here we need to check whether TitledId property is configured in setting or not
    void Start()
    {
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
        {
            // if not we need to assign it to the appropriate variable manually
            //otherwise we can just remove this if statement at all
            PlayFabSettings.staticSettings.TitleId = "2BDAF";
        }

        var request = new LoginWithCustomIDRequest { CustomId = "GB//Lesson3", CreateAccount = true };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
    }
  
    private void OnLoginSuccess(LoginResult result)
    { Debug.Log("Congratulation, you made successful API call!"); }
    private void OnLoginFailure(PlayFabError error)
    {
        var errorMessage = error.GenerateErrorReport();
        Debug.LogError($"Something went wrong: {errorMessage}");
    }

    private void ConnectPlayFab()
    {
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
        {
            // if not we need to assign it to the appropriate variable manually
            //otherwise we can just remove this if statement at all
            PlayFabSettings.staticSettings.TitleId = "2BDAF";
        }

        var request = new LoginWithCustomIDRequest { CustomId = "GB//Lesson3", CreateAccount = true };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PlayFab;

public class AutorizationMenu : MonoBehaviour
{
    [Header("registrarion")]
    private string _userName;
    private string _password;
    private string _userEmail;
    [SerializeField] private TMP_InputField _userNameInputFiels;
    [SerializeField] private Button _createAccButton;

    public void SetUserName(string userName) => _userName = userName;
    public void SetUserPass(string userPass) => _password = userPass;
    public void SetUserEnail(string userMail) => _userEmail = userMail;

    private void Awake()
    {
        _userNameInputFiels.onEndEdit.AddListener(SetUserName);
        _createAccButton.onClick.AddListener(CreateNewAccount);
    }

    [SerializeField]
    GameObject _registrationWindow;
    public void OpenRegisterWindow()
    {
        //  transform.gameObject.SetActive(false);
        _registrationWindow.SetActive(true);
    }
    public void CloseRegisterWindow()
    {
        //  transform.gameObject.SetActive(false);
        _registrationWindow.SetActive(false);
    }
    //xfsdad@asd.ru
    //asdad
    //asdasd
    public void CreateNewAccount()
    {
        PlayFabClientAPI.RegisterPlayFabUser(new PlayFab.ClientModels.RegisterPlayFabUserRequest
        {
            Username = _userName,
            Email = _userEmail,
            Password = _password,
            RequireBothUsernameAndEmail = true
        },result =>
        {
            Debug.Log("new user with name: " + result.Username + " || registered");
        }, error =>
        {
            Debug.LogError("something whent wrong " + error.ErrorMessage); 
        }
        );
        _registrationWindow.SetActive(false);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;

public class AutorisationMenu : MonoBehaviour
{
    [Header("Registration")]
    [SerializeField] private InputField _userNameInputField;
    [SerializeField] private Button _registrationButton;
    
    [Header("Autorisation")]
    [SerializeField] private InputField _userNameSignInInputField;
    [SerializeField] private InputField _userPasswordSignInInputField;
    [SerializeField] private Button _signInButton;
    
    private string _userName;
    private string _userMail;
    private string _userPassword;

    private void Awake()
    {
        _userNameInputField.onEndEdit.AddListener(SetUserName);
        _registrationButton.onClick.AddListener(RegistrationAccount);
        
        _userNameSignInInputField.onEndEdit.AddListener(SetUserName);
        _userPasswordSignInInputField.onEndEdit.AddListener(SetUserPassword);
        _signInButton.onClick.AddListener(SignIn);
    }

    public void SetUserName(string userName) => _userName = userName;
    public void SetUserMail(string userMail) => _userMail = userMail;
    public void SetUserPassword(string userPassword) => _userPassword = userPassword;

    private void RegistrationAccount()
    {
        PlayFabClientAPI.RegisterPlayFabUser(new RegisterPlayFabUserRequest
        {
            Username = _userName,
            Email = _userMail,
            Password = _userPassword,
            RequireBothUsernameAndEmail = true
        }, result =>
        {
            Debug.Log($"Success: {result.Username}");
        }, error =>
        {
            Debug.LogError($"Fail: {error.ErrorMessage}\n{error.ErrorDetails}");
        });
    }

    private void SignIn()
    {
        PlayFabClientAPI.LoginWithPlayFab(new LoginWithPlayFabRequest
        {
            Username = _userName,
            Password = _userPassword
        }, result =>
        {
            Debug.Log($"Success: {_userName}");
        }, error =>
        {
            Debug.LogError($"Fail: {error.ErrorMessage}\n{error.ErrorDetails}");
        });
    }
}

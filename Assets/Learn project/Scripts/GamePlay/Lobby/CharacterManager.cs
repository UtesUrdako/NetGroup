using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour
{
    private const string GOLD = "GD";
    
    [SerializeField] private GameObject _enterNamePanel;
    [SerializeField] private Button _createNewCharacter;
    private string _characterName;

    [Space(20)]
    [SerializeField] private Button[] _addCharacters;
    //[SerializeField] private Button _addCharacter2;
    [SerializeField] private Button[] _useCharacters;
    //[SerializeField] private Button _useCharacter2;

    private void Start()
    {
        UpdateCharacterSlots();
        
        foreach (var character in _addCharacters)
            character.onClick.AddListener(() => _enterNamePanel.SetActive(true));
        foreach (var character in _useCharacters)
            character.onClick.AddListener(() => PhotonNetwork.LoadLevel(1));
        
        _createNewCharacter.onClick.AddListener(GetCharacterTokens);
    }

    public void SetCharacterName(string newName)
    {
        _characterName = newName;
    }

    private void GetCharacterTokens()
    {
        _createNewCharacter.interactable = false;
        PlayFabClientAPI.GetStoreItems(new GetStoreItemsRequest
        {
            CatalogVersion = "0.1",
            StoreId = "Characters_store"
        }, result =>
        {
            CreateNewCharacter(result.Store[0]);
        }, Error);
    }

    private void CreateNewCharacter(StoreItem item)
    {
        PlayFabClientAPI.PurchaseItem(new PurchaseItemRequest
        {
            ItemId = item.ItemId,
            Price = (int)item.VirtualCurrencyPrices[GOLD],
            VirtualCurrency = GOLD
        }, result =>
        {
            foreach (var item in result.Items)
            {
                CreateCharacterWithItemId(item.ItemId);
                return;
            }
        }, Error);
    }

    private void CreateCharacterWithItemId(string itemId)
    {
        PlayFabClientAPI.GrantCharacterToUser(new GrantCharacterToUserRequest
        {
            CharacterName = _characterName,
            ItemId = itemId
        }, result =>
        {
            Debug.Log($"Get character type: {result.CharacterType}");

            UpdateCharacterStatistics(result.CharacterId);
        }, Error);
    }

    private void UpdateCharacterStatistics(string characterId)
    {
        PlayFabClientAPI.UpdateCharacterStatistics(new UpdateCharacterStatisticsRequest
        {
            CharacterId = characterId,
            CharacterStatistics = new Dictionary<string, int>
            {
                {"Level", 1},
                {"Exp", 0},
                {"Gold", 0}
            }
        }, result =>
        {
            _enterNamePanel.SetActive(false);
            _createNewCharacter.interactable = true;
            
            UpdateCharacterSlots();
        }, Error);
    }

    private void UpdateCharacterSlots()
    {
        PlayFabClientAPI.GetAllUsersCharacters(new ListUsersCharactersRequest(),
            result =>
            {
                Debug.Log($"Count Characters: {result.Characters.Count}");

                foreach (var character in _addCharacters)
                    character.gameObject.SetActive(true);
                foreach (var character in _useCharacters)
                    character.gameObject.SetActive(false);

                for (int i = 0; i < result.Characters.Count; i++)
                {
                    _addCharacters[i].gameObject.SetActive(false);
                    _useCharacters[i].gameObject.SetActive(true);
                    _useCharacters[i].transform.GetChild(0).GetComponent<Text>().text =
                        result.Characters[i].CharacterName;
                    UpdateCharacterView(result.Characters[i].CharacterId, i);
                }
            }, Debug.LogError);
    }

    public void UpdateCharacterView(string characterId, int numberSlots)
    {
        PlayFabClientAPI.GetCharacterStatistics(new GetCharacterStatisticsRequest
        {
            CharacterId = characterId
        }, result =>
        {
            _useCharacters[numberSlots].transform.GetChild(1).GetComponent<Text>().text =
                result.CharacterStatistics["Level"].ToString();
        }, Debug.LogError);
    }

    private void Error(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
        _createNewCharacter.interactable = true;
    }
}

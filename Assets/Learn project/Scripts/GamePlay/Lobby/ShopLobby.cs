using System;
using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;

public class ShopLobby : MonoBehaviour
{
    public List<Sprite> icons;
    
    [SerializeField] private ShopUIItem _shopItemPrefab;
    [SerializeField] private Transform _contentShop;
    [SerializeField] private InventoryLobby _inventory;
    [SerializeField] private Button _shopButton;

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => CatalogManager.Instance.isLoaded);
        PlayFabClientAPI.GetStoreItems(new GetStoreItemsRequest
        {
            CatalogVersion = "0.1",
            StoreId = "ls1"
        }, result =>
        {
            foreach (var item in result.Store)
            {
                Instantiate(_shopItemPrefab, _contentShop).SetUseInventoryItem(_inventory.UpdateInventory, CatalogManager.Instance[item.ItemId], icons);
                Debug.Log($"Item: {CatalogManager.Instance[item.ItemId].DisplayName}");
            }
            gameObject.SetActive(false);
            _inventory.gameObject.SetActive(false);
            _shopButton.onClick.AddListener(() =>
            {
                gameObject.SetActive(!gameObject.activeSelf);
                _inventory.gameObject.SetActive(!_inventory.gameObject.activeSelf);
            });
        }, Debug.LogError);
    }

    public void UpdateInventory()
    {
        _inventory.UpdateInventory();
    }
}

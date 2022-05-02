using System;
using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class ShopLobby : MonoBehaviour
{
    public List<Sprite> icons;
    
    [SerializeField] private ShopUIItem _shopItemPrefab;
    [SerializeField] private Transform _contentShop;

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
                Instantiate(_shopItemPrefab, _contentShop).SetShopItem(CatalogManager.Instance[item.ItemId], icons);
                Debug.Log($"Item: {CatalogManager.Instance[item.ItemId].DisplayName}");
            }
        }, Debug.LogError);
    }
}

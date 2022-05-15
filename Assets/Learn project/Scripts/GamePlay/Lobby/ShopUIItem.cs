using System;
using System.Collections;
using System.Collections.Generic;
using PlayFab;
using UnityEngine;
using UnityEngine.UI;
using PlayFab.ClientModels;

public class ShopUIItem : UIItem
{
    public override void SetShopItem(Action action, CatalogItem item, List<Sprite> icons)
    {
        base.SetShopItem(action, item, icons);
        
        GetComponent<Button>().onClick.AddListener(() =>
        {
            PlayFabClientAPI.PurchaseItem(new PurchaseItemRequest
            {
                CatalogVersion = "0.1",
                ItemId = _item.ItemId,
                Price = (int)_price.Value,
                VirtualCurrency = _price.Key
            }, result =>
            {
                _action?.Invoke();
            }, Debug.LogError);
        });
    }
}

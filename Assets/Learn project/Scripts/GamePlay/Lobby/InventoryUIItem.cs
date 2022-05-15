using System;
using System.Collections;
using System.Collections.Generic;
using PlayFab;
using UnityEngine;
using UnityEngine.UI;
using PlayFab.ClientModels;

public sealed class InventoryUIItem : UIItem
{
    public override void SetShopItem(Action action, CatalogItem item, List<Sprite> icons)
    {
        base.SetShopItem(action, item, icons);
        
        GetComponent<Button>().onClick.AddListener(() =>
        {
            switch (_item.Tags[0])
            {
                case "prize":
                case "box":
                    PlayFabClientAPI.UnlockContainerItem(new UnlockContainerItemRequest() {
                        CatalogVersion = _item.CatalogVersion,
                        ContainerItemId = _item.ItemId
                    }, result =>
                    {
                        _action?.Invoke();
                    }, Debug.LogError);
                    break;
                default:
                    PlayFabClientAPI.ConsumeItem(new ConsumeItemRequest()
                    {
                        ConsumeCount = 1,
                        ItemInstanceId = _item.ItemId
                    }, result =>
                    {
                        _action?.Invoke();
                    }, Debug.LogError);
                    break;
            }
            
        });
    }
}

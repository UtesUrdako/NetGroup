using System;
using System.Collections;
using System.Collections.Generic;
using PlayFab;
using UnityEngine;
using UnityEngine.UI;
using PlayFab.ClientModels;

public sealed class InventoryUIItem : UIItem
{
    public ItemInstance _inventoryItem;
    
    public override void SetUseInventoryItem(Action action, CatalogItem item, List<Sprite> icons)
    {
        base.SetUseInventoryItem(action, item, icons);
        
        GetComponent<Button>().onClick.AddListener(() =>
        {
            if (_item.Tags.Count > 0)
            {
                switch (_item.Tags[0])
                {
                    case "prize":
                    case "box":
                        PlayFabClientAPI.UnlockContainerItem(new UnlockContainerItemRequest()
                        {
                            CatalogVersion = _item.CatalogVersion,
                            ContainerItemId = _item.ItemId
                        }, result => { _action?.Invoke(); }, Debug.LogError);
                        return;
                }
            }
            PlayFabClientAPI.ConsumeItem(new ConsumeItemRequest()
            {
                ConsumeCount = 1,
                ItemInstanceId = _inventoryItem.ItemInstanceId
            }, result =>
            {
                _action?.Invoke();
            }, Debug.LogError);
            
        });
    }
}

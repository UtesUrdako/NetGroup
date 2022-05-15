using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;

public class InventoryLobby : MonoBehaviour
{
    public List<Sprite> icons;
        
    [SerializeField] private InventoryUIItem _shopItemPrefab;
    [SerializeField] private Transform _contentShop;
    [SerializeField] private TMP_Text _currencyText;

    private List<UIItem> _items;

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => CatalogManager.Instance.isLoaded);
        _items = new List<UIItem>();
        UpdateInventory();
    }

    public void UpdateInventory()
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), result =>
        {
            foreach (var item in _items)
            {
                Destroy(item.gameObject);
            }
            _items.Clear();
            foreach (var item in result.Inventory)
            {
                _items.Add(Instantiate(_shopItemPrefab, _contentShop));
                    _items.Last().SetShopItem(UpdateInventory, CatalogManager.Instance[item.ItemId], icons);
                Debug.Log($"Item: {CatalogManager.Instance[item.ItemId].DisplayName}");
            }

            _currencyText.text = "";
            foreach (var item in result.VirtualCurrency)
            {
                _currencyText.text += $"{item.Key} {item.Value}/";
            }
        }, Debug.LogError);
    }
}

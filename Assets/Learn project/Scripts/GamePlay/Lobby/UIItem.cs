using System;
using System.Collections;
using System.Collections.Generic;
using PlayFab;
using UnityEngine;
using UnityEngine.UI;
using PlayFab.ClientModels;

public class UIItem : MonoBehaviour
{
    [SerializeField] protected Text _nameItemText;
    [SerializeField] protected Text _priceItemText;
    [SerializeField] protected Text _idItemText;
    [SerializeField] protected Image _iconItemImage;

    protected CatalogItem _item;
    protected KeyValuePair<string, uint> _price;
    protected Action _action;

    public virtual void SetUseInventoryItem(Action action, CatalogItem item, List<Sprite> icons)
    {
        _action = action;
        _item = item;
        foreach (var kv in _item.VirtualCurrencyPrices)
        {
            _price = kv;
            break;
        }
        
        _nameItemText.text = item.DisplayName;
        _priceItemText.text = "GD " + item.VirtualCurrencyPrices["GD"];
        _idItemText.text = item.ItemId;
        _iconItemImage.sprite = GetIconToItem(item.ItemId, icons);
    }

    protected Sprite GetIconToItem(string id, List<Sprite> icons)
    {
        switch (id)
        {
            case "cnt1":
                return icons[0];
            case "key":
                return icons[1];
            case "bnd1":
                return icons[2];
            case "s_h_p":
                return icons[3];
            case "m_h_p":
                return icons[4];
        }

        return default;
    }
}

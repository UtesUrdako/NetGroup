using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab.ClientModels;

public class ShopUIItem : MonoBehaviour
{
    [SerializeField] private Text _nameItemText;
    [SerializeField] private Text _priceItemText;
    [SerializeField] private Text _idItemText;
    [SerializeField] private Image _iconItemImage;

    public void SetShopItem(CatalogItem item, List<Sprite> icons)
    {
        _nameItemText.text = item.DisplayName;
        _priceItemText.text = "GD " + item.VirtualCurrencyPrices["GD"];
        _idItemText.text = item.ItemId;
        _iconItemImage.sprite = GetIconToItem(item.ItemId, icons);

    }

    private Sprite GetIconToItem(string id, List<Sprite> icons)
    {
        switch (id)
        {
            case "cnt1":
                return icons[0];
        }

        return default;
    }
}

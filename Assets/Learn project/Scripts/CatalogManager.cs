using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEditor;
using UnityEngine;

public class CatalogManager : MonoBehaviour
{
    private Dictionary<string, CatalogItem> _catalog = new Dictionary<string, CatalogItem>();

    void Start()
    {
        PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest(), ResultCallback, error =>
        {
            var errorMessage = error.GenerateErrorReport();
            Debug.LogError($"Something went wrong: {errorMessage}");
        });
    }

    private void ResultCallback(GetCatalogItemsResult obj)
    {
        HandleCatalog(obj.Catalog);
        Debug.Log("Catalog was loaded successfully!");
    }

    private void HandleCatalog(List<CatalogItem> catalog)
    {
        foreach (var catalogItem in catalog)
        {
            _catalog.Add(catalogItem.ItemId, catalogItem);
            Debug.Log($"Catalog item \"{catalogItem.DisplayName}\" was added!");
        }
    }

    public CatalogItem GetCatalogItem(string key)
    {
        if (_catalog.ContainsKey(key))
            return _catalog[key];
        return null;
    }
}

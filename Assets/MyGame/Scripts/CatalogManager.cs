using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatalogManager : MonoBehaviour
{
    [SerializeField]
    GameObject _catalogWindow;
    [SerializeField]
    GameObject _catalogWindowScrollContent;

    private void GetCatalogFromPlayfab()
    {
        PlayFabClientAPI.GetCatalogItems(new PlayFab.ClientModels.GetCatalogItemsRequest(), ResultCallback, error =>
        {
            var errorMessage = error.GenerateErrorReport();
            Debug.LogError($"error like this - { errorMessage}");
        }
);
    }
    public void OpenCatalogWindow()
    {
        _catalogWindow.SetActive(true);
        GetCatalogFromPlayfab();
    }
    public void CloseCatalogWindow()
    {      
        _catalogWindow.SetActive(false);
    }

    private void ResultCallback(GetCatalogItemsResult obg)
    {
        HandleCatalog(obg.Catalog);
        Debug.Log("catalog was loaded");
    }
    private List<CatalogItem> playerBag;
    string[] propertiesForItem = new string[2];
    private void HandleCatalog(List<CatalogItem> catalog)
    {
        foreach (var catalogItem in catalog)
        {
            playerBag.Add(catalogItem);
            Debug.Log($"Catalog item \"{catalogItem.DisplayName}\" loaded");
        }
    }

    private void FixedUpdate()
    {
        if (playerBag != null && _catalogWindow.activeInHierarchy)
        {
            foreach (var item in playerBag)
            {
              //  Instantiate(item, _catalogWindowScrollContent.transform.position, Quaternion.identity, _catalogWindowScrollContent.transform);
            }
        }
    }
}

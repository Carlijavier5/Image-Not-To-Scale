using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebsiteManager : MonoBehaviour 
{
    [SerializeField] private WebItem[] _webItems;
    [SerializeField] private GameObject _shoppingPage;
    [SerializeField] private ItemBuyPage _itemBuyPage;
    private Canvas _canvas;

    // TESTING ONLY
    public WebsiteItem[] _SOItems;

    void Start() {
        _canvas = GetComponent<Canvas>();

        // TESTING ONLY
        OpenWebsite(_SOItems);
    }

    public void OpenWebsite(WebsiteItem[] items) {
        _canvas.enabled = true;
        UpdateWebItems(items);
    }

    /// <summary>
    /// Update the items in the scroll list
    /// </summary>
    private void UpdateWebItems(WebsiteItem[] items) {
        for (int i = 0; i < items.Length; i++) {
            _webItems[i].LoadWebItem(items[i], this);
        }
    }

    // managing the behaviors of clicking on an item to buy
    public void OnItemEnter(WebItem item) {
        
    }

    public void OnItemExit(WebItem item) {

    }

    public void OnItemSelected(WebItem item) {
        _itemBuyPage.gameObject.SetActive(true);
        _shoppingPage.gameObject.SetActive(false);
        _itemBuyPage.OpenPage(item.GetItemData());
    }
}
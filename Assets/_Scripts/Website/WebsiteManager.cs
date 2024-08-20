using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebsiteManager : MonoBehaviour {

    public event System.Action<List<(WebsiteItem, float)>> OnSaleEnd;

    [SerializeField] private WebThing webThing;
    private List<(WebsiteItem, float)> buyList;
    private bool awaitingBuy;

    public void OpenWebsite(WebsiteItem[] items) {
        StartCoroutine(ShowItems(items));
    }

    private IEnumerator ShowItems(WebsiteItem[] items) {
        for (int i = 0; i < items.Length; i++) {
            webThing.SetupItem(items[i]);
            awaitingBuy = true;
            while (awaitingBuy) yield return null;
        }
        OnSaleEnd?.Invoke(buyList);
        buyList = null;
    }

    public void ConfirmBuy() {
        awaitingBuy = false;
        buyList.Add(webThing.ReadValue());
    }
}

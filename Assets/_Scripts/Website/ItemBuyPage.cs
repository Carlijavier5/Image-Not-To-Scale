using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemBuyPage : MonoBehaviour
{
    //[SerializeField] private GameObject _itemBuyPage;
    [SerializeField] private Image _itemImage;
    [SerializeField] private TextMeshProUGUI _itemName;
    [SerializeField] private TextMeshProUGUI _itemDescription;

    public void OpenPage(WebsiteItem itemData) {
        //_itemBuyPage.SetActive(true);
        PopulateData(itemData);
    }

    private void PopulateData(WebsiteItem itemData) {
        _itemImage.sprite = itemData.websitePNG;
        _itemName.text = itemData.name;
        _itemDescription.text = itemData.description;
    }
}

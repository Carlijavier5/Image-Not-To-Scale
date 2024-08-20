using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class WebItem : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler {
    [SerializeField] private Image _itemImage;
    [SerializeField] private TextMeshProUGUI _itemName;
    private WebsiteItem _itemData;
    public Image _thisImage;

    private WebsiteManager _siteManager;

    void Start() { _thisImage = this.gameObject.GetComponent<Image>(); }

    public void LoadWebItem(WebsiteItem webItemData, WebsiteManager manager) {
        _itemImage.sprite = webItemData.websitePNG;
        _itemName.text = webItemData.productName;
        _siteManager = manager;
        _itemData = webItemData;
    }

    public void OnPointerClick(PointerEventData eventData) {
        _siteManager.OnItemSelected(this);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        _siteManager.OnItemEnter(this);
    }

    public void OnPointerExit(PointerEventData eventData) {
        _siteManager.OnItemExit(this);
    }

    public WebsiteItem GetItemData() {
        return _itemData;
    }
}

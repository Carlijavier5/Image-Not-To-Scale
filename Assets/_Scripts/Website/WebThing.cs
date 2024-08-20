﻿using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WebThing : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private Sprite inch, mouse, eagle;
    [SerializeField] private Image furnitureImage;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private TextMeshProUGUI sliderNumber;
    [SerializeField] private Image unitImage;
    [SerializeField] private Slider slider;
    private WebsiteItem item;

    public void SetNum() {
        sliderNumber.text = $"{slider.value}";
    }

    public void SetupItem(WebsiteItem webItem) {
        item = webItem;
        itemName.text = webItem.productName;
        furnitureImage.sprite = webItem.websitePNG;
        unitImage.sprite = webItem.scaleUnit switch { ScaleUnit.Inches => inch, ScaleUnit.Mice => mouse, _ => eagle };
        description.text = $"How many {webItem.scaleUnit switch { ScaleUnit.Inches => "inches", ScaleUnit.Mice => "mice", _ => "eagles" }} should your {webItem.productName} be?";
        slider.wholeNumbers = webItem.wholeNumbers;
        slider.maxValue = webItem.minMax.x;
        slider.maxValue = webItem.minMax.y;
    }

    public (WebsiteItem, float) ReadValue() {
        return (item, 0);
    }
}
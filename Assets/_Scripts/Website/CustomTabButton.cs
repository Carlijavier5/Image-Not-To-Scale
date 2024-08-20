using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class CustomTabButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    [SerializeField] private TabGroup _tabGroup;
    
    private Image _background;
    
    void Start() {
        _background = GetComponent<Image>();
        _tabGroup.Subscribe(this);
    }

    public void OnPointerClick(PointerEventData eventData) {
        _tabGroup.OnTabSelected(this);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        _tabGroup.OnTabEnter(this);
    }

    public void OnPointerExit(PointerEventData eventData) {
        _tabGroup.OnTabExit(this);
    }

    public void ChangeBackground(Sprite i) {
        _background.sprite = i;
    }
}

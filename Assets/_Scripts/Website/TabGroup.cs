using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manager for swapping between tabs
/// </summary>
public class TabGroup : MonoBehaviour
{
    private List<CustomTabButton> _tabButtons = new List<CustomTabButton>();
    [SerializeField] private List<GameObject> _pages = new List<GameObject>();
    [SerializeField] private Sprite _tabIdle;
    [SerializeField] private Sprite _tabHover;
    [SerializeField] private Sprite _tabActive;

    private CustomTabButton _currTab;

    public void Subscribe(CustomTabButton button) {
        _tabButtons.Add(button);
    }

    public void OnTabEnter(CustomTabButton button) {
        ResetTabs();
        if (_currTab == null || button != _currTab) {
            button.ChangeBackground(_tabHover);
        }
    }

    public void OnTabExit(CustomTabButton button) {
        ResetTabs();
    }

    public void OnTabSelected(CustomTabButton button) {
        _currTab = button;
        ResetTabs();
        button.ChangeBackground(_tabActive);

        // change pages
        int index = button.transform.GetSiblingIndex();
        for (int i = 0; i < _pages.Count; i++) {    // jank bitch loop 
            if (i == index) { _pages[i].SetActive(true); }
            else { _pages[i].SetActive(false); }
        }
    }

    private void ResetTabs() {
        foreach (CustomTabButton b in _tabButtons) {
            if (_currTab != null && b == _currTab) { continue; }
            b.ChangeBackground(_tabIdle);
        }
    }
}

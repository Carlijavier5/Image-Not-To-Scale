using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureManager : MonoBehaviour {

    public event System.Action OnShowcaseEnd;

    [SerializeField] private GameObject[] models;
    [SerializeField] private WebsiteItem[] websiteItems;
    [SerializeField] private AnimationCurve growthCurveXZ, growthCurveY;
    private Dictionary<WebsiteItem, GameObject> furnitureDict;

    void Awake() {
        if (models == null || websiteItems == null) {
            Debug.LogError("Models or Items array was null");
            enabled = false;
            return;
        } else if (models.Length != websiteItems.Length) {
            Debug.LogError("Model and Item length differ");
            enabled = false;
            return;
        }

        furnitureDict = new();
        for (int i = 0; i < models.Length; i++) {
            furnitureDict[websiteItems[i]] = models[i];
            models[i].transform.localScale = Vector3.zero;
        }
    }

    public void BuyItems((WebsiteItem, float)[] items) {
        StartCoroutine(AnimateObjectSpawn(items));
    }

    private IEnumerator AnimateObjectSpawn((WebsiteItem, float)[] items) {
        for (int i = 0; i < items.Length; i++) {
            GameObject go = furnitureDict[items[i].Item1];
            go.transform.localScale = Vector3.zero;
            go.SetActive(true);
            Transform t = go.transform;

            float lerpVal = 0;
            while (lerpVal < 1) {
                lerpVal = Mathf.MoveTowards(lerpVal, 1, Time.deltaTime);
                t.localScale = new Vector3(growthCurveXZ.Evaluate(lerpVal),
                                           growthCurveY.Evaluate(lerpVal),
                                           growthCurveXZ.Evaluate(lerpVal)) * items[i].Item2;
                yield return null;
            }
        } OnShowcaseEnd?.Invoke();
    }
}

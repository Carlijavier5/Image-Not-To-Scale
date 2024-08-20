using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadingMotion : MonoBehaviour {

    public event System.Action OnLoadingDone;

    [SerializeField] private RectTransform loadingElements,
                                           loadingText,
                                           loadingIcon;
    [SerializeField] private TextMeshProUGUI textMesh;
    [SerializeField] private float dotTickInterval;
    [SerializeField] private float growthMult;
    [SerializeField] private float oscillationAmp;

    private Vector2 textAnchor;
    private float lerpVal;
    private bool loadingInProgress;

    private int dots = 0;
    private float tickTimer;

    void Awake() {
        textAnchor = loadingElements.anchoredPosition;
        loadingIcon.localScale = Vector2.zero;
        loadingText.localScale = Vector2.zero;
        loadingElements.gameObject.SetActive(false);
    }

    void Update() {
        if (loadingInProgress) {
            loadingElements.anchoredPosition = Vector2.MoveTowards(loadingElements.anchoredPosition, textAnchor
                                                       + new Vector2(Mathf.Sin(Time.time), Mathf.Cos(Time.time / 1.72f)) * oscillationAmp,
                                                       Time.deltaTime * 10);
            tickTimer += Time.deltaTime;
            if (tickTimer > dotTickInterval) {
                tickTimer = 0;
                textMesh.text = "Loading " + (dots switch { 0 => "", 1 => ".", 2 => ". .", _ => ". . ." });
                dots = (dots + 1) % 3;
            }
        }
    }

    private IEnumerator Loading(bool start) {
        if (start) {
            loadingElements.gameObject.SetActive(true);
            loadingInProgress = start;
            while (lerpVal < 2) {
                float res = lerpVal < 1 ? Mathf.Lerp(0, 1.3f, lerpVal)
                                        : Mathf.Lerp(1.3f, 1f, lerpVal - 1);
                loadingIcon.localScale = new Vector3(res, res, 1);
                lerpVal = Mathf.MoveTowards(lerpVal, 4, Time.deltaTime * growthMult);
                yield return null;
            } while (lerpVal < 4) {
                float res = lerpVal < 3 ? Mathf.Lerp(0, 1.1f, lerpVal - 2)
                                        : Mathf.Lerp(1.1f, 1f, lerpVal - 3);
                loadingText.localScale = new Vector3(res, res, 1);
                lerpVal = Mathf.MoveTowards(lerpVal, 4, Time.deltaTime * growthMult);
                yield return null;
            } 
        } else {
            while (lerpVal > 2) {
                loadingText.localScale = Vector3.MoveTowards(loadingText.localScale, Vector3.zero, Time.deltaTime * growthMult * 1.5f);
                if (loadingText.localScale == Vector3.zero) lerpVal = 1;
                yield return null;
            } while (lerpVal > 0) {
                loadingIcon.localScale = Vector3.MoveTowards(loadingIcon.localScale, Vector3.zero, Time.deltaTime * growthMult * 1.5f);
                if (loadingIcon.localScale == Vector3.zero) lerpVal = 0;
                yield return null;
            } yield return new WaitForSeconds(0.5f);

            loadingElements.gameObject.SetActive(false);
            
            OnLoadingDone?.Invoke();
            OnLoadingDone = null;
        }
    }

    public void InitiateLoading() {
        StopAllCoroutines();
        StartCoroutine(Loading(true));
    }

    public void FinishLoading() {
        StopAllCoroutines();
        StartCoroutine(Loading(false));
    }
}

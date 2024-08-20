using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour {

    /// <summary> Whether a fadeout happened; </summary>
    public event System.Action<bool> OnFadeChange;

    public static TransitionManager Instance { get; private set; }

    [SerializeField] private CanvasGroup cg;
    [SerializeField] private Image fadeBG;
    [SerializeField] private LoadingMotion loadingMotion;
    [SerializeField] private float transitionTime;

    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
            return;
        }

        fadeBG.fillAmount = 1;
        StartCoroutine(EnterMainMenu());
    }

    private IEnumerator EnterMainMenu() {
        loadingMotion.InitiateLoading();
        yield return new WaitForSeconds(2f);
        loadingMotion.OnLoadingDone += RevealMainMenu;
        loadingMotion.FinishLoading();
    }

    private void RevealMainMenu() => StartCoroutine(ICoverScreen(false));

    public void StartGame() => StartCoroutine(IStartGame());

    private IEnumerator IStartGame() {
        yield return StartCoroutine(ICoverScreen(true));
        AsyncOperation loadOP = SceneManager.LoadSceneAsync(1);
        loadingMotion.InitiateLoading();
        yield return new WaitForSeconds(4f);
        while (!loadOP.isDone) {
            yield return null;
        } loadingMotion.OnLoadingDone += BeginGame;
        loadingMotion.FinishLoading();
    }

    private void BeginGame() => StartCoroutine(IUnfadeFB());

    public void FadeFB(bool fadeout) {
        if (fadeout) StartCoroutine(IFadeFB());
        else StartCoroutine(IUnfadeFB());
    }

    private IEnumerator IUnfadeFB() {
        while (cg.alpha > 0) {
            cg.alpha = Mathf.MoveTowards(cg.alpha, 0, Time.deltaTime * 0.4f);
            yield return null;
        } OnFadeChange?.Invoke(false);
        OnFadeChange = null;
    }

    private IEnumerator IFadeFB() {
        while (cg.alpha < 1) {
            cg.alpha = Mathf.MoveTowards(cg.alpha, 0, Time.deltaTime * 0.4f);
            yield return null;
        } OnFadeChange?.Invoke(true);
        OnFadeChange = null;
    }

    public void CoverScreen(bool fadeout) => StartCoroutine(ICoverScreen(fadeout));

    private IEnumerator ICoverScreen(bool fadeout) {
        cg.alpha = 1;
        float target = fadeout ? 1 : 0;
        fadeBG.fillClockwise = fadeout;
        if (fadeout) cg.blocksRaycasts = fadeout;
        while (fadeBG.fillAmount != target) {
            float delta = Time.deltaTime * (1 / (transitionTime == 0 ? 1 : transitionTime));
            fadeBG.fillAmount = Mathf.MoveTowards(fadeBG.fillAmount, target, delta);
            yield return null;
        }
        if (!fadeout) cg.blocksRaycasts = fadeout;
        OnFadeChange?.Invoke(fadeout);
    }
}
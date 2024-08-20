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
        StartCoroutine(CoverScreen(false));
    }

    public void StartGame() => StartCoroutine(IStartGame());

    private IEnumerator IStartGame() {
        yield return StartCoroutine(CoverScreen(true));
        AsyncOperation loadOP = SceneManager.LoadSceneAsync(1);
        loadingMotion.InitiateLoading();
        yield return new WaitForSeconds(4f);
        while (!loadOP.isDone) {
            yield return null;
        } loadingMotion.OnLoadingDone += BeginGame;
        loadingMotion.FinishLoading();
    }

    private void BeginGame() => StartCoroutine(CoverScreen(false));

    private IEnumerator CoverScreen(bool fadeout) {
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
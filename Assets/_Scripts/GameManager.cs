using System.Linq;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager Instance { get; private set; }

    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private WebsiteManager websiteManager;
    [SerializeField] private TutorialManager tutorialManager;
    [SerializeField] private FurnitureManager fm;
    [SerializeField] private WebsitePage[] pages;
    private (WebsiteItem, float)[] items;

    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
            return;
        }
    }

    public void StartGame() {
        StartCoroutine(IStartGame());
    }

    private IEnumerator IStartGame() {
        AudioManager.Instance.StopMusic();
        yield return new WaitForSeconds(1f);
        yield return new WaitForSeconds(AudioManager.Instance.PlaySFX("PhoneReverb"));
        yield return new WaitForSeconds(AudioManager.Instance.PlaySFX("PhoneReverb"));
        yield return new WaitForSeconds(AudioManager.Instance.PlaySFX("PhonePickup"));
        yield return new WaitForSeconds(0.25f);
        dialogueManager.DoDialogue(DialogueType.Start);
        dialogueManager.OnDialogueEnd += ToTutorial1;
    }

    private void ToTutorial1() {
        dialogueManager.OnDialogueEnd -= ToTutorial1;
        dialogueManager.gameObject.SetActive(false);
        TransitionManager.Instance.OnFadeChange += ToTutorial2;
        TransitionManager.Instance.CoverScreen(true);
    }

    private void ToTutorial2(bool _) {
        AudioManager.Instance.PlayMusic("ShopStart-NORMAL");
        TransitionManager.Instance.OnFadeChange -= ToTutorial2;
        tutorialManager.gameObject.SetActive(true);
        tutorialManager.OnTutorialEnd += ExitTutorial;
        TransitionManager.Instance.FadeFB(false);
    }

    private void ExitTutorial() {
        tutorialManager.OnTutorialEnd -= ExitTutorial;
        TransitionManager.Instance.OnFadeChange += ToWebsiteLvl1;
        TransitionManager.Instance.FadeFB(true);
    }

    private void ToWebsiteLvl1(bool _) {
        TransitionManager.Instance.OnFadeChange -= ToWebsiteLvl1;
        tutorialManager.gameObject.SetActive(false);
        websiteManager.gameObject.SetActive(true);
        TransitionManager.Instance.CoverScreen(false);
        websiteManager.OpenWebsite(pages[0].items);
        websiteManager.OnSaleEnd += WebsiteManager_OnSaleEnd;
    }

    private void WebsiteManager_OnSaleEnd(System.Collections.Generic.List<(WebsiteItem, float)> list) {
        items = list.ToArray();
        websiteManager.OnSaleEnd -= WebsiteManager_OnSaleEnd;
        TransitionManager.Instance.OnFadeChange += ShowRes1;
        TransitionManager.Instance.CoverScreen(true);
    }

    private void ShowRes1(bool _) {
        TransitionManager.Instance.OnFadeChange -= ShowRes1;
        fm = FindObjectOfType<FurnitureManager>();
        fm.BuyItems(items);
        websiteManager.gameObject.SetActive(false);
        TransitionManager.Instance.CoverScreen(false);
        fm.OnShowcaseEnd += Fm_OnShowcaseEnd;
    }

    private void Fm_OnShowcaseEnd() {
        AudioManager.Instance.StopMusic();
        dialogueManager.gameObject.SetActive(true);
        StartCoroutine(IFm_OnShowcaseEnd());
    }

    private IEnumerator IFm_OnShowcaseEnd() {
        fm.OnShowcaseEnd -= Fm_OnShowcaseEnd;
        yield return new WaitForSeconds(AudioManager.Instance.PlaySFX("PhoneNoReverb"));
        yield return new WaitForSeconds(AudioManager.Instance.PlaySFX("PhonePickup"));
        yield return new WaitForSeconds(0.5f);
        dialogueManager.DoDialogue(DialogueType.Final);
        dialogueManager.OnDialogueEnd += StartW;
    }

    private void StartW() {
        dialogueManager.OnDialogueEnd -= StartW;
        StartCoroutine(W());
    }

    private IEnumerator W() {
        yield return new WaitForSeconds(2);
        TransitionManager.Instance.OnFadeChange += Instance_OnFadeChange;
        StopAllCoroutines();
        TransitionManager.Instance.CoverScreen(true);
    }

    private void Instance_OnFadeChange(bool obj) {
        TransitionManager.Instance.OnFadeChange -= Instance_OnFadeChange;
        websiteManager.gameObject.SetActive(true);
        StopAllCoroutines();
        TransitionManager.Instance.CoverScreen(false);
        websiteManager.OpenWebsite(pages[1].items);
        AudioManager.Instance.PlayMusicWithPreamble("Shopping-Chaotic-Intro", "Shopping-Chaotic-Loopable");
        websiteManager.OnSaleEnd += WebsiteManager_OnSaleEnd2;
    }

    private void WebsiteManager_OnSaleEnd2(System.Collections.Generic.List<(WebsiteItem, float)> list) {
        items = list.ToArray();
        websiteManager.OnSaleEnd -= WebsiteManager_OnSaleEnd2;
        StopAllCoroutines();
        TransitionManager.Instance.OnFadeChange += ShowRes2;
        TransitionManager.Instance.CoverScreen(true);
    }

    private void ShowRes2(bool _) {
        TransitionManager.Instance.OnFadeChange -= ShowRes2;
        fm = FindObjectOfType<FurnitureManager>();
        fm.BuyItems(items);
        websiteManager.gameObject.SetActive(false);
        TransitionManager.Instance.CoverScreen(false);
        fm.OnShowcaseEnd += Fm_OnShowcaseEnd;
    }
}
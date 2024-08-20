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
        yield return new WaitForSeconds(1f);
        yield return new WaitForSeconds(AudioManager.Instance.PlaySFX("PhoneReverb"));
        yield return new WaitForSeconds(AudioManager.Instance.PlaySFX("PhoneReverb"));
        yield return new WaitForSeconds(AudioManager.Instance.PlaySFX("PhonePickup"));
        yield return new WaitForSeconds(1f);
        dialogueManager.DoDialogue(DialogueType.Start);
        dialogueManager.OnDialogueEnd += ToTutorial1;
    }

    private void ToTutorial1() {
        dialogueManager.OnDialogueEnd -= ToTutorial1;
        TransitionManager.Instance.OnFadeChange += ToTutorial2;
        TransitionManager.Instance.CoverScreen(true);
    }

    private void ToTutorial2(bool _) {
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
        fm.BuyItems(items);
        websiteManager.gameObject.SetActive(false);
        TransitionManager.Instance.CoverScreen(false);
    }
}
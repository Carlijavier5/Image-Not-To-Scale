using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager Instance { get; private set; }

    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private WebsiteManager websiteManager;
    [SerializeField] private TutorialManager tutorialManager;
    [SerializeField] private WebsitePage[] pages;

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
        TransitionManager.Instance.OnFadeChange += ToTutorial2;
        TransitionManager.Instance.CoverScreen(true);
    }

    private void ToTutorial2(bool _) {
        tutorialManager.gameObject.SetActive(true);
        tutorialManager.OnTutorialEnd += ExitTutorial;
        TransitionManager.Instance.FadeFB(false);
    }

    private void ExitTutorial() {
        TransitionManager.Instance.OnFadeChange += ToWebsiteLvl1;
        TransitionManager.Instance.FadeFB(true);
    }

    private void ToWebsiteLvl1(bool _) {
        websiteManager.gameObject.SetActive(true);
        TransitionManager.Instance.CoverScreen(false);
    }
}

public class TutorialManager : MonoBehaviour {

    public event System.Action OnTutorialEnd;

    [SerializeField] private Image tutImg;
    [SerializeField] private Sprite[] tutImgs;
    private int currIndex = 0;

    void Update() {
        if (Input.anyKeyDown) {
            currIndex++;
            if (currIndex < tutImgs.Length) {
                tutImg.sprite = tutImgs[currIndex];
            } else OnTutorialEnd?.Invoke();
        }
    }
}
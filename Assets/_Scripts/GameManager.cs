using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager Instance { get; private set; }

    [SerializeField] private DialogueManager dialogueManager;
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
    }
}
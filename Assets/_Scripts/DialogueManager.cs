using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum DialogueType { Start, Final }

public class DialogueManager : MonoBehaviour {

    public event System.Action OnDialogueEnd;

    private enum State { Idle, Writing, Awaiting }
    private State state;

    [SerializeField] private TextMeshProUGUI textMesh;
    [SerializeField] private RectTransform dialogueBox;
    [SerializeField] private DialogueData startDialogue, finalDialogue;
    [SerializeField] private RectTransform npcRect;
    [SerializeField] private Vector2 fontBounds;
    [SerializeField] private float textGrowthMult;
    [SerializeField] private float audioInterval;
    [SerializeField] private float anchorSpeed;

    private DialogueData currDialogue;
    private string currStr;

    private int currLine;
    private int currLetter;

    private bool[] growthSet;
    private float[] sizeSet;

    private float audioCD;

    private Vector2 nameYAnchor;
    private Vector2 targetAnchor = new Vector2(0.265f, 0.34f);

    void Awake() {
        dialogueBox.localScale = new Vector3(dialogueBox.localScale.x, 0, dialogueBox.localScale.z);
        npcRect.gameObject.SetActive(false);
        dialogueBox.gameObject.SetActive(false);
        nameYAnchor = new Vector2(npcRect.anchorMin.y, npcRect.anchorMax.y);
        ResetText();
    }

    void Update() {
        if (state == State.Writing
            && audioCD <= 0) PlayDialogueAudio();
        if (Input.anyKeyDown) {
            switch (state) {
                case State.Writing:
                    StopAllCoroutines();
                    textMesh.text = currStr;
                    state = State.Awaiting;
                    break;
                case State.Awaiting:
                    StopAllCoroutines();
                    if (LoadNextLine()) {
                        StartCoroutine(IPlayDialogue());
                    } else {
                        StartCoroutine(IEndDialogue());
                    } break;
            }
        }
        audioCD = Mathf.MoveTowards(audioCD, 0, Time.deltaTime);
    }

    private void PlayDialogueAudio() {
        string clipName = $"Mom{Random.Range(1, 13)}";
        AudioManager.Instance.PlaySFX(clipName, 0.25f);
        audioCD = audioInterval;
    }

    public void DoDialogue(DialogueType type) {
        dialogueBox.gameObject.SetActive(true);
        currDialogue = type switch { _ => startDialogue };
        currLine = -1;
        LoadNextLine();
        StartCoroutine(IBeginDialogue());
    }

    private IEnumerator IBeginDialogue() {
        while (dialogueBox.localScale.y < 1) {
            dialogueBox.localScale = new Vector3(dialogueBox.localScale.x,
                                                 Mathf.MoveTowards(dialogueBox.localScale.y, 1, Time.deltaTime * 5),
                                                 dialogueBox.localScale.z);
            yield return null;
        } npcRect.gameObject.SetActive(true);
        while (npcRect.anchorMin.y != targetAnchor.x
               || npcRect.anchorMax.y != targetAnchor.y) {
            npcRect.anchorMin = Vector2.MoveTowards(npcRect.anchorMin, new Vector2(npcRect.anchorMin.x, targetAnchor.x), Time.deltaTime * anchorSpeed);
            npcRect.anchorMax = Vector2.MoveTowards(npcRect.anchorMax, new Vector2(npcRect.anchorMax.x, targetAnchor.y), Time.deltaTime * anchorSpeed);
            yield return null;
        } yield return new WaitForSeconds(0.5f);
        StartCoroutine(IPlayDialogue());
    }

    private IEnumerator IEndDialogue() {
        state = State.Idle;
        ResetText();
        while (npcRect.anchorMin.y != nameYAnchor.x
               || npcRect.anchorMax.y != nameYAnchor.y) {
            npcRect.anchorMin = Vector2.MoveTowards(npcRect.anchorMin, new Vector2(npcRect.anchorMin.x, nameYAnchor.x), Time.deltaTime * anchorSpeed * 2);
            npcRect.anchorMax = Vector2.MoveTowards(npcRect.anchorMax, new Vector2(npcRect.anchorMax.x, nameYAnchor.y), Time.deltaTime * anchorSpeed * 2);
            yield return null;
        } npcRect.gameObject.SetActive(false);
        while (dialogueBox.localScale.y > 0) {
            dialogueBox.localScale = new Vector3(dialogueBox.localScale.x,
                                                 Mathf.MoveTowards(dialogueBox.localScale.y, 0, Time.deltaTime * 5),
                                                 dialogueBox.localScale.z);
            yield return null;
        } yield return new WaitForSeconds(0.5f) ;
        OnDialogueEnd?.Invoke();
    }

    private IEnumerator IPlayDialogue() {
        state = State.Writing;
        while (true) {
            string str = "<line-height=100%>";
            for (int i = 0; i < currLetter && i < currStr.Length; i++) {
                if (currStr[i] == ' ') {
                    if (!growthSet[i]) {
                        currLetter++;
                        growthSet[i] = true;
                    }
                    str += " ";
                    continue;
                }
                if (!growthSet[i]) {
                    sizeSet[i] = Mathf.MoveTowards(sizeSet[i], fontBounds.y, Time.deltaTime * textGrowthMult);
                    if (sizeSet[i] >= fontBounds.y) {
                        growthSet[i] = true;
                        currLetter++;
                    }
                } else {
                    sizeSet[i] = Mathf.MoveTowards(sizeSet[i], fontBounds.x, Time.deltaTime * textGrowthMult * 0.5f);
                }
                if (state != State.Awaiting
                    && currLetter >= currStr.Length) {
                    state = State.Awaiting;
                }
                str += $"<size={sizeSet[i]}>{currStr[i]}</size>";
            }
            textMesh.text = str;
            yield return null;
        }
    }

    private bool LoadNextLine() {
        bool hasNextLine = ++currLine < currDialogue.lines.Length;
        if (hasNextLine) {
            currLetter = 1;
            currStr = currDialogue.lines[currLine];
            growthSet = new bool[currStr.Length];
            sizeSet = new float[currStr.Length];
        }
        return hasNextLine;
    }

    private void ResetText() {
        currLetter = 1;
        currLine = -1;
        currStr = "";
        growthSet = null;
        sizeSet = null;
        currDialogue = null;
    }
}
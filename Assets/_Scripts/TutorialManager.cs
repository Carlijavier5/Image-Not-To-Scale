using UnityEngine;
using UnityEngine.UI;

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
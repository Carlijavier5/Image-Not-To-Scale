using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour {

    public static TransitionManager Instance { get; private set; }

    [SerializeField] private Image image;
    [SerializeField] private float transitionTime;

    void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
            return;
        }
    }

    public void StartGame() {
        AsyncOperation loadOP = SceneManager.LoadSceneAsync(1);
        while (!loadOP.isDone) {

        }
    }
}
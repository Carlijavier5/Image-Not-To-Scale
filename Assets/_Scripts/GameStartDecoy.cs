using System.Collections;
using UnityEngine;

public class GameStartDecoy : MonoBehaviour {

    void Awake() => StartCoroutine(GameStart());

    private IEnumerator GameStart() {
        yield return new WaitForSeconds(4);
        GameManager.Instance.StartGame();
    }
}
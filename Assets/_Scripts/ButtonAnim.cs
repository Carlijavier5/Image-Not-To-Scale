using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonAnim : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,
                                         IPointerDownHandler, IPointerUpHandler {

    private Vector3 baseSize;
    private Vector3 targetSize;
    private readonly float multSize = 1.1f;
    private readonly float redSize = 0.9f;

    private readonly float growMult = 4f;
    private readonly float redMult = 8f;
    private float speedMult;

    private bool mouseContained;

    void Awake() {
        baseSize = transform.localScale;
        targetSize = baseSize;
        speedMult = growMult;
    }

    void Update() {
        transform.localScale = Vector3.MoveTowards(transform.localScale, targetSize, Time.deltaTime * speedMult);
    }

    public void OnPointerEnter(PointerEventData data) {
        mouseContained = true;
        targetSize = baseSize * multSize;
        speedMult = growMult;
    }

    public void OnPointerExit(PointerEventData data) {
        mouseContained = false;
        targetSize = baseSize;
        speedMult = redMult;
    }

    public void OnPointerDown(PointerEventData data) {
        targetSize = baseSize * redSize;
        speedMult = redMult;
    }

    public void OnPointerUp(PointerEventData data) {
        targetSize = mouseContained ? baseSize * multSize : baseSize;
        speedMult = growMult;
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AttributeUpdate : MonoBehaviour {
    [SerializeField] private CanvasGroup CanvasGroup;
    [SerializeField] private RectTransform AttributePopUp;
    [SerializeField] private RectTransform OriginalPosition;
    [SerializeField] private RectTransform PopUpPosition;
    [SerializeField] private float FadeSpeed = 1f;
    [SerializeField] private float StayDuration = 1f;
    private bool FadeIn = false;
    private bool FadeOut = false;
    private bool InView = false;
    private float InViewStartTime;
    private float transformMuliplier;

    public void Start()
    {
        AttributePopUp.position = OriginalPosition.position;
        //transformMuliplier = AttributePopUp.position.y + PopUpPosition.position.y;
        transformMuliplier = PopUpPosition.position.y - OriginalPosition.position.y;
    }

    public void ShowUI() {
        FadeIn = true;
    }

    public void HideUI() {
        FadeOut = true;
    }

    private void Update() {
        Debug.Log("X: " + AttributePopUp.position.x + " | Y: " + AttributePopUp.position.y);
        Debug.Log("Transform Multiplier: " + transformMuliplier);
        //Debug.Log("InViewStartTime: " + InViewStartTime + " | CurrentTime: " + Time.time);
        if(FadeIn)
            if(CanvasGroup.alpha < 1) {
                CanvasGroup.alpha += Time.deltaTime * FadeSpeed;
                AttributePopUp.position = Vector3.MoveTowards(AttributePopUp.position, PopUpPosition.position, Time.deltaTime * transformMuliplier);
                if (CanvasGroup.alpha >= 1) {
                    FadeIn = false;
                    InView = true;
                    InViewStartTime = Time.time;
                }
            }
        if (FadeOut)
            if (CanvasGroup.alpha >= 0) {
                CanvasGroup.alpha -= Time.deltaTime * FadeSpeed;
                AttributePopUp.position = Vector3.MoveTowards(AttributePopUp.position, OriginalPosition.position, Time.deltaTime * transformMuliplier);
                if (CanvasGroup.alpha == 0) {
                    FadeOut = false;
                    InView = false;
                }
            }

        if(InView && Time.time >= InViewStartTime + StayDuration)
            HideUI();
    }

    public void UpdateAttribute() {

    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AttributeUpdate : MonoBehaviour {
    [SerializeField] private Player Player;
    [SerializeField] private CanvasGroup CanvasGroup;
    [SerializeField] private RectTransform AttributePopUp;
    [SerializeField] private RectTransform OriginalPosition;
    [SerializeField] private RectTransform PopUpPosition;
    [SerializeField] private TMP_Text PopUpTextUI;
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
        transformMuliplier = PopUpPosition.position.y - OriginalPosition.position.y;
    }

    public void ShowUI() {
        FadeIn = true;
    }

    public void HideUI() {
        FadeOut = true;
    }

    private void Update() {
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
                    PopUpTextUI.text = "";
                }
            }

        if(InView && Time.time >= InViewStartTime + StayDuration)
            HideUI();
    }

    public void UpdateAttribute(string attribute) {
        if(FadeIn || FadeOut || InView)
            return;

        Debug.Log("Attribute Text: " + attribute);
        PopUpTextUI.text = attribute + " has been ";

        if(attribute[attribute.Length - 1] == '+')
            PopUpTextUI.text += "increased";
        else
            PopUpTextUI.text += "decreased";

        Player.UpdateAttribute(attribute, 1);
        ShowUI();
    }
}

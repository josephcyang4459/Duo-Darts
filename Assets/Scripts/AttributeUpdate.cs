using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AttributeUpdate : MonoBehaviour {
    public static AttributeUpdate inst;
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
    public Queue<info> info = new();

    public void Start()
    {
        inst = this;
        AttributePopUp.position = OriginalPosition.position;
        transformMuliplier = MathF.Abs(PopUpPosition.position.y - OriginalPosition.position.y);
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


                    if (info.Count > 0)
                    {
                        UpdateAttribute(info.Peek().stat, info.Peek().change);
                        info.Dequeue();
                    }
                  
                }
            }

        if(InView && Time.time >= InViewStartTime + StayDuration)
            HideUI();
    }

    public void UpdateAttribute(string attribute, int value) {
        if(FadeIn || FadeOut || InView)
        {
            info t = new info();
            t.stat = attribute;
            t.change = value;
            info.Enqueue(t);
            return;
        }
            

        PopUpTextUI.text = "";
        //PopUpTextUI.text += char.ToUpper(attribute[0]);
        PopUpTextUI.text += attribute + " has been ";

        PopUpTextUI.text += (value > 0) ? "increased by " : "decreased by ";
        PopUpTextUI.text += Mathf.Abs(value);

        Player.UpdateAttribute(attribute, value);
        ShowUI();
    }

    string AttributeName(PlayerSkills skill)
    {
        switch (skill)
        {
            case PlayerSkills.Charisma:
                return "charisma";
            case PlayerSkills.Intoxication:
                return "intoxication";
            case PlayerSkills.Skill:
                return "skill";
            case PlayerSkills.Luck:
                return "luck";
        }
        return null;
    }

    public void UpdateAttribute(PlayerSkills skill, int value)
    {
            UpdateAttribute(AttributeName(skill), value);
    }

    public void UpdateAttributeButton(string String) {
        string AttributeString = String.Substring(1, String.Length - 1);
        char c = String[String.Length - 1];
        int AttributeValue = int.Parse(c.ToString());

        if (String[0].Equals('+'))
            UpdateAttribute(AttributeString, AttributeValue);
        else
            UpdateAttribute(AttributeString, AttributeValue * -1);

    }

    
}

public class info
{
    public string stat;
    public int change;
}
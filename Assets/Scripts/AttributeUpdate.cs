using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AttributeUpdate : MonoBehaviour, Caller {
    public static AttributeUpdate inst;
    [SerializeField] private Player Player;
    [SerializeField] private TMP_Text PopUpTextUI;
    [SerializeField] bool Active = false;
    [SerializeField] Queue<StatChangeInfo> StatChangeQueue = new();
    [SerializeField] UIAnimationElement AnimationHead;

    public void Start() {
        inst = this;
    } 

    public void UpdateAttribute(string attribute, int value) {
        if(Active) {
            StatChangeInfo t = new StatChangeInfo { stat = attribute, change = value };
            StatChangeQueue.Enqueue(t);
            return;
        }
            
        PopUpTextUI.text = "";
        PopUpTextUI.text += attribute.ToUpper() + ((value > 0) ? " +" : " -") + Mathf.Abs(value); 

        Player.UpdateAttribute(attribute, value);
        Active = true;
        AnimationHead.Begin(this);
    }

    string AttributeName(PlayerSkills skill) {
        switch (skill) {
            case PlayerSkills.Intoxication:
                return "intoxication";
            case PlayerSkills.Skill:
                return "skill";
            case PlayerSkills.Luck:
                return "luck";
        }
        return null;
    }

    public void UpdateAttribute(PlayerSkills skill, int value) { UpdateAttribute(AttributeName(skill), value); }

    public void UpdateAttributeButton(string String) {
        string AttributeString = String.Substring(1, String.Length - 1);
        char c = String[String.Length - 1];
        int AttributeValue = int.Parse(c.ToString());

        if (!String[0].Equals('+'))
            AttributeValue *= -1;

        UpdateAttribute(AttributeString, AttributeValue);
    }

    public void Ping() {
        Active = false;
        if (StatChangeQueue.Count > 0) {
            StatChangeInfo temp = StatChangeQueue.Dequeue();
            UpdateAttribute(temp.stat, temp.change);
        }
    }

    class StatChangeInfo {
        public string stat;
        public int change;
    }
}


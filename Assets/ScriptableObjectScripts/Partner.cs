using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

[CreateAssetMenu]
public class Partner : ScriptableObject {
    public string Name;
    public TMP_FontAsset Font;
    public float textSize;
    public float Composure = 0f;
    public float Intoxication = 0f;
    public float Love = 0f;
    public RelatedCutScene[] RelatedCutScenes;
    public CutScene DefaultCutScene;
    public CutScene DefaultDrinkingCutScene;
    public Sprite[] Expressions;
    public int bias = -1;
    public Sprite TextBox;
    public Sprite textLineTHing;

    public int GetCutScene() {
        for(int i = 0; i < RelatedCutScenes.Length; i++)
            if(!RelatedCutScenes[i].completed && Love >= RelatedCutScenes[i].loveRequirement)
                return i;
        return -1;
    }
}

[System.Serializable]
public class RelatedCutScene {
    public string Location;
    public CutScene CutScene;
    public bool completed;
    public int loveRequirement;
}
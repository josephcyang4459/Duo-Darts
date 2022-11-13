using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

[CreateAssetMenu]
public class Partner : ScriptableObject {
    public string Name;
    public TMP_FontAsset Font;
    public float Composure = 0f;
    public float Intoxication = 0f;
    public float Love = 0f;
    public RelatedCutScene[] RelatedCutScenes;
    public Sprite[] Expressions;
}

public class RelatedCutScene {
    public Sprite Background;
    string Location;
    CutScene CutScene;
    bool completed;
}
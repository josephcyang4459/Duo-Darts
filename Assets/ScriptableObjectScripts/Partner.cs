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
        for (int i = 0; i < RelatedCutScenes.Length; i++)
            if (!RelatedCutScenes[i].completed && Love >= RelatedCutScenes[i].loveRequirement)
                if (i != 3)
                {
                    return i;

                }
                else if (Intoxication >= 3)
                {
                    return i;

                }
                else
                    return -1;
        return -1;
    }

    public void stateChange(int stat, int change)
    {
        switch (stat)
        {
            case 0:
                Composure += change;
                return;
            case 1:
                Intoxication += change;
                if (Intoxication < 0)
                    Intoxication = 0;
                return;
            case 2:
                Love += change;
                return;
        }
    }
}

[System.Serializable]
public class RelatedCutScene {
    public string Location;
    public CutScene CutScene;
    public bool completed;
    public int loveRequirement;
}
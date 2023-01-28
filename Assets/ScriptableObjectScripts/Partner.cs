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
        for (int cutscene_index = 0; cutscene_index < RelatedCutScenes.Length; cutscene_index++)
            if ((!RelatedCutScenes[cutscene_index].completed) && Love >= RelatedCutScenes[cutscene_index].loveRequirement)
                if (cutscene_index != 3)
                {
                    return cutscene_index;
                }
                else if (Intoxication >= 3)
                {
                    return cutscene_index;// this should be drunk dutscene to handle
                }
                else
                    return cutscene_index; // not drunk cutscene to handle
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
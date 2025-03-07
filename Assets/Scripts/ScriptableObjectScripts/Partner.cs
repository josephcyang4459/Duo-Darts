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
    public CutScene DefaultRepeatingScene;
    public CutScene DefaultDrinkingCutScene;
    public CutScene FinalCutscene; 
    public Sprite[] Expressions;
    public DartAI AI;
    public DartsBanterLines RegularBanterLines;
    public DartsBanterLines FinalsBanterLines;
    public ColorSwatch TextBoxColors;
    
    /// <summary>
    /// Index of next available Cutscene or -1 if default
    /// </summary>
    /// <returns></returns>
    public int GetCutScene() {

        if (Love < 0)
            return -1;

        for (int cutscene_index = 0; cutscene_index < RelatedCutScenes.Length; cutscene_index++)
            if ((!RelatedCutScenes[cutscene_index].completed) && Love >= RelatedCutScenes[cutscene_index].loveRequirement)
            {
                if (cutscene_index != (int)PartnerCutscenes.DrunkScene)
                {
                    return cutscene_index;
                }
                //if it is drunk scene
                if (Intoxication >= 3)
                {
                    return cutscene_index;// this should be drunk dutscene to handle
                }// if they are not drunk enought we move on to next one if we can
            }
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

    public Sprite GetExpression(int i) {
        if (i > Expressions.Length)
            return Expressions[0];
        return Expressions[i];
    }

    public bool FinalRoundEligable() {
        return RelatedCutScenes[(int)PartnerCutscenes.FinalScene].completed && Love >= 0;
    }

    public void ResetBanterLineUsage() {
        if (RegularBanterLines != null)
            RegularBanterLines.ResetAllGroups();
        if (FinalsBanterLines != null)
            FinalsBanterLines.ResetAllGroups();
    }

    public void ResetValues(float love, float intox, float compose)
    {
        Love = love;
        Intoxication = intox;
        Composure = compose;
        for (int i = 0; i < RelatedCutScenes.Length; i++)
            RelatedCutScenes[i].completed = false;
    }
#if UNITY_EDITOR

    public void OnValidate()
    {
        for(int i = 0; i < RelatedCutScenes.Length; i++)
        {
            RelatedCutScenes[i].Location = RelatedCutScenes[i].CutsceneLocation.ToString();
        }
        UnityEditor.EditorUtility.SetDirty(this);

    }

    public void __resetValues() {
        Love = 0; Intoxication = 0; Composure = 0;

        for (int i = 0; i < RelatedCutScenes.Length; i++)
            RelatedCutScenes[i].completed = false;
    }

    public void __resetValues(float compose)
    {
        Love = 0;
        Intoxication = 0;
        Composure = compose;
        for (int i = 0; i < RelatedCutScenes.Length; i++)
            RelatedCutScenes[i].completed = false;
    }

#endif
}



[System.Serializable]
public class RelatedCutScene {
#if UNITY_EDITOR
    [HideInInspector] public string Location;
#endif
    public Locations CutsceneLocation;
    public CutScene CutScene;
    public bool completed;
    public int loveRequirement;
}
using UnityEngine;

[System.Serializable]
public class SaveFile {
    public int Hour;
    public int Minute;
    public int MascFemPortraitIndex;
    public PlayerSaveData Player;
    public PartnerSaveData[] Characters;
    public bool[] EventCompletion;
}
[System.Serializable]
public struct PlayerSaveData {
    public float Skill;
    public float Intoxication;
    public float Luck;
    public int TotalPoints;
}

[System.Serializable]
public struct PartnerSaveData {
    public float Composure;
    public float Intoxication;
    public float Love;
    public bool[] CutsceneCompletion;
}
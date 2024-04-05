using UnityEngine;

[System.Serializable]
public class SaveFile {
    public int Hour;
    public int Minute;
    public int MascFemPortraitIndex;
    public PlayerSaveData Player;
    public PartnerSaveData[] Characters;
    public bool[] EventCompletion;

    /// <summary>
    /// Joseph's code Originally from schedule :)
    /// </summary>
    /// <param name="hour"></param>
    /// <param name="minutes"></param>
    /// <returns></returns>
    private string TimeAsString(int hour, int minutes) {
        string minutesString = (minutes < 10) ? "0" : "";
        minutesString += minutes.ToString();
        return hour + ":" + minutesString + "PM";
    }

    public string GetDisplayData() {
        return TimeAsString(Hour, Minute) + " Score " + Player.TotalPoints;
    }
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

[System.Serializable]
public class CompletionData {
    public bool[] Endings = new bool[4];
}
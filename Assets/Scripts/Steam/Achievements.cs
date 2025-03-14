using UnityEngine;
#if !DISABLESTEAMWORKS
using Steamworks;
#endif
public class Achievements : MonoBehaviour {
    public static Achievements Instance;
   
    [SerializeField] Achievement[] AllAchievements;
    [SerializeField] bool OopsAllChad;
    [SerializeField] SpriteCollection ChadSprites;
    [SerializeField] DecodingState State;
    string AchievementHeader = "achievements";
#if UNITY_EDITOR
    [SerializeField] bool SaveAfterLoad;
#endif
    public void Start() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public bool IsAllChad() {
        return OopsAllChad;
    }

    /// <summary>
    /// Generates Major Garbage
    /// </summary>
    public void LoadLocalAchievements() {
        string data = FileHandler.LoadCompletion();

        string[] lines = data.Split("\n");

        for (int i = 0; i < lines.Length; i++) {
            DecodeLine(lines[0]);
        }
        TryReconcileSteamAchiements();
#if UNITY_EDITOR
        if (SaveAfterLoad)
            SaveLocalToDisk();
#endif
    }

    public Sprite GetChadExpression(int index) {
        return ChadSprites.GetSprite(index);
    }

    void DecodeLine(string line) {
        if (line.Length == 0)
            return;
        if (line.Trim().CompareTo(AchievementHeader) == 0) {
            State = DecodingState.Achiements;
            return;
        }
        if (line.Trim().CompareTo("stats") == 0) {
            State = DecodingState.Stats;
            return;
        }
        string[] subLine = line.Split("|");

        switch (State) {
            case DecodingState.Achiements:
                Achievement a = FindAchievement(subLine[0].Trim());
                if (a == null) {
#if UNITY_EDITOR
                    Debug.Log("Could not find Achievement " + subLine[0].Trim());
#endif
                    return;
                }
                a.SetLocalState(subLine[1].Trim().CompareTo("T") == 0);

                return;
            case DecodingState.Stats:
                return;
        }
    }

    Achievement FindAchievement(string name) {
        foreach (Achievement a in AllAchievements) {
            if (a.GetInternalName().CompareTo(name) == 0)
                return a;
        }
        return null;
    }

    void TryReconcileSteamAchiements() {
        if (!SteamManager.Initialized)
            return;
        foreach (Achievement a in AllAchievements)
            a.TryReconcileWithOnline();
    }

    /// <summary>
    /// Generates Major Garbage call during loading or other non performance sensitive areas
    /// </summary>
    public void SaveLocalToDisk() {
        string result = string.Empty;
        result += "\n" + AchievementHeader + "\n";
        foreach (Achievement a in AllAchievements)
            result += a.Data();
        FileHandler.SaveCompletion(result);
    }

    enum DecodingState {
        Achiements,
        Stats,
    }
}

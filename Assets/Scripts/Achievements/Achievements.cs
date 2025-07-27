using UnityEngine;

public class Achievements : MonoBehaviour {
    public static Achievements Instance;
   
    [SerializeField] Achievement[] AllAchievements;
    [SerializeField] Statistic[] AllStatistics;
    [SerializeField] bool OopsAllChad;
    [SerializeField] SpriteCollection ChadSprites;
    [SerializeField] DecodingState State;
    string AchievementHeader = "achievements";
    string StatisticsHeader = "statistics";
    [SerializeField] bool AlreadyLoaded;
    [SerializeField] bool CurrentlyLoading;
    [SerializeField] AchivementsPopup Popup;
#if UNITY_EDITOR
    [SerializeField] bool __SaveAfterLoad;
#endif

    public void Start() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }
        AlreadyLoaded = false;
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public Achievement[] GetAllAchivements() {
        return AllAchievements;
    }

    public bool IsAllChad() {
        return OopsAllChad;
    }

    public void SetAllChad(bool b) {
        OopsAllChad = b;
    }

    public void ShowAchivementPop(Achievement achieved) {
        if (CurrentlyLoading)
            return;

    }

    /// <summary>
    /// Generates Major Garbage
    /// </summary>
    public void LoadLocalAchievements() {
        if (AlreadyLoaded)
            return;
        CurrentlyLoading = true;
        string data = FileHandler.LoadCompletion();
        if (data == null) {
            return;
        }
        AlreadyLoaded = true;
        string[] lines = data.Split("\n");

        for (int i = 0; i < lines.Length; i++) {
            DecodeLine(lines[i]);
        }
        TryReconcileSteamAchiements();
#if UNITY_EDITOR
        if (__SaveAfterLoad)
            SaveLocalToDisk();
#endif
        CurrentlyLoading = false;
    }

    public Sprite GetChadExpression(int index) {
        return ChadSprites.GetSprite(index);
    }

    void DecodeLine(string line) {
        if (line.Length == 0)
            return;
#if UNITY_EDITOR
        Debug.Log(line);
#endif
        if (line.Trim().CompareTo(AchievementHeader) == 0) {
            State = DecodingState.Achiements;
            return;
        }
        if (line.Trim().CompareTo(StatisticsHeader) == 0) {
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
#if UNITY_EDITOR
                Debug.Log("Setting Achievement " + subLine[0].Trim()+" to "+ subLine[1]);
#endif
                a.SetLocalState(subLine[1].Trim().CompareTo("T") == 0);

                return;
            case DecodingState.Stats:
                Statistic s = FindStatistic(subLine[0].Trim());
                if (s == null) {
#if UNITY_EDITOR
                    Debug.Log("Could not find Statistic " + subLine[0].Trim());
#endif
                    return;
                }
                s.SetLocalData(subLine);
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

    Statistic FindStatistic(string name) {
        foreach (Statistic a in AllStatistics) {
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
        result += "\n" + StatisticsHeader + "\n";
        foreach (Statistic a in AllStatistics)
            result += a.GetData();
        FileHandler.SaveCompletion(result);
    }

    enum DecodingState {
        Achiements,
        Stats,
    }
    private void OnDestroy() {
        SaveLocalToDisk();
    }
#if UNITY_EDITOR
    [SerializeField] bool __testPopup;
    [SerializeField] bool __resetAll;
    [SerializeField] bool __grab;

    private void OnValidate() {
        if (__grab) {
            __grab = false;
            AllStatistics = (Statistic[])Resources.FindObjectsOfTypeAll(typeof(Statistic));
            AllAchievements = (Achievement[])Resources.FindObjectsOfTypeAll(typeof(Achievement));
        }
        if (__resetAll) {
            __resetAll = false;
            foreach (Achievement a in AllAchievements) {
                a.SetLocalState(false);
            }
            foreach (Statistic a in AllStatistics) {
                a.ResetStatistic();
            }
        }
        if (__testPopup) {
            __testPopup = false;
            Popup.AddToQueue(AllAchievements[0]);
        }
    }
#endif
}

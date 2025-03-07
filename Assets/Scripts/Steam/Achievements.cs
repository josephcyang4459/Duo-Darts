using UnityEngine;
#if !DISABLESTEAMWORKS
using Steamworks;
#endif
public class Achievements : MonoBehaviour {
    public static Achievements Instance;
    [SerializeField] FileHandler Handler;
    
    public void Start() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void LoadLocalAchievements() {

    }

    void TryReconcileSteamAchiements() {
        if (!SteamManager.Initialized)
            return;
    }


    public void TrySetAchievement(string title, bool state) {


        if (!SteamManager.Initialized)
            return;
#if !DISABLESTEAMWORKS
        if (state) {
            if (SteamUserStats.GetUserAchievement(SteamUser.GetSteamID(), title, out bool achieved)) {
                if (!achieved)
                    SteamUserStats.SetAchievement(title);
            }
            return;
        }
        SteamUserStats.ClearAchievement(title);
#endif
    }
}

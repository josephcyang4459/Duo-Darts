using UnityEngine;
#if !DISABLESTEAMWORKS
using Steamworks;
#endif

[CreateAssetMenu]
public class Achievement : ScriptableObject {
    [SerializeField] string InternalName;
    [SerializeField] string DisplayName;
    [SerializeField] [TextArea (2,5)] string Description;
    [SerializeField] bool Completed;
    [SerializeField] bool Secret;
    [SerializeField] Sprite IncompleteIcon;
    [SerializeField] Sprite CompleteIcon;
  
    public Sprite GetCorrectIcon() {
        return Completed ? CompleteIcon : IncompleteIcon;
    }

    /// <summary>
    /// API Call Name
    /// </summary>
    /// <returns></returns>
    public string GetInternalName() {
        return InternalName;
    }

    /// <summary>
    /// This Is The official Name displayed on steam and stuff
    /// </summary>
    /// <returns></returns>
    public string GetDisplayName() {
        return DisplayName;
    }

    public string GetDescription() {
        return Description;
    }

    public bool IsComplete() {
        return Completed;
    }

    /// <summary>
    /// Tries to set Steam to same state USE DURING GAMEPLAY
    /// </summary>
    /// <param name="state"></param>
    public void TrySetAchievement(bool state) {
        if (state == Completed)
            return;
        Completed = state;

        if (!SteamManager.Initialized)
            return;
#if !DISABLESTEAMWORKS
        if (state) {
            if (SteamUserStats.GetUserAchievement(SteamUser.GetSteamID(), InternalName, out bool achieved)) {
                if (!achieved)
                    SteamUserStats.SetAchievement(InternalName);
            }
            return;
        }
        SteamUserStats.ClearAchievement(InternalName);
#endif
    }

    /// <summary>
    /// Using During initial loading to avoid Online Achievement Service conflicts
    /// </summary>
    /// <param name="state"></param>
    public void SetLocalState(bool state) {
        Completed = state;
    }

    /// <summary>
    /// CHECK THAT STEAM MANAGER IS INITIALIZED BEFORE CALLING PLEASE
    /// </summary>
    public void TryReconcileWithOnline() {
#if !DISABLESTEAMWORKS
        if (SteamUserStats.GetUserAchievement(SteamUser.GetSteamID(), InternalName, out bool achieved)) {
            if (achieved && !Completed) {
                Completed = true;
                return;
            }
            if (!achieved && Completed) {
                Completed = true;
                SteamUserStats.SetAchievement(InternalName);
                return;
            }
        }
#endif
    }

    public string Data() {
        return InternalName + "|" + (Completed ? "T" : "F") + "\n";
    }
}

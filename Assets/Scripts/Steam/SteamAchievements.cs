using UnityEngine;

public class SteamAchievements : MonoBehaviour
{
    public static SteamAchievements Instance;

    public void Start() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

}

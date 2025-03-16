using UnityEngine;


public class AccomplishmentsUI : MonoBehaviour
{
    [SerializeField] Canvas UI;
    [SerializeField] AchievementUI AchievementsTab;
    [SerializeField] MainMenu MainMenu;
    public void Begin() {
        UI.enabled = true;
        AchievementsTab.Begin();
    }

    public void End() {
        UI.enabled = false;
        MainMenu.Ping();
    }
}

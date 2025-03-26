using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class AccomplishmentsUI : MonoBehaviour
{
    [SerializeField] Canvas UI;
    [SerializeField] AchievementUI AchievementsTab;
    [SerializeField] StatisticsUI StatisticsTab;
    [SerializeField] SecretUI SecretTab;
    [SerializeField] MainMenu MainMenu;
    [SerializeField] Accomplishment_SubMenu CurrentMenu;
    [SerializeField] Image[] ToColor;
    [SerializeField] Color NonSetColor;
    [SerializeField] ControlVisual[] Controls;
    [SerializeField] InputActionReference ChangeTab;

    public void Begin() {
        ControlState.UsingController += UsingController;
        UsingController(ControlState.inst.IsUsingController());
        DartSticker.inst.SetVisible(false);
        UI.enabled = true;
        if (CurrentMenu != null) {
            CurrentMenu.End();
            CurrentMenu.SetColor(NonSetColor);
        }
        CurrentMenu = null;
        SetAchievementUI();
        foreach (ControlVisual c in Controls)
            c.Begin();
    }

    public void SetSubMenu(Accomplishment_SubMenu menu) {
        if (menu == CurrentMenu)
            return;
        if (CurrentMenu != null) {
            CurrentMenu.End();
            CurrentMenu.SetColor(NonSetColor);
        }
           
        CurrentMenu = menu;// ==what the fuck have i done here jesus christ
        SetColor(menu.GetColor());
        menu.SetColor(menu.GetColor());
        menu.Begin();
    }

    public void UsingController(bool b) {
        if (b) {
            ChangeTab.action.Enable();
            ChangeTab.action.performed += ChangeTabFunction;
            return;
        }
        if (b) {
            ChangeTab.action.Disable();
            ChangeTab.action.performed -= ChangeTabFunction;
            return;
        }

    }

    public void ChangeTabFunction(InputAction.CallbackContext c) {// what the actual fuck am i doing at this point
        float value = c.ReadValue<float>();
        if (CurrentMenu == AchievementsTab) {
            if (value > 0)
                SetStatisticsUI();
            return;
        }
        if (CurrentMenu == StatisticsTab) {
            if (value > 0) {
                SetSecretUI();
                return;
            }
            if (value < 0) {
                SetAchievementUI();
                return;
            }
            return;
        }
        if (CurrentMenu == SecretTab) {
            if (value < 0)
                SetStatisticsUI();
            return;
        }
    }

    public void SetAchievementUI() {//all these beigbn the same bad shit ass code
        SetSubMenu(AchievementsTab);
    }

    public void SetStatisticsUI() {
        SetSubMenu(StatisticsTab);
    }
    public void SetSecretUI() {
        SetSubMenu(SecretTab);
    }

    public void End() {
        UsingController(false);
        ControlState.UsingController -= UsingController;
        UI.enabled = false;
        MainMenu.Ping();
        foreach (ControlVisual c in Controls)
            c.End();
    }

    public void SetColor(Color c) {
        foreach(Image i in ToColor) {
            i.color = c;
        }
    }
}

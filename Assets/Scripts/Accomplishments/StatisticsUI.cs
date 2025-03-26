using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class StatisticsUI : Accomplishment_SubMenu {

    [SerializeField] Canvas StatisiticsCanvas;
    [SerializeField] Canvas[] Pages;
  
    [SerializeField] int CurrentPage;
    [SerializeField] InputActionReference ChangePage;
    [Header("Character numbers")]
    [SerializeField] CharacterList Characters;
    [SerializeField] TMP_Text[] VictoriesTexts;
    [Header("Darts Stats")]
    [SerializeField] Statistic_Int TotalWins;
    [SerializeField] TMP_Text TotalWinText;
    [SerializeField] Statistic_Int Bullseyes;
    [SerializeField] TMP_Text BullseyesText;
    [SerializeField] Statistic_Int Sixties;
    [SerializeField] TMP_Text SixtiesText;
    [SerializeField] Statistic_Int Misses;
    [SerializeField] TMP_Text MissesText;
    [Header("Big")]
    [SerializeField] ControlVisual ControlVisual;
    [SerializeField] Image LeftArrow;
    [SerializeField] Image RightArrow;
    public override void Begin() {
        StatisiticsCanvas.enabled = true;
        CurrentPage = -1;
        foreach (Canvas c in Pages)
            c.enabled = false;
        BeginPage(0);
       
        ControlVisual.Begin();
       
        ControlState.UsingController += UsingController;
        UsingController(ControlState.inst.IsUsingController());
        Back.action.Enable();
        Back.action.performed += BackFunction;
    }

    public void BackFunction(InputAction.CallbackContext c) {
        End();
        Accomplishents.End();
    }

    void ChangePageStick(InputAction.CallbackContext c) {
        float var = c.ReadValue<Vector2>().x;
        if(var == 0) return;
        ChangePageFunction(var > 0 ? 1 : -1);
    }

    public void ChangePageFunction(int num) {
        BeginPage(CurrentPage + num);
    }

    void UsingController(bool b) {
       
        if (b) {
            ChangePage.action.Enable();
            ChangePage.action.performed += ChangePageStick;
            return;
        }
        ChangePage.action.Disable();
        ChangePage.action.performed -= ChangePageStick;
    }

    void BeginPage(int newPage) {
        if (newPage < 0)
            return;
        if (newPage >= Pages.Length)
            return;
        if (newPage == CurrentPage)
            return;

        if (CurrentPage > -1) {
            Pages[CurrentPage].enabled = false;
        }
        
        CurrentPage = newPage;
        LeftArrow.enabled = CurrentPage != 0;
        RightArrow.enabled = CurrentPage != Pages.Length - 1;
        Pages[CurrentPage].enabled = true;
        switch (CurrentPage) {
            case 0://Victories Page
                UpdateVictoriesPage();
                return;
            case 1:
                UpdateDartsPage();
                return;
        }
    }

    void UpdateVictoriesPage() {
        int numer = (int)CharacterNames.Player;
        for(int i = 0; i < numer; i++) {
            VictoriesTexts[i].text = Characters.list[i].Victories.GetNumber().ToString();
        }
    }

    void UpdateDartsPage() {
        TotalWinText.text = TotalWins.GetNumber().ToString();
        BullseyesText.text = Bullseyes.GetNumber().ToString();
        SixtiesText.text = Sixties.GetNumber().ToString();
        MissesText.text = Misses.GetNumber().ToString();
    }

    public override void End() {
        StatisiticsCanvas.enabled = false;
        Pages[CurrentPage].enabled = false;
        foreach (Canvas c in Pages)
            c.enabled = false;
        Back.action.Disable();
        Back.action.performed -= BackFunction;
        ControlVisual.End();
        UsingController(false);
        ControlState.UsingController -= UsingController;
    }

    private void OnDestroy() {
        End();
    }

#if UNITY_EDITOR
    [SerializeField] Transform __VictoriesHolder;
    [SerializeField] CharacterList __List;
    [SerializeField] bool __FixVictories;

    private void OnValidate() {
        if (__FixVictories) {
            __FixVictories = false;
            __FixVic();
        }
    }

    void __FixVic() {
        int num = (int)CharacterNames.Player;
        VictoriesTexts = new TMP_Text[num];
        for (int i = 0; i < num; i++) {
            GameObject g = __VictoriesHolder.GetChild(i).gameObject;
            g.transform.GetChild(0).GetComponent<Image>().sprite = __List.list[i].GetRawExpression(1);
            VictoriesTexts[i] = g.transform.GetChild(2).transform.GetComponent<TMP_Text>();
        }
    }
#endif
}

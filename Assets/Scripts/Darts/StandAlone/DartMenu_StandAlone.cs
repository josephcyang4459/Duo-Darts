using UnityEngine;
using UnityEngine.UI;

public class DartMenu_StandAlone : MonoBehaviour, Caller, SceneEntrance, TransitionCaller {
    [SerializeField] Player Player;
    [SerializeField] CharacterList Partners;
    [SerializeField] InSceneTransition Transition;
    [SerializeField] DartGame DartGame;
  
    [SerializeField] ResetStats ResetStats;
    [SerializeField] float TipsyIntoxValue;
    [SerializeField] float DefaultSkill;
    [SerializeField] float DefaultLuck;
    [Header("Entry Menu")]
    [SerializeField] UIAnimationElement OverallEnterAnimationHead;
    [SerializeField] Canvas OverallCanvas;
    [SerializeField] GameObject FirstOverallButton;
    [SerializeField] GroupImageFill EntryMenuFill;
    [SerializeField] Fillable_Image[] EntryMenuImageGroups;
    [SerializeField] Transform[] OverallDartTargets;

    [Header("Partner Select")]
    [SerializeField] Canvas PartnerCanvas;
    [SerializeField] GameObject FirstPartnerButton;
    [SerializeField] Image[] PartnerButtonImages;
    [SerializeField] Vector3 PartnerLocationOffset;
    [SerializeField] ImageFill Fill;
    [SerializeField] ImageSlide Slide;
    [SerializeField] ImageSmoothFill[] BorderFills;
    [Header("Score/ Settings")]
    [SerializeField] DartMenu_StandAlone_Options Options;
    [SerializeField] UIAnimationElement ScoreAnimationEnterHead;
    [SerializeField] UIAnimationElement ScoreAnimationLeaveHead;
    [SerializeField] GameObject FirstScoreButton;
    [SerializeField] Canvas ScoreCanvas;
    [SerializeField] Image[] ScoreButtonImages;
    [SerializeField] Vector3 ScoreLocationOffset;

    [SerializeField] Image Portrait;
    [SerializeField] int PartnerIndex;

    [SerializeField] PingState AnimationState;
    public void Start() { TransitionManager.inst.ReadyToEnterScene(this); }

    public void EnterScene() {
        /* if (PlayerPrefs.GetInt("hasReadDartsTutorial") == 0) {
             PauseMenu.inst.SetEnabled(false);
             PauseMenu.inst.PauseInput.action.Enable();
         } else {
             PauseMenu.inst.SetEnabled(true);
         }*/
      
     
        BeginSetUp();
    }

    public void BeginSetUp() {
        Audio.inst.PlaySong(MusicTrack.LocationSelect);
        //PartnerCanvas.enabled = true;
        UIState.inst.SetInteractable(true);
        Player.Skill = DefaultSkill;
        Player.Luck = DefaultLuck;
       
        if (TutorialHandler.inst.ShouldDisplayTutorial()) {
            Tutorial();
            return;
        }
        OverallCanvas.enabled = true;
        AnimationState = PingState.EnterMain;
        OverallEnterAnimationHead.Begin(this);
        

        //UIState.inst.SetAsSelectedButton(FirstPartnerButton);
    }

    public void Overall_SelectButton(int index) {
        EntryMenuFill.SetCurrentImageToFill(EntryMenuImageGroups[index], OverallDartTargets[index].position);
    }

    public void SelectPartnerButton() {
        Fill.ClearImages();
        OverallCanvas.enabled = false;
        PartnerCanvas.enabled = true;
        DartSticker.inst.SetVisible(false);
        UIState.inst.SetAsSelectedButton(FirstPartnerButton);
    }

    public void StartGameWithPartner(int i) {
        PartnerIndex = i;
        OverallCanvas.enabled = false;
        ScoreCanvas.enabled = true;
        AnimationState = PingState.EnterScore;
        ScoreAnimationEnterHead.Begin(this);
        DartSticker.inst.SetVisible(false);
        Fill.ClearImages();
       
    }

    public void ShowCharacterPortrait(int i) {
        Fill.SetCurrentImageToFill(PartnerButtonImages[i], ((RectTransform)PartnerButtonImages[i].transform).position + PartnerLocationOffset * Screen.width / 1920f);
       
       // Audio.inst.PlayClip(AudioClips.RandomDart);
        if (i >= (int)CharacterNames.Player) {
            Portrait.sprite = Partners.list[0].GetExpression(2);
            Slide.SetToStart();
            return;
        }
           

        if (Portrait.sprite == Partners.list[i].GetExpression(0))
            return;
        float fill = (float)i / (float)(Partners.list.Length - 2);// bruh this the top and bottom things duh
        foreach (ImageSmoothFill image in BorderFills)
            image.FillTo(fill);

        Portrait.sprite = Partners.list[i].GetExpression(0);
        Slide.BeginSlide();     
    }

    public void Back() {
        Fill.ClearImages();
        DartSticker.inst.SetVisible(false);
        OverallCanvas.enabled = true;
        PartnerCanvas.enabled = false;
        UIState.inst.SetAsSelectedButton(FirstOverallButton);
        
    }

    public void BackToMenu() {
        PauseMenu.inst.ExitToMain();
    }

    public void Tutorial() {
        AnimationState = PingState.Tutorial;
        TutorialHandler.inst.EnableDartsTutorial(true, this);
    }

    public void HoverScoreButton(int i) {
        Audio.inst.PlayClip(AudioClips.Click);
        Fill.SetCurrentImageToFill(ScoreButtonImages[i], ((RectTransform)ScoreButtonImages[i].transform).position + ScoreLocationOffset * Screen.width / 1920f);
    }

    public void BackToPartnerScreen() {
        Fill.ClearImages();
        DartSticker.inst.SetVisible(false);
        AnimationState = PingState.ExitScore;
        ScoreAnimationLeaveHead.Begin(this);
    }

    bool CharacterIsAlreadyDrunkCharacter(int i) {
        switch (i) {
            case (int)CharacterNames.CharmingGirl: return true;
            case (int)CharacterNames.DanceGirl: return true;
            case (int)CharacterNames.LoungeGuy: return true;
        }
        return false;
    }

    public void SetScore(int i) {
        if (!CharacterIsAlreadyDrunkCharacter(PartnerIndex))
            Partners.list[PartnerIndex].Intoxication = Options.TipsyPartner ? TipsyIntoxValue : 0;

        Player.Intoxication = Options.TipsyPlayer ? TipsyIntoxValue : 0;
        DartGame.ScoreNeededToWin = (i == 0 ? 501 : 701);
        UIState.inst.SetInteractable(false);
        DartSticker.inst.SetVisible(false);
        DartGame.PartnerIndex = PartnerIndex;
        Transition.BeginTransition(this);
    }

    public void NowHidden() {
        PartnerCanvas.enabled = false;
        ScoreCanvas.enabled = false;
        ScoreAnimationLeaveHead.ReachEndState();
        DartGame.BeginGame();
    }

    public void Ping() {
        switch (AnimationState) {
            case PingState.Tutorial:
                AnimationState = PingState.EnterMain;
                OverallEnterAnimationHead.Begin(this);
                OverallCanvas.enabled = true;
                return;
            case PingState.EnterMain:
               // Debug.Log("here");
                UIState.inst.SetAsSelectedButton(FirstOverallButton);
                break;
            case PingState.EnterScore:
                    UIState.inst.SetAsSelectedButton(FirstScoreButton);
                    break;
            case PingState.ExitScore:
                    ScoreCanvas.enabled = false;
                    UIState.inst.SetAsSelectedButton(FirstPartnerButton);
                    break;
        }
    }

    enum PingState {
        EnterMain,
        EnterScore,
        ExitScore,
        Tutorial,
    }

#if UNITY_EDITOR
    [SerializeField] ColorSwatch __KeyColors;
    [SerializeField] Transform __buttonHolder;
    [SerializeField] float __fontSize;
    [SerializeField] TMPro.FontStyles __style;
    [SerializeField] bool __useAllUpper;
    [SerializeField] bool __reset;

    private void OnValidate() {
        if (__reset) {
            __reset = false;
            TMPro.TMP_Text[] temp = __buttonHolder.GetComponentsInChildren<TMPro.TMP_Text>();
            PartnerButtonImages = new Image[temp.Length];

            if (__fontSize == 0)
                __fontSize = temp[0].fontSize;

            for(int i = 0; i < temp.Length; i++) {
                temp[i].text = __useAllUpper ? Partners.list[i].Name.ToUpper() : Partners.list[i].Name;
                temp[i].fontSize = __fontSize;
                temp[i].fontStyle = __style;
                PartnerButtonImages[i] = temp[i].gameObject.transform.parent.GetComponent<Image>();
                PartnerButtonImages[i].color = __KeyColors.colors[i];
            }
        }
    }
#endif
}

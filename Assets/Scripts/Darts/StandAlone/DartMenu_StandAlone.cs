using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class DartMenu_StandAlone : MonoBehaviour, Caller, SceneEntrance {
    [SerializeField] Player Player;
    [SerializeField] CharacterList Partners;
    [SerializeField] DartGame DartGame;
    [SerializeField] DartMenu_StandAlone_Options Options;
    [SerializeField] float TipsyIntoxValue;
    [SerializeField] ImageFill Fill;
    [SerializeField] ImageSlide Slide;
    [SerializeField] ImageSmoothFill[] BorderFills;
    [SerializeField] UIAnimationElement ScoreAnimationEnterHead;
    [SerializeField] UIAnimationElement ScoreAnimationLeaveHead;
    [SerializeField] Canvas PartnerCanvas;
    [SerializeField] Canvas ScoreCanvas;
    [SerializeField] Image[] PartnerButtonImages;
    [SerializeField] Vector3 PartnerLocationOffset;
    [SerializeField] Image[] ScoreButtonImages;
    [SerializeField] Vector3 ScoreLocationOffset;
    [SerializeField] Image Portrait;
    [SerializeField] int PartnerIndex;
    [SerializeField] bool TurnOffScoreCanvas;
    [SerializeField] GameObject FirstPartnerButton;
    [SerializeField] GameObject FirstScoreButton;
    [SerializeField] GameObject Tutorial;

    public void Start() {
        TransitionManager.inst.ReadyToEnterScene(this);
    }

    public void EnterScene() {
        PauseMenu.inst.SetEnabled(true);
        BeginSetUp();
    }

    public void BeginSetUp() {
        Audio.inst.StopSong();
        PartnerCanvas.enabled = true;
        //EventSystem.current.enabled = true;
        UIState.inst.SetInteractable(true);
        UIState.inst.SetAsSelectedButton(FirstPartnerButton);
    }

    public void StartGameWithPartner(int i) {
        PartnerIndex = i;
        ScoreCanvas.enabled = true;
        ScoreAnimationEnterHead.Begin(this);
        DartSticker.inst.SetVisible(false);
        Fill.ClearImages();
        TurnOffScoreCanvas = false;
    }

    public void ShowCharacterPortrait(int i) {
        if (Portrait.sprite == Partners.list[i].Expressions[0])
            return;

        Audio.inst.PlayClip(AudioClips.Click);
        Portrait.sprite = Partners.list[i].Expressions[0];
        Slide.BeginSlide();
        Fill.SetCurrentImageToFill(PartnerButtonImages[i] , ((RectTransform)PartnerButtonImages[i].transform).position + PartnerLocationOffset);

        float fill = (float)i / (float)(Partners.list.Length - 2);
        foreach (ImageSmoothFill image in BorderFills)
            image.FillTo(fill);
    }

    public void HoverScoreButton(int i) {
        Audio.inst.PlayClip(AudioClips.Click);
        Fill.SetCurrentImageToFill(ScoreButtonImages[i], ((RectTransform)ScoreButtonImages[i].transform).position + ScoreLocationOffset);
    }

    public void BackToPartnerScreen() {
        Fill.ClearImages();
        DartSticker.inst.SetVisible(false);
        TurnOffScoreCanvas = true;
        ScoreAnimationLeaveHead.Begin(this);
    }

    bool CharacterIsAlreadyDrunkCharacter(int i) {
        switch (i) {
            case (int)Characters.CharmingGirl: return true;
            case (int)Characters.DanceGirl: return true;
            case (int)Characters.LoungeGuy: return true;
        }
        return false;  
    }

    public void SetScore(int i) {
        if (!CharacterIsAlreadyDrunkCharacter(PartnerIndex))
            Partners.list[PartnerIndex].Intoxication = Options.TipsyPartner ? TipsyIntoxValue : 0;

        Player.Intoxication = Options.TipsyPlayer ? TipsyIntoxValue : 0;
        DartGame.ScoreNeededToWin = (i == 0 ? 501 : 701);
        PartnerCanvas.enabled = false;
        ScoreCanvas.enabled = false;
        ScoreAnimationLeaveHead.ReachEndState();
        DartSticker.inst.SetVisible(false);
        DartGame.partnerIndex = PartnerIndex;

        if (DartGame.firstTimePlaying)
            showTutorial();
        else
            DartGame.BeginGame();
    }

    public void Ping() {
        if (TurnOffScoreCanvas) {
            ScoreCanvas.enabled = false;
            UIState.inst.SetAsSelectedButton(FirstPartnerButton);
        } else
            UIState.inst.SetAsSelectedButton(FirstScoreButton);
    }

    public void showTutorial() { Tutorial.SetActive(true); }

    public void hideTutorial() {
        Tutorial.SetActive(false);
        DartGame.firstTimePlaying = false;
        UIState.inst.SetInteractable(false);
        DartGame.BeginGame();
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
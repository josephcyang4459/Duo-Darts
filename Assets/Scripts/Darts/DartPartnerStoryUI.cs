using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DartPartnerStoryUI : MonoBehaviour, Caller, TransitionCaller
{
    [SerializeField] InSceneTransition Transition;
    [SerializeField] ColorSwatch KeyColors;
    [SerializeField] CharacterList Partners;
    [SerializeField] InputActionReference Click;
    [SerializeField] CharacterList PartnerList;
    [SerializeField] DartGame DartGame;
    [SerializeField] Schedule Schedule;
    [SerializeField] EventSelectorUI EventSelector;
    [SerializeField] Canvas DartPartnerCanvas;
    [SerializeField] ImageFill Fill;
    [SerializeField] ImageSlide Slide;
    [SerializeField] Image CharacterImage;
    [SerializeField] Transform Corner;
    [SerializeField] Button[] Buttons;
    [SerializeField] Transform[] DartPositions;
    [SerializeField] Image[] ButtonFills;
    [SerializeField] GameObject[] PlayerButtons;
    [SerializeField] Image[] PlayerColor;
    [SerializeField] TMP_Text[] PlayerTexts;
    [SerializeField] float[] DistanceRings;
    [SerializeField] UIAnimationElement EnterDefaultHead;
    [SerializeField] UIAnimationElement ExitDefaultHead;
    [SerializeField] AnimationState State;
    [SerializeField] bool Active;
    [SerializeField] bool UsingController;
    [SerializeField] Mouse Mouse;
    [SerializeField] Vector2 MousePosition;
    [SerializeField] int CurrentRing;
    [SerializeField] int UIcharacterSlotUsed;
    [SerializeField] int[] AdjustedIdices = new int[4];
    [SerializeField] float ScreenRatio;

    bool CheckCanPlay(int partnerIndex) {
        if (Schedule.hour >= 8 && Schedule.minutes >= 30)
            return PartnerList.list[partnerIndex].RelatedCutScenes[(int)PartnerCutscenes.FinalScene].completed;
        return PartnerList.list[partnerIndex].RelatedCutScenes[1].completed;
    }

    public void BeginSetUp() {
        Fill.ClearImages();
        UIState.inst.SetInteractable(false);
            PauseMenu.inst.SetEnabled(false);
        DartPartnerCanvas.enabled = true;
        UIcharacterSlotUsed = 0;
        //turns on all slots
        for (int i = 0; i < (int)CharacterNames.Owner; i++) {
            if (CheckCanPlay(i)) {
                PlayerButtons[UIcharacterSlotUsed].SetActive(true);
                Buttons[UIcharacterSlotUsed + 2].gameObject.SetActive(true);
                PlayerColor[UIcharacterSlotUsed].color = KeyColors.colors[i];
                PlayerTexts[UIcharacterSlotUsed].text = PartnerList.list[i].Name;
                AdjustedIdices[UIcharacterSlotUsed] = i;//used to align the internal character list with the UI representation
                UIcharacterSlotUsed++;
            }
        }
        //sets all other slots off
        for (int i = UIcharacterSlotUsed; i < (int)CharacterNames.Owner; i++) {
            PlayerButtons[i].SetActive(false);
            Buttons[i + 2].gameObject.SetActive(false);
        }
           
        State = AnimationState.Entering;
        EnterDefaultHead.Begin(this);
    }

    int DistanceFromCorner() {
        Vector2 temp = Mouse.position.ReadValue();
        if (temp != MousePosition)
            MousePosition = Mouse.position.ReadValue();
        else
            return CurrentRing;
        float distance = Vector2.Distance(MousePosition, Corner.position);
        for (int i = 0; i < UIcharacterSlotUsed+2; i++) {
            if (distance < DistanceRings[i]*ScreenRatio && Buttons[i].isActiveAndEnabled)
                return i;
        }
        return UIcharacterSlotUsed + 1;
    }

    void IsUsingController(bool b) {
        UsingController = b;
    }

    void SetActive(bool state) {
        if (!state) {
            enabled = false;
            DartPartnerCanvas.enabled = false;
            Fill.ClearImages();
            Slide.SetToStart();
            ControlState.UsingController -= IsUsingController;
            return;
        }
        UIState.inst.SetInteractable(true);
        PauseMenu.inst.SetEnabled(true);
        enabled = true;
        EnableClick();
        ControlState.UsingController += IsUsingController;
        Mouse = Mouse.current;
        UsingController = ControlState.inst.IsUsingController();
        UIState.inst.SetAsSelectedButton(Buttons[1].gameObject);
    }

    void ClickFunction(InputAction.CallbackContext c) {
        Buttons[CurrentRing].onClick.Invoke();
    }

    void EnableClick() {
        Click.action.Enable();
        Click.action.canceled += ClickFunction;
    }

    void UnenableClick() {
        Click.action.Reset();
        Click.action.Disable();
        Click.action.canceled -= ClickFunction;
        Click.action.Reset();
    }


    public void SelectButton(int index) {
        if (Buttons[index].IsActive()) {
            if (index - 2 >= 0) {
                if (CharacterImage.sprite != Partners.list[AdjustedIdices[index - 2]].GetExpression(0) || Slide.IsAtStart()) {
                    CharacterImage.sprite = Partners.list[AdjustedIdices[index - 2]].GetExpression(0);
                    Slide.BeginSlide();
                }
            }
            else {
                Slide.SetToStart();
            }
            Fill.SetCurrentImageToFill(ButtonFills[index], DartPositions[index].position);
        }
    }

    public void ShowTutorial() {
        State = AnimationState.Tutorial;
        enabled = false;
        TutorialHandler.inst.EnableDartsTutorial(true, this);
    }

    public void BackToEventPicker() {
        UnenableClick();
        State = AnimationState.ExitingToEvent;
        UIState.inst.SetInteractable(false);
        PauseMenu.inst.SetEnabled(false);
        ExitDefaultHead.Begin(this);
    }

    public void SetPartner(int i) {
        UnenableClick();
        DartGame.ScoreNeededToWin = Schedule.hour < 7 ? 501 : 701;
        DartGame.PartnerIndex = AdjustedIdices[i];
        UIState.inst.SetInteractable(false);
        DartSticker.inst.SetVisible(false);
        State = AnimationState.ExitingToGame;
        ExitDefaultHead.Begin(this);
    }

    public void ForceDartsException(int characterIndex, int currentHour) {
        Schedule.TurnLocationAndEventSelectorUIOff();
        DartGame.ScoreNeededToWin = currentHour < 7 ? 501 : 701;
        DartGame.PartnerIndex = characterIndex;
        DartGame.BeginGame();
    }

    private void FixedUpdate() {
        ScreenRatio = Screen.width / 1920f;
        int temp = DistanceFromCorner();
        if (temp != CurrentRing) {
            CurrentRing = temp;
            if (UsingController)
                UIState.inst.SetAsSelectedButton(Buttons[CurrentRing].gameObject);
            else
                SelectButton(CurrentRing);
        }
    }

    private void OnDestroy() {
        UnenableClick();
    }

    public void Ping() {
        if(State == AnimationState.Entering) {
            SetActive(true);
            return;
        }

        if(State == AnimationState.ExitingToEvent) {
            SetActive(false);
            UIState.inst.SetInteractable(true);
            PauseMenu.inst.SetEnabled(true);
            EventSelector.SelectFirstButton();
            return;
        }
        if(State == AnimationState.ExitingToGame) {
            SetActive(false);
            Transition.BeginTransition(this);
            return;
        }

        if(State == AnimationState.Tutorial) {
            enabled = true;
            if (UsingController)
                UIState.inst.SetAsSelectedButton(Buttons[CurrentRing].gameObject);
            else
                SelectButton(CurrentRing);
            return;
        }
    }

    public void NowHidden() {
       
        DartGame.BeginGame();
        Schedule.TurnLocationAndEventSelectorUIOff();
    }

    enum AnimationState {
        Entering,
        ExitingToEvent,
        ExitingToGame,
        Tutorial,
    }

#if UNITY_EDITOR
    [SerializeField] bool __set;

    private void OnValidate() {
        if (__set) {
            __set = false;
        }
    }

#endif
}

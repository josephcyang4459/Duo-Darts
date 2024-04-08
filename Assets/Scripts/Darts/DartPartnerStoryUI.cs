using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DartPartnerStoryUI : MonoBehaviour, Caller
{
    [SerializeField] ColorSwatch KeyColors;
    [SerializeField] CharacterList Partners;
    [SerializeField] InputActionReference Click;
    [SerializeField] CharacterList PartnerList;
    [SerializeField] DartGame DartGame;
    [SerializeField] Schedule Schedule;
    [SerializeField] Canvas DartPartnerCanvas;
    [SerializeField] ImageFill Fill;
    [SerializeField] ImageSlide Slide;
    [SerializeField] Image CharacterImage;
    [SerializeField] Vector2 Corner;
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
    [SerializeField] int[] AdjustedIdices = new int[4];

    private void Start() {
        enabled = false;
        BeginSetUp();
    }

    public bool CheckCanPlay(int partnerIndex) {
        if (Schedule.hour >= 8 && Schedule.minutes >= 30)
            return PartnerList.list[partnerIndex].RelatedCutScenes[(int)PartnerCutscenes.FinalScene].completed;
        return PartnerList.list[partnerIndex].RelatedCutScenes[0].completed;
    }

    public void BeginSetUp() {

        //DartGame.ScoreNeededToWin = Schedule.hour < 7 ? 501 : 701;//gets correct score
        //Schedule.off();
        DartPartnerCanvas.enabled = true;
        int UIcharacterSlotUsed = 0;

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

        float distance = Vector2.Distance(MousePosition, Corner);
        for (int i = 0; i < DistanceRings.Length; i++) {
            if (distance < DistanceRings[i])
                return i;
        }
        return DistanceRings.Length - 1;
    }

    void IsUsingController(bool b) {
        UsingController = b;
        //enabled = !UsingController;
    }

    void SetActive(bool state) {
        if (!state) {
            enabled = false;
            ControlState.UsingController -= IsUsingController;
            UnenableClick();
            return;
        }
        enabled = true;
        EnableClick();
        ControlState.UsingController += IsUsingController;
        Mouse = Mouse.current;
        UsingController = ControlState.inst.IsUsingController();
        if (UsingController) {
            UIState.inst.SetAsSelectedButton(Buttons[1].gameObject);
        }
    }

    void ClickFunction(InputAction.CallbackContext c) {
        Buttons[CurrentRing].onClick.Invoke();
    }

    void EnableClick() {
        Click.action.Enable();
        Click.action.performed += ClickFunction;
    }

    void UnenableClick() {
        Click.action.Disable();
        Click.action.performed -= ClickFunction;
    }


    public void SelectButton(int index) {
        
        if (Buttons[index].IsActive()) {
            if (index-2 >= 0) {
                CharacterImage.sprite = Partners.list[AdjustedIdices[index-2]].Expressions[0];
                Slide.BeginSlide();
            }
            else
                Slide.SetToStart();
                
            Fill.SetCurrentImageToFill(ButtonFills[index], DartPositions[index].position);
        }
            
    }

    public void ShowTutorial() {

    }

    public void BackToEventPicker() {

        State = AnimationState.Exiting;
        ExitDefaultHead.Begin(this);
    }

    public void SetPartner(int i) {
        DartGame.partnerIndex = AdjustedIdices[i];
        DartGame.BeginGame();
    }

    private void FixedUpdate() {
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
        if(State == AnimationState.Exiting) {
            SetActive(false);
            return;
        }
    }

    enum AnimationState {
        Entering,
        Exiting
    }

#if UNITY_EDITOR
    [SerializeField] bool __set;
    [SerializeField] Transform __Corner;
    private void OnValidate() {
        if (__set) {
            __set = false;
            Corner = __Corner.position;
        }
    }

#endif
}

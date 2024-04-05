using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventSelectorUI : MonoBehaviour, Caller
{
    [SerializeField] LocationSelecterUI LocationUI;
    [SerializeField] Canvas EventSelectorCanvas;
    [SerializeField] Canvas EventButtonsCanvas;
    [SerializeField] Image[] FirstImage;
    [SerializeField] Image[] SecondImage;
    [SerializeField] Image[] Banners;
    [SerializeField] UIA_MultiImageFill FillPrimary;
    [SerializeField] UIA_MultiImageFill FillSecondary;
    [SerializeField] UIA_MultiImageFill EmptySecondary;
    [SerializeField] UIA_MultiImageFill EmptyBanner;
    [SerializeField] UIAnimationElement EnterButtons;
    [SerializeField] UIAnimationElement ExitButtons;
    [SerializeField] UIAnimationElement ExitListAnimation;
    [SerializeField] GameObject[] LocationPlates;
    [SerializeField] Fillable_Image[] FillGroups;
    [SerializeField] GroupImageFill Fill;
    [SerializeField] AnimationState State;
    public void Ping() {

        switch (State) {
            case AnimationState.EnterList: ShowButtons();return;
            case AnimationState.EnterButtons: UIState.inst.SetInteractable(true);return;
            case AnimationState.ExitButtons:
                State = AnimationState.ExitList;
                ExitListAnimation.Begin(this);
                return;
            case AnimationState.ExitList:
                EventButtonsCanvas.enabled = false;
                EventSelectorCanvas.enabled = false;
                LocationUI.BeginEntrance();
                return;
        }
    }

    public void HideUI() {
        ExitButtons.ReachEndState();
        ExitListAnimation.ReachEndState();
        EventSelectorCanvas.enabled = false;
        EventButtonsCanvas.enabled = false;
    }

    void ShowButtons() {
        State = AnimationState.EnterButtons;
        EventButtonsCanvas.enabled = true;
        EnterButtons.Begin(this);
    }

    public void SelectEventButton(int i) {
        Fill.SetCurrentImageToFill(FillGroups[i]);
    }
    
    public void BackToLocations() {
        State = AnimationState.ExitButtons;
        ExitButtons.Begin(this);
    }


    public void SetLocation(int locationIndex) {
        UIState.inst.SetInteractable(false);
        State = AnimationState.EnterList;
        EventSelectorCanvas.enabled = true;
        EventButtonsCanvas.enabled = true;
        foreach (GameObject g in LocationPlates)
            g.SetActive(false);
        LocationPlates[locationIndex].SetActive(true);
        Banners[locationIndex].fillAmount = 1;
        FirstImage[locationIndex].fillAmount = 1;
        EmptyBanner.Images[0] = Banners[locationIndex];
        FillPrimary.Images[0] = FirstImage[locationIndex];
        FillSecondary.Images[0] = SecondImage[locationIndex];
        EmptySecondary.Images[0] = SecondImage[locationIndex];
        FillPrimary.Begin(this);
    }

    enum AnimationState {
        EnterList,
        EnterButtons,
        ExitButtons,
        ExitList
    }
}

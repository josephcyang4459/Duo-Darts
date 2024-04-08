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
    [SerializeField] UIA_MultiImageFill EmptyBanner;
    [SerializeField] UIAnimationElement EnterButtons;
    [SerializeField] UIAnimationElement ExitButtons;
    [SerializeField] UIAnimationElement ExitListAnimation;
    [SerializeField] GameObject[] LocationPlates;
    [SerializeField] Image[] Fills;
    [SerializeField] Vector3 DartOffset;
    [SerializeField] ImageFill Fill;
    [SerializeField] GameObject FirstSelected;
    [SerializeField] AnimationState State;
    public void Ping() {

        switch (State) {
            case AnimationState.EnterList: ShowButtons();return;
            case AnimationState.EnterButtons: 
                UIState.inst.SetInteractable(true); 
                PauseMenu.inst.SetEnabled(true);
                UIState.inst.SetAsSelectedButton(FirstSelected);
                return;
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
        Fill.ClearImages();
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
        Fill.SetCurrentImageToFill(Fills[i], Fills[i].transform.position+DartOffset);
    }
    
    public void BackToLocations() {
        PauseMenu.inst.SetEnabled(false);
        UIState.inst.SetInteractable(false);
        DartSticker.inst.SetVisible(false);
        Fill.ClearImages();
        State = AnimationState.ExitButtons;
        ExitButtons.Begin(this);
    }


    public void SetLocation(int locationIndex) {
        UIState.inst.SetInteractable(false);
        PauseMenu.inst.SetEnabled(false);
        State = AnimationState.EnterList;
        Fill.ClearImages();
        EventSelectorCanvas.enabled = true;
        EventButtonsCanvas.enabled = true;
        foreach (GameObject g in LocationPlates)
            g.SetActive(false);
        foreach(Image i in SecondImage) {
            i.enabled = false;
        }
        LocationPlates[locationIndex].SetActive(true);
        SecondImage[locationIndex].enabled = true;
        Banners[locationIndex].fillAmount = 1;
        FirstImage[locationIndex].fillAmount = 1;
        EmptyBanner.Images[0] = Banners[locationIndex];
        FillPrimary.Images[0] = FirstImage[locationIndex];
        FillPrimary.Begin(this);
    }

    enum AnimationState {
        EnterList,
        EnterButtons,
        ExitButtons,
        ExitList
    }
}

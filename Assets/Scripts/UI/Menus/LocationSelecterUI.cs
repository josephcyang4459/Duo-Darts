using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocationSelecterUI : MonoBehaviour, Caller {
    [SerializeField] Schedule Schedule;
    [SerializeField] Canvas LocationSelectorCanvas;
    [SerializeField] Canvas BackGroundImageCanvas;
    [SerializeField] EventSelectorUI EventSelector;
    [SerializeField] UIAnimationElement EnterAnimationHead;
    [SerializeField] UIAnimationElement ExitAnimationHead;
    [SerializeField] AnimationState State;
    [SerializeField] ImageFill Fill;
    [SerializeField] Vector3 DartOffset;
    [SerializeField] Image[] FillImages;
    [SerializeField] Transform[] DartPositions;
    [SerializeField] Image BackgroundImage;
    [SerializeField] SpriteCollection SpriteCollection;
    [SerializeField] GameObject FirstSelected;
    int SelectedLocation;
    public void Start() {
        State = AnimationState.Passive;
        SelectedLocation = 0;
    }

    public void SelectButton(int i) {
        BackgroundImage.sprite = SpriteCollection.Sprites[i];
        Fill.SetCurrentImageToFill(FillImages[i], DartPositions[i].position+DartOffset);
    }

    public void HideUI() {
        LocationSelectorCanvas.enabled = false; 
        BackGroundImageCanvas.enabled =false;
    }

    public void BeginGoToLocation(int index) {
        ExitAnimationHead.ReachEndState();
        SelectedLocation = index;
        LocationSelectorCanvas.enabled = false;
        Fill.ClearImages();
        Schedule.SetEventsForLocation(index);
        EventSelector.SetLocation(index);
    }

    public void BeginEntrance() {
        BackGroundImageCanvas.enabled = true;
        LocationSelectorCanvas.enabled = true;
        PauseMenu.inst.SetEnabled(false);
        UIState.inst.SetInteractable(false);
        State = AnimationState.Enter;
        EnterAnimationHead.Begin(this);
    }

    public void BeginExit() {
        PauseMenu.inst.SetEnabled(false);
        DartSticker.inst.SetVisible(false);
        UIState.inst.SetInteractable(false);
        State = AnimationState.Exit;
    }

    void Enter() {
        
        PauseMenu.inst.SetEnabled(true);
        UIState.inst.SetInteractable(true);
        UIState.inst.SetAsSelectedButton(FirstSelected);

    }

    void Exit() {
        
    }

    public void Ping() {
        switch (State) {
            case AnimationState.Enter: Enter(); break;
            case AnimationState.Exit: Exit(); break;
        }
        State = AnimationState.Passive;
    }

    enum AnimationState {
        Passive,
        Enter,
        Exit
    }
#if UNITY_EDITOR
    [SerializeField] bool __testState;

    void OnValidate() {
        if (__testState) {
            __testState = false;
        }
    }

#endif
}

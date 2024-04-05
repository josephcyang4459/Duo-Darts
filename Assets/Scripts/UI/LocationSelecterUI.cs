using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocationSelecterUI : MonoBehaviour, Caller {
    [SerializeField] Schedule Schedule;
    [SerializeField] Canvas LocationSelectorCanvas;
    [SerializeField] EventSelectorUI EventSelector;
    [SerializeField] UIAnimationElement EnterAnimationHead;
    [SerializeField] UIAnimationElement ExitAnimationHead;
    [SerializeField] AnimationState State;
    [SerializeField] ImageFill Fill;
    [SerializeField] Image[] FillImages;
    [SerializeField] Image BackgroundImage;
    [SerializeField] SpriteCollection SpriteCollection;
    int SelectedLocation;
    public void Start() {
        State = AnimationState.Passive;
        SelectedLocation = 0;
    }

    public void SelectButton(int i) {
        BackgroundImage.sprite = SpriteCollection.Sprites[i];
        Fill.SetCurrentImageToFill(FillImages[i]);
    }

    public void BeginGoToLocation(int index) {
        ExitAnimationHead.ReachEndState();
        SelectedLocation = index;
        LocationSelectorCanvas.enabled = false;
        Schedule.SetEventsForLocation(index);
        EventSelector.SetLocation(index);
    }

    public void BeginEntrance() {
        LocationSelectorCanvas.enabled = true;
        UIState.inst.SetInteractable(false);
        State = AnimationState.Enter;
        EnterAnimationHead.Begin(this);
    }

    public void BeginExit() {
        UIState.inst.SetInteractable(false);
        State = AnimationState.Exit;
    }

    void Enter() {
        UIState.inst.SetInteractable(true);

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

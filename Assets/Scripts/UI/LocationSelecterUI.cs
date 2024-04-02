using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocationSelecterUI : MonoBehaviour, Caller {
    [SerializeField] UIAnimationElement EnterAnimationHead;
    [SerializeField] UIAnimationElement ExitAnimationHead;
    [SerializeField] AnimationState State;
    [SerializeField] ImageFill Fill;
    [SerializeField] Image[] FillImages;
    public Button b;
    int SelectedLocation;
    public void Start() {
        State = AnimationState.Passive;
        BeginEntrance();
        SelectedLocation = 0;
    }

    public void SelectButton(int i) {
        Fill.SetCurrentImageToFill(FillImages[i]);
    }

    public void BeginGoToLocation(int index) {
        SelectedLocation = index;
        BeginExit();
    }

    public void BeginEntrance() {
        UIState.inst.SetInteractable(false);
        State = AnimationState.Enter;
        EnterAnimationHead.Begin(this);
    }

    public void BeginExit() {
        UIState.inst.SetInteractable(false);
        State = AnimationState.Exit;
        ExitAnimationHead.Begin(this);
    }

    void Enter() {
        Debug.Log("Entered");
      
    }

    void Exit() {
        Debug.Log("Exited");
    }

    public void Ping() {
        UIState.inst.SetInteractable(true);
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
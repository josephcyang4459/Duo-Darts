using UnityEngine;
using UnityEngine.UI;

public class DefaultCutsceneUI : MonoBehaviour, Caller {
    [SerializeField] Canvas UI;
    [SerializeField] UIAnimationElement EnterHead;
    [SerializeField] UIAnimationElement ExitHead;
    [SerializeField] Image[] ImageFills;
    [SerializeField] Transform[] DartTargets;
    [SerializeField] ImageFill Fill;
    [SerializeField] int Choice;
    [SerializeField] AnimationState State;
    [SerializeField] UIType Type;
    [SerializeField] GameObject FirstSelectedButton;
    
    public void HideUI(){
        UI.enabled = false;
        ExitHead.ReachEndState();
    }

    public void SelectButton(int index) {
        Fill.SetCurrentImageToFill(ImageFills[index], DartTargets[index].position);
    }

    public void SetChoice(int index) {
        Choice = index;
        DartSticker.inst.SetVisible(false);
        BeginExit();
    }

    public void BeginEnter() {
        UI.enabled = true;
        UIState.inst.SetInteractable(false);
        State = AnimationState.Entering;
        EnterHead.Begin(this);
    }

    public void BeginExit() {
        UIState.inst.SetInteractable(false);
        State = AnimationState.Exiting;
        ExitHead.Begin(this);
    }

    void Enter() {
        UIState.inst.SetInteractable(true);
        UIState.inst.SetAsSelectedButton(FirstSelectedButton);
    }

    void Exit() {
        UI.enabled = false;
        switch (Type) {
            case UIType.Default: CutsceneHandler.Instance.DefaultCutsceneSelection(Choice);return;
            case UIType.Response: CutsceneHandler.Instance.UI_Response(Choice); return;
        }
       
    }

    public void Ping() {
        switch (State) {
            case AnimationState.Entering: Enter(); return;
            case AnimationState.Exiting: Exit(); return;
        }
    }

    enum AnimationState {
        Entering,
        Exiting
    }
    enum UIType {
        Default,
        Response
    }
}

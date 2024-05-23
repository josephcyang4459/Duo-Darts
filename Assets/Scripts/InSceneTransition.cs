using UnityEngine;

public class InSceneTransition : MonoBehaviour, Caller
{

    [SerializeField] Canvas Canvas;
    [SerializeField] UIAnimationElement EnterAnimationHead;
    [SerializeField] UIAnimationElement ExitAnimationHead;
    [SerializeField] AnimationState State;
    [SerializeField] TransitionCaller Destination;

    public void BeginTransition(TransitionCaller caller) {
        Destination = caller;
        Canvas.enabled = true;
        State = AnimationState.Entering;
        DartSticker.inst.SetVisible(false);
        EnterAnimationHead.Begin(this);
    }

    public void Ping() {
        if(State == AnimationState.Entering) {//scene is now hidden
            Destination.NowHidden();
            State = AnimationState.Exiting;
            ExitAnimationHead.Begin(this);
        }
        else {// scene is now Visible
            Canvas.enabled = false;
        }
    }

enum AnimationState {
        Entering,
        Exiting,
    }    

}



public enum Destination {
    CutScene,
    Darts,
    DartsFinals
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIA_SetGameObjectEnabled : UIAnimationElement
{
    [SerializeField] GameObject[] Objects;
    [SerializeField] NewState IntendedState = NewState.Auto;
    public override void ReachEndState()
    {
        Action();
        PassToNextEndState();
    }

    void Action() {
        bool state = false;
        switch (IntendedState) {
            case NewState.Auto: state = !Objects[0].activeSelf; break;
            case NewState.Active: state = true; break;
            case NewState.Inactive: state = false; break;
        }
        for (int i = 0; i < Objects.Length; i++)
            Objects[i].SetActive(state);
    }

    public override void Begin(Caller caller)
    {
        Caller = caller;
        Action();
        Pass();
    }

    enum NewState {
        Auto,
        Active,
        Inactive
    }
}

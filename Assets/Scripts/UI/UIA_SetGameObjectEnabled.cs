using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIA_SetGameObjectEnabled : UIAnimationElement
{
    [SerializeField] GameObject[] Objects;

    public override void ReachEndState()
    {
        bool state = !Objects[0].activeSelf;
        for (int i = 0; i < Objects.Length; i++)
            Objects[i].SetActive(state);

        Pass();
    }

    public override void Begin(Caller caller)
    {
        Caller = caller;
        ReachEndState();
    }
}

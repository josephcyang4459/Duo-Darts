using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIA_MultiImageFill : UIAnimationElement
{
    public Image[] Images;
    float CurrentFill;
    float TargetFill;
    [SerializeField] float Speed;
    [SerializeField] [Range(0,2)] float SubsequentOffset;
    [SerializeField] Direction Behavior = Direction.Auto;

    float GetDirection() {
        if (Behavior == Direction.Auto)
            return CurrentFill < .1 ? 1 : 0;
        if (Behavior == Direction.Fill)
            return 1;
        return 0;
    }

    public override void Begin(Caller caller) {
        Caller = caller;
        CurrentFill = Images[0].fillAmount;
        TargetFill = GetDirection();

        enabled = true;
    }

    void End()
    {
        enabled = false;
        Pass();
    }

    public override void ReachEndState()
    {
        CurrentFill = GetDirection();
        for (int i = 0; i < Images.Length; i++)
            Images[i].fillAmount = CurrentFill;
        PassToNextEndState();
    }



    public void Update()
    {
        CurrentFill = Mathf.MoveTowards(CurrentFill, TargetFill, Speed * Time.deltaTime);
        for (int i = 0; i < Images.Length; i++)
            Images[i].fillAmount = Mathf.Clamp01(CurrentFill * (1+(SubsequentOffset * i)));
        if (CurrentFill == TargetFill)
            End();
    }

    enum Direction {
        Auto,
        Fill,
        Empty
    }
}

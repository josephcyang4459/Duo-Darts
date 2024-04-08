using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIA_MultiImageFill : UIAnimationElement
{
    public Image[] Images;
    float CurrentFill;
    float TargetFill;
    float FillDirection;
    [SerializeField] float Speed;
    [SerializeField] [Range(-2,2)] float SubsequentOffset;
    [SerializeField] Direction Behavior = Direction.Auto;

    float GetDirection() {
        if (Behavior == Direction.Auto)
            return CurrentFill < .1 ? 1 : 0;
        if (Behavior == Direction.Fill) {
            return 1;
        }
          
        return 0;
    }

    public override void Begin(Caller caller) {
        Caller = caller;
        CurrentFill = Images[0].fillAmount;
        TargetFill = GetDirection();
        FillDirection = TargetFill == 1 ? 1 : -1;
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

    int CurrentNonTargetFill() {
        for (int i = 0; i < Images.Length; i++)
            if (Images[i].fillAmount != TargetFill)
                return i;
        return -1;
    }

    public void Update()
    {      
        CurrentFill += Speed * Time.deltaTime * FillDirection;
        for (int i = 0; i < Images.Length; i++)
            Images[i].fillAmount = Mathf.Clamp01(CurrentFill * (1+(SubsequentOffset * i)));

        if (Images[^1].fillAmount == 1) {
            End();
            return;
        }
    }

    enum Direction {
        Auto,
        Fill,
        Empty
    }
}

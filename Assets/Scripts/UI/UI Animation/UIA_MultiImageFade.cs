using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIA_MultiImageFade : UIAnimationElement {
    [SerializeField] Image[] Images;
    float CurrentFill;
    float TargetFill;
    [SerializeField] float Speed;
    float ChangeDirection;
    [SerializeField] [Range(-2, 2)] float SubsequentOffset;
    [SerializeField] Direction Behavior = Direction.Auto;

    float GetTarget() {
        if (Behavior == Direction.Auto)
            return CurrentFill < .1 ? 1 : 0;
        if (Behavior == Direction.Fill) {
            for (int i = 0; i < Images.Length; i++)
                SetImageAlpha(Images[i], 0);
            return 1;
        }
        for (int i = 0; i < Images.Length; i++)
            SetImageAlpha(Images[i], 1);
        return 0;
    }

    public override void Begin(Caller caller) {
        Caller = caller;
        TargetFill = GetTarget();
        CurrentFill = Images[0].color.a;
        ChangeDirection = TargetFill > .5 ? 1 : -1;
        enabled = true;
    }

    void End() {
        enabled = false;
        Pass();
    }

    public override void ReachEndState() {
        CurrentFill = GetTarget();
        for (int i = 0; i < Images.Length; i++)
            SetImageAlpha(Images[i], CurrentFill);
        PassToNextEndState();
    }

    void SetImageAlpha(Image i, float alpha) {
        Color c = i.color;
        c.a = alpha;
        i.color = c;
    }

    public void Update() {
        CurrentFill += Speed * Time.deltaTime * ChangeDirection;
        for (int i = 0; i < Images.Length; i++)
            SetImageAlpha(Images[i], Mathf.Clamp01(CurrentFill * (1 + (SubsequentOffset * i))));
        if (Images[0].color.a == TargetFill && Images[^1].color.a == TargetFill)
            End();
    }

    enum Direction {
        Auto,
        Fill,
        Empty
    }
}

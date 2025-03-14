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

    float GetTargetFill() {
        if (Behavior == Direction.Auto)
            return CurrentFill < .1 ? 1 : 0;
        if (Behavior == Direction.Fill) {
            for (int i = 0; i < Images.Length; i++) {
                Images[i].fillAmount = 0;
            }
            return 1;
        }

        for (int i = 0; i < Images.Length; i++) {
            if (Images[i] != null)//Why is this ever Null? what? this causes crash in only one place idk why
                Images[i].fillAmount = 1;// this crashes on trying to load final darts game
        }
        return 0;
    }

    public override void Begin(Caller caller) {
        Caller = caller;
        CurrentFill = Images[0].fillAmount;
        TargetFill = GetTargetFill();
        FillDirection = TargetFill > .5f ? 1 : -1;
        enabled = true;
    }

    void End()
    {
        enabled = false;
        Pass();
    }

    public override void ReachEndState() {
        CurrentFill = GetTargetFill();
        for (int i = 0; i < Images.Length; i++)
            if (Images[i] != null)
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

        if (Images[^1].fillAmount == TargetFill && Images[0].fillAmount == TargetFill) {
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

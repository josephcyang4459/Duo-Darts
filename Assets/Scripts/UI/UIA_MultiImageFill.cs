using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIA_MultiImageFill : UIAnimationElement
{
    [SerializeField] Image[] Images;
    float CurrentFill;
    float TargetFill;
    [SerializeField] float Speed;
    [SerializeField] [Range(0,2)] float SubsequentOffset;
    public override void Begin(Caller caller)
    {
        Caller = caller;
        CurrentFill = Images[0].fillAmount;
        if (CurrentFill < .1f)
        {
            TargetFill = 1;
           
        }
        else
        {
            TargetFill = 0;
        }
        enabled = true;
    }

    void End()
    {
        enabled = false;
        Pass();
    }

    public override void ReachEndState()
    {
        CurrentFill = CurrentFill<.1f?1:0;
        for (int i = 0; i < Images.Length; i++)
            Images[i].fillAmount = CurrentFill;
        End();
    }



    public void Update()
    {
        CurrentFill = Mathf.MoveTowards(CurrentFill, TargetFill, Speed * Time.deltaTime);
        for (int i = 0; i < Images.Length; i++)
            Images[i].fillAmount = Mathf.Clamp01(CurrentFill * (1+(SubsequentOffset * i)));
        if (CurrentFill == TargetFill)
            End();
    }

}

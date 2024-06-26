using UnityEngine;

public class UIA_Timer : UIAnimationElement
{
    [SerializeField] float TimeLimit;
    float CurrentTime;
    public override void Begin(Caller caller)
    {
        Caller = caller;
        CurrentTime = 0;
        enabled = true;
    }

    public override void ReachEndState()
    {
        PassToNextEndState();
    }

    private void FixedUpdate()
    {
        CurrentTime += .02f;
        if(CurrentTime>=TimeLimit)
        {
            enabled = false;
            Pass();
        }
    }
}

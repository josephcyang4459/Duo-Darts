using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIAnimationElement : MonoBehaviour
{
    protected Caller Caller;
    [SerializeField] protected UIAnimationElement CallWhenComplete;
    public abstract void Begin(Caller caller);

    public abstract void ReachEndState();


    public void Pass()
    {
        if (CallWhenComplete != null)
            CallWhenComplete.Begin(Caller);
        else if (Caller != null)
        {
            Caller.Ping();
        }

    }
}

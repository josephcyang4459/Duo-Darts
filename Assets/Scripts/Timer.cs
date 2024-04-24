using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    [SerializeField] float SecondsToWaitFor;
    [SerializeField] float CurrentTime;
    [SerializeField] UnityEvent Complete;

    public void BeginTimer(float wait) {
        SecondsToWaitFor = wait;
        BeginTimer();
    }
    public void BeginTimer() {
        CurrentTime = 0;
        enabled = true;
    }

    private void FixedUpdate() {
        CurrentTime += .02f;
        if (CurrentTime >= SecondsToWaitFor) {
            Complete.Invoke();
            enabled = false;
        }
    }
}

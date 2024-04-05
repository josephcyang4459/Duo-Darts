using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartPlayerAim_Drift : MonoBehaviour
{
    [SerializeField] DartPlayerAim Aim;
    [SerializeField] DartsSettings Settings;
    [SerializeField] float DriftSpeed;
    [SerializeField] Vector2 center;
    [SerializeField] Vector2 XBounds;
    [SerializeField] Vector2 YBounds;
    [SerializeField] float TimeToNewLocation;
    [SerializeField] Vector2 directionMove;
    float Timer;
    [SerializeField] bool recovering;

    public void SetUp(float intoxication, float skill) {
        float tempSpeed = ((intoxication * Settings.DriftSpeedIntoxicationWeight) - skill) / Settings.DriftSpeedFactor;
        DriftSpeed = Mathf.Clamp(tempSpeed, Settings.MinDriftSpeed, Settings.MaxDriftSpeed);
        float tempTime = 3 - (intoxication) / 3;
        TimeToNewLocation = Mathf.Clamp(tempTime, .5f, 3);
        NewRandomDirection();
    }

    public void NewRandomDirection() {
        recovering = false;
        directionMove.x = Random.Range(-DriftSpeed, DriftSpeed);
        directionMove.y = Random.Range(-DriftSpeed, DriftSpeed);
        Timer = 0;
    }

    public void RecoverFromOutOfBounds() {
        recovering = true;
        directionMove.x = Mathf.Clamp(center.x - Aim.CurrentLocation.x, -DriftSpeed, DriftSpeed);
        directionMove.y = Mathf.Clamp(center.y - Aim.CurrentLocation.y, -DriftSpeed, DriftSpeed);
        Timer = 0;
        /*
        directionMove.x = Mathf.Clamp(directionMove.x, -moveSpeed, moveSpeed);
        directionMove.y = Mathf.Clamp(directionMove.y, -moveSpeed, moveSpeed);*/
    }

    bool OutOfBounds(float x, float y) {
        if (x < XBounds.x)
            return true;
        if (x > XBounds.y)
            return true;
        if (y < YBounds.x)
            return true;
        if (y > YBounds.y)
            return true;
        return false;
    }

    public void UpdateDrift(float dTime) {
        Aim.ChangeLocation(directionMove.x * dTime, directionMove.y * dTime);

        if (OutOfBounds(Aim.CurrentLocation.x, Aim.CurrentLocation.y))
            RecoverFromOutOfBounds();

        Timer += dTime;
        if(Timer>TimeToNewLocation) {
            NewRandomDirection();
        }
    }
}

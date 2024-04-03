using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartPlayerAim_Bloom : MonoBehaviour
{
    [SerializeField] DartPlayerAim Aim;
    public float BloomSpeed;
    public Transform t;
    public float BloomMax;
    public float BloomMin;
    public float CurrentBloom;
    public float TargetBloom;
    public Vector3 Size;
    public void SetUp(float luck) {
        BloomMin = Aim.MinBloomDiameter;
        BloomMax = Mathf.Clamp(Aim.MaxBloomDiameter - (luck / 10), BloomMin*2, Aim.MaxBloomDiameter);
        CurrentBloom = (BloomMax - BloomMin) / 2;
        TargetBloom = BloomMax;
        Size.x = CurrentBloom;
        Size.y = CurrentBloom;
        Size.z = 1;
        t.localScale = Size;
    }

    public void UpdateBloom(float dTime) {
        CurrentBloom = Mathf.MoveTowards(CurrentBloom, TargetBloom, BloomSpeed * dTime);
        
        if(CurrentBloom == TargetBloom) {
            TargetBloom = TargetBloom == BloomMax ? BloomMin : BloomMax;
        }
        Size.x = CurrentBloom;
        Size.y = CurrentBloom;
        Size.z = 1;
        t.localScale = Size;
    }

    public float GetCurrentBloom() {
        return CurrentBloom;
    }
}

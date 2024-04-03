using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartPlayerAim_Bloom : MonoBehaviour
{
    [SerializeField] DartPlayerAim Aim;
    [SerializeField] DartsSettings Settings;
    [SerializeField] Player Player;
    public float BloomSpeed;
    public Transform t;
    public float BloomMax;
    public float BloomMin;
    public float CurrentBloom;
    public float TargetBloom;
    public Vector3 Size;
    public void SetUp() {
        BloomMin = Mathf.Clamp(Settings.BaseBloomDiameter - (Player.Luck / Settings.MinBloomLuckWeight), 0, Settings.BaseBloomDiameter);
        BloomMax = Mathf.Clamp(Settings.MaxBloomDiameter - (Player.Skill / Settings.MaxBloomSkillWeight), BloomMin*2, Settings.MaxBloomDiameter);
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

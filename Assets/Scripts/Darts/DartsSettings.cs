using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Darts Settings", menuName ="Reference/Darts Settings")]
public class DartsSettings : ScriptableObject
{
    public LayerMask DartsLayerMask;
    public float MinDriftSpeed;//2
    public float MaxDriftSpeed;//3
    public float BaseBloomDiameter;//.1f
    public float MaxBloomDiameter;//1
    [Header("Luck is divided by this")]
    public float MinBloomLuckWeight;
    [Header("Skill is divided by this")]
    public float MaxBloomSkillWeight;
    [Header("Intoxication is multipled by this")]
    public float DriftSpeedIntoxicationWeight;
    [Header("((Intoxication * IntoxWeight) - Skill) / this")]
    public float DriftSpeedFactor;
    public float BaseTimeToNewLocation;
    public float IntoxicationTiers;
    public float IntoxicationTimeChangeMin;
}


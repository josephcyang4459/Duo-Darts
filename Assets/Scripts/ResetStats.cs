using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Reset Stats", menuName = "Testing Tools/Stat Changer")]
public class ResetStats : ScriptableObject
{
    [Header("Chad, Jess, Faye, Elaine")]
    [SerializeField] Partner[] partners;
    [SerializeField] Player Player;
    [SerializeField] __BaseStats[] BaseStats = new __BaseStats[4];
    [SerializeField] __BasePlayerStats BasePlayerStats;
    [SerializeField] EventStart[] Events;
    public bool ResetToBase;
    public bool ResetToZero;
    public bool ResetToBaseAllButLoveAndIntoxication;

    public void OnValidate()
    {

        for(int i = 0; i < 4; i++)
        {
            BaseStats[i].name = ((Characters)i).ToString();
        }

        if(ResetToBase)
        {
            __resetToBase();
            resetEvents();
        }

        if (ResetToZero)
        {
            __resetToZero();
            resetEvents();
        }

        if (ResetToBaseAllButLoveAndIntoxication)
        {
            __resetToExceptComposure();
            resetEvents();
        }

    }

    private void resetEvents()
    {
       
        for( int i = 0; i < Events.Length; i++)
        {
            Events[i].done = false;
        }
    }
    
    private void playerBase()
    {
        Player.Intoxication = BasePlayerStats.Intoxication;
        Player.Luck = BasePlayerStats.Luck;
        Player.Skill = BasePlayerStats.Skill;
    }

    private void __resetToBase()
    {
        ResetToBase = false;
        for (int i = 0; i < 4; i++)
        {
            partners[i].__resetValues(BaseStats[i].Love, BaseStats[i].Intoxication, BaseStats[i].Composure);
        }
        partners[4].__resetValues();
        playerBase();
       
    }

    private void __resetToExceptComposure()
    {
        ResetToBaseAllButLoveAndIntoxication = false;
        for (int i = 0; i < 4; i++)
        {
            partners[i].__resetValues(BaseStats[i].Composure);
        }
        partners[4].__resetValues();
        playerBase();

    }

    private void __resetToZero()
    {
        ResetToZero = false;
        for (int i = 0; i < partners.Length; i++)
        {
            partners[i].__resetValues();
        }
        Player.TotalPointsScoredAcrossAllDartMatches = 0;
        Player.Intoxication = 0;
        Player.Luck = 0;
        Player.Skill = 0;

    }
}

[System.Serializable]
class __BaseStats
{
    public string name;
    [Header("Chad and Elaine ignore this stat almost entirely")]
    public float Composure = 0f;
    [Header("INTOXICATION SHOULD ONLY BE SET TO NON ZERO FOR TESTING")]
    public float Intoxication = 0f;
    [Header("LOVE SHOULD ONLY BE SET TO NON ZERO FOR TESTING")]
    public float Love = 0f;
}

[System.Serializable]
class __BasePlayerStats
{
    public float Intoxication = 0f;
    public float Skill = 0f;
    public float Luck = 0f;
    public float Points =0f;
}

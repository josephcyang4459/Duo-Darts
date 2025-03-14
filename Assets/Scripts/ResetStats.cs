using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Reset Stats", menuName = "Testing Tools/Stat Changer")]
public class ResetStats : ScriptableObject
{
    [SerializeField] Player Player;
    [SerializeField] BaseStats[] BaseStats = new BaseStats[5];
    [SerializeField] BasePlayerStats BasePlayerStats;
    [SerializeField] EventList Events;
    [SerializeField] EventList PlayerNotification;
    [SerializeField] EventList BadEndings;
    [SerializeField] CharacterList characters;


    public void ResetStatsAndCompletionToBase()
    {
        
        for (int i = 0; i < 5; i++)
        {
            characters.list[i].ResetValues(BaseStats[i].Love, BaseStats[i].Intoxication, BaseStats[i].Composure);
        }
        playerBase();
        resetEvents();

    }

    private void playerBase()
    {
        Player.Intoxication = BasePlayerStats.Intoxication;
        Player.Luck = BasePlayerStats.Luck;
        Player.Skill = BasePlayerStats.Skill;
        Player.TotalPointsScoredAcrossAllDartMatches = BasePlayerStats.Points;
    }

    private void resetEvents()
    {
        foreach (EventStart e in BadEndings.List)
            e.done = false;
        foreach (EventStart e in PlayerNotification.List)
            e.done = false;
        foreach (EventStart e in Events.List)
            e.done = false;
    }

#if UNITY_EDITOR

    public bool ResetToBase;
    public bool ResetToZero;
    public bool ResetToBaseAllButLoveAndIntoxication;

    public void OnValidate()
    {
        for(int i = 0; i < BaseStats.Length; i++)
        {
            BaseStats[i].name = ((CharacterNames)i).ToString();
        }

        if(ResetToBase)
        {
            ResetToBase = false;
            ResetStatsAndCompletionToBase();
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






    private void __resetToExceptComposure()
    {
        ResetToBaseAllButLoveAndIntoxication = false;
        for (int i = 0; i < 4; i++)
        {
            characters.list[i].__resetValues(BaseStats[i].Composure);
        }
        characters.list[4].__resetValues();
        playerBase();

    }

    private void __resetToZero()
    {
        ResetToZero = false;
        for (int i = 0; i < characters.list.Length; i++)
            characters.list[i].__resetValues();

        Player.TotalPointsScoredAcrossAllDartMatches = 0;
        Player.Intoxication = 0;
        Player.Luck = 0;
        Player.Skill = 0;

    }
#endif
}

[System.Serializable]
class BaseStats
{
#if UNITY_EDITOR
    public string name;
#endif
    [Header("Chad and Elaine ignore this stat almost entirely")]
    public float Composure = 0f;
    [Header("INTOXICATION SHOULD ONLY BE SET TO NON ZERO FOR TESTING")]
    public float Intoxication = 0f;
    [Header("LOVE SHOULD ONLY BE SET TO NON ZERO FOR TESTING")]
    public float Love = 0f;
}

[System.Serializable]
class BasePlayerStats
{
    public float Intoxication = 0f;
    public float Skill = 0f;
    public float Luck = 0f;
    public int Points =0;
}

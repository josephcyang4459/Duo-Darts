
#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ResetStats : ScriptableObject
{
    [SerializeField] Partner[] partners;
    [SerializeField] Player Player;
    [SerializeField] __BaseStats[] BaseStats = new __BaseStats[4];
    [SerializeField] __BasePlayerStats BasePlayerStats;

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
            Debug.Log("all Stats have been reset to BASE");
        }

        if (ResetToZero)
        {
            __resetToZero();
            Debug.Log("all Stats have been reset to ZERO");
        }

    }
    
    private void playerBase()
    {
        Player.Charisma = BasePlayerStats.Charisma;
        Player.Intoxication = BasePlayerStats.Intoxication;
        Player.Luck = BasePlayerStats.Luck;
        Player.Skill = BasePlayerStats.Skill;
    }

    private void __resetToBase()
    {
        ResetToBase = false;
        for (int i = 0; i < partners.Length; i++)
        {
            partners[i].__resetValues(BaseStats[i].Love, BaseStats[i].Intoxication, BaseStats[i].Composure);
        }

        playerBase();
       
    }

    private void __resetToExceptComposure()
    {
        ResetToBase = false;
        for (int i = 0; i < partners.Length; i++)
        {
            partners[i].__resetValues(BaseStats[i].Composure);
        }

        playerBase();

    }

    private void __resetToZero()
    {
        ResetToBase = false;
        for (int i = 0; i < partners.Length; i++)
        {
            partners[i].__resetValues();
        }

        Player.Charisma = 0;
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
    public float Charisma = 1f;
    public float Intoxication = 1f;
    public float Skill = 1f;
    public float Luck = 1f;
}
#endif

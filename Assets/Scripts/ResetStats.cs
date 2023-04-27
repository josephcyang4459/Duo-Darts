
#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ResetStats : ScriptableObject
{
    [SerializeField] Partner[] partners;
    [SerializeField] Player Player;

    public bool Reset;
    public void OnValidate()
    {
        if(Reset)
        {
            Reset = false;
            for (int i = 0; i < partners.Length; i++)
            {
                partners[i].__resetValues();
                //if(i==(int)Characters.faye)
            }

            Player.Charisma = 0;
            Player.Intoxication = 0;
            Player.Luck = 0;
            Player.Skill = 0;
            Debug.Log("all Stats have been reset to zero");
        }
        
    }
}
#endif

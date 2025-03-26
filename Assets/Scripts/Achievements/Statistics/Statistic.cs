using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class Statistic : ScriptableObject
{
    [SerializeField] protected string InternalName;

    public string GetInternalName() {
        return InternalName;
    }

    public abstract void ResetStatistic();

    public abstract string GetData();

    public abstract void SetLocalData(string[] s);
}

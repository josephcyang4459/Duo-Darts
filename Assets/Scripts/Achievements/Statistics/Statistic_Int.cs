using UnityEngine;

[CreateAssetMenu]
public class Statistic_Int : Statistic
{
    [SerializeField] int Number;

    public int GetNumber() {
        return Number;
    }

    public void IncreaseNumber() {
        Number++;
    }

    public void IncreaseNumber(int inc) {
        Number += inc;
    }

    public override string GetData() {
        return InternalName + "|" + Number + "\n";
    }

    public override void SetLocalData(string[] s) {
        Number = int.Parse(s[1].Trim());
    }

    public override void ResetStatistic() {
        Number = 0;
    }
}

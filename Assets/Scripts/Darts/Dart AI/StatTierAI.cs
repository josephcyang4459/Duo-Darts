using UnityEngine;

[CreateAssetMenu(fileName = "new Stat Tier AI", menuName = "Reference/Dart AI/Stat Tier AI")]
public class StatTierAI : DartAI {

    [SerializeField] Vector2Int DoubleRange;
    [Header("Use Supplied partner stats or use Current Game Partner")]
    [SerializeField] bool GamePartner;
    [SerializeField] Partner Partner;
    [SerializeField] Stats StatToUse;
    [SerializeField] DartAIStatCutOff[] Tiers;

    public float GetStat(Partner p) {
        switch (StatToUse) {
            case Stats.Composure: return p.Composure;
            case Stats.Intoxication: return p.Intoxication;
            case Stats.Love: return p.Love;
        }
        return 0;
    }

    public override void SelectTarget(int neededToWin, DartGame game) {
        if (neededToWin >= 60) {
            float stat = GetStat(GamePartner ? game.CurrentPartner : Partner);
            for (int i = 0; i < Tiers.Length; i++) {
                if (Tiers[i].CanUse(stat)) {
                    UseTier(DoubleRange.x, DoubleRange.y, Tiers[i].TierData, game);
                    return;
                }

            }
            UseTier(DoubleRange.x, DoubleRange.y, Tiers[^1].TierData, game);
            return;
        }
        BaseOverFifty(neededToWin, game);
    }

#if UNITY_EDITOR
    [SerializeField] bool __rectify;
    private void OnValidate() {
        if (!__rectify)
            return;
        foreach (DartAIStatCutOff tierData in Tiers)
            tierData.TierData.__rectifyTiers();
    }
#endif

}

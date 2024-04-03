using UnityEngine;

public abstract class DartAI : ScriptableObject {
    [SerializeField] protected float BaseOffset;

    public abstract void SelectTarget(int neededToWin, DartGame game);
    protected void BaseOverFifty(int neededToWin, DartGame game) {
        if (neededToWin >= 50)// always goes for bullseye
        {
            game.PartnerTarget(BaseOffset);
            return;
        }

        if (neededToWin > 20)// goes for random small
        {
            int temp = Random.Range(5, 7);//from 5 to 6 so points from  15, 18, 21
            game.PartnerTarget(temp, (int)PointValueTarget.Triple, BaseOffset);
            return;
        }

        if (neededToWin <= 20)//goes for single to win
        {// Whether we Inner or Outer Just to add to the presentation
            int target = Random.Range(0, 10) > 4 ? (int)PointValueTarget.OuterSingle : (int)PointValueTarget.InnerSingle;
            game.PartnerTarget(neededToWin, target, BaseOffset);
            return;
        }
    }

    protected void UseTier(int low, int high, DartAIPercentChance tierData, DartGame game) {
        DartAIAction action = tierData.GetAction();
        switch (action) {
            case DartAIAction.Sixty: game.PartnerTarget(20, (int)PointValueTarget.Triple, BaseOffset); return;
            case DartAIAction.Bullseye: game.PartnerTarget(BaseOffset); return;

        }

        int pick = Random.Range(low, high+1);
        int ringTarget;
        if (action == DartAIAction.Double) ringTarget = (int)PointValueTarget.Double;
        else ringTarget = Random.Range(0, 10) < 5 ? (int)PointValueTarget.InnerSingle : (int)PointValueTarget.OuterSingle;
        game.PartnerTarget(pick, ringTarget, BaseOffset);

        return;
    }

    [System.Serializable]
    protected class DartAIStatCutOff {
        [Header("Can Use if Supplied Stat is Less than StatCutOff")]
        [SerializeField] int StatCutOff;
        public DartAIPercentChance TierData;

        public bool CanUse(float stat) {
            return stat < StatCutOff;
        }
    }

    [System.Serializable]
    protected class DartAIPercentChance {
        [SerializeField] [Range(0, 1)] float Sixty;
        [SerializeField] [Range(0, 1)] float Bullseye;
        [SerializeField] [Range(0, 1)] float Double;
        [SerializeField] [Range(0, 1)] float Single;

        public DartAIAction GetAction() {
            float total = Sixty + Bullseye + Double + Single;
            float temp = Random.Range(0, total);
            if (temp <= Sixty)
                return DartAIAction.Sixty;
            if (temp <= Sixty + Bullseye)
                return DartAIAction.Bullseye;
            if (temp <= Sixty + Bullseye + Double)
                return DartAIAction.Double;
            return DartAIAction.Single;
        }

#if UNITY_EDITOR
        public void __rectifyTiers() {
            float temp = Sixty + Bullseye + Double + Single;
            Sixty /= temp;
            Bullseye /= temp;
            Double /= temp;
            Single /= temp;
        }
#endif
    }
    protected enum DartAIAction {
        Sixty,
        Bullseye,
        Double,
        Single
    }
}




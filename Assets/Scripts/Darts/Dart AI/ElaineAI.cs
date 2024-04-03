using UnityEngine;

[CreateAssetMenu(fileName = "Elaine AI", menuName = "Reference/Dart AI/Elaine AI")]
public class ElaineAI : DartAI
{
    public override void SelectTarget(int neededToWin, DartGame game) {
        if (neededToWin >= 60) {
            game.PartnerTarget(20, (int)PointValueTarget.Triple, BaseOffset);
            return;
        }
        BaseOverFifty(neededToWin, game);
    }
}

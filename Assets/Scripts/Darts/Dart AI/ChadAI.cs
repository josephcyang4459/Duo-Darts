using UnityEngine;

[CreateAssetMenu(fileName = "Chad AI", menuName = "Reference/Dart AI/Chad AI")]
public class ChadAI : DartAI {

    public override void SelectTarget(int neededToWin, DartGame game) {
        BaseOverFifty(neededToWin, game);
    }

 
}

using UnityEngine;

public class BoardCollider : MonoBehaviour
{
    public int point;
    public DartGame gameState;
    public MeshRenderer mr;
    public Transform target;

    public void hit()
    {
        gameState.Dart.go(gameState.aim.t.position, point);
    }
}

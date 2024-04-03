using UnityEngine;

public class BoardCollider : MonoBehaviour
{
    public int point;
    public DartGame gameState;
    public MeshRenderer mr;
    public Transform target;

    public void hit(Vector3 position)
    {
        gameState.Dart.ShootDart(position, point);
    }
}

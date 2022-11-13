
using System.Collections.Generic;
using UnityEngine;

public class BoardCollider : MonoBehaviour
{
    public byte point;
    public DartGame gameState;
    public MeshRenderer mr;
    public Transform target;

    public void hit()
    {
        gameState.Dart.go[gameState.numberOfDartsThrow].(gameState.aim.t.localPosition, point);
        //StartCoroutine(wait());
    }
}

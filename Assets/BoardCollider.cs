using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardCollider : MonoBehaviour
{
    public byte point;
    public DartGame g;

    public void hit()
    {
        gameObject.SetActive(false);
        g.gainPoints(point);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardCollider : MonoBehaviour
{
    public byte point;
    public DartGame g;
#if UNITY_EDITOR
    public void Awake()
    {
        mr = gameObject.GetComponent<MeshRenderer>();
    }
#endif
    public MeshRenderer mr;
    public void hit()
    {
        
        g.gainPoints(point);
        StartCoroutine(wait());
    }

    public IEnumerator wait()
    {
        mr.material = g.flash;
        yield return new WaitForSeconds(2);
        mr.material = g.gone;
    }
}

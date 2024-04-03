using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class AimCone : MonoBehaviour
{
    public SpriteRenderer cone;
    public Transform t;


 
    //[HideInInspector]
    public Vector3 normal = new Vector3(0,0,-.2f);
    public float accuracy;
    //[HideInInspector]
    public Vector2 directionMove;

    public WaitForSeconds wait;
    public Coroutine swap;


 
    public Vector2 LowerBounds;
    public Vector2 UpperBounds;

    public void begin()
    {
        normal.x = 0;
        normal.y = 0;
        normal.z = -.2f;
        t.localPosition = normal;
        cone.enabled = true;
        enabled = true;
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        //Debug.Log("Out\a");
        if (!cone.enabled)
            return;
        StopCoroutine(swap);
    }
}

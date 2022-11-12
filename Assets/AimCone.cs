using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class AimCone : MonoBehaviour
{
    public SpriteRenderer cone;
    public Transform t;
    public Vector2 center;
    public float driftSpeed;
    public float moveSpeed;
    //[HideInInspector]
    public Vector2 normal;
    //[HideInInspector]
    public Vector2 directionMove;
    public Vector2 cache = Vector2.zero;
    public WaitForSeconds wait;
    public Coroutine swap;
    public Rigidbody2D r;

    public InputActionReference move;
    public InputActionReference fire;

    public void Start()
    {
        begin();
    }

    public void begin()
    {
        wait = new WaitForSeconds(3);
        swap = StartCoroutine(direction());
        cone.enabled = true;
        normal.x = t.localPosition.x;
        normal.x = t.localPosition.y;

        move.action.Enable();
        move.action.performed += onMove;
        move.action.canceled += off;
        fire.action.Enable();
        fire.action.performed += shoot;

    }


    public void OnDisable()
    {
        StopCoroutine(swap);
        cone.enabled = false;
        move.action.Disable();
        move.action.performed -= onMove;
        move.action.canceled -= off;

        fire.action.Disable();
        fire.action.performed -= shoot;
    }

    public void Update()
    {
        normal.y += (directionMove.y + cache.y ) * Time.deltaTime ;
        normal.x += (directionMove.x + cache.x) * Time.deltaTime;
        t.localPosition = normal;
        
        //normal.x = t.localPosition.x;
        //normal.y = t.localPosition.y;
        
    }

    public void shoot(InputAction.CallbackContext c)
    {
        


        enabled = false;
    }

    public IEnumerator direction()
    {
        while (true)
        {
            directionMove.x = Random.Range(-driftSpeed, driftSpeed);
            directionMove.y = Random.Range(-driftSpeed, driftSpeed);
            yield return wait;

        }
    }

    public IEnumerator Recover()
    {
        //cache = Vector2.MoveTowards(normal, center, 1);
        //Debug.Log("center " + center);
        //Debug.Log("ca " + normal);
        //Debug.Log("dir " + directionMove);

        directionMove.x = center.x - normal.x;
        directionMove.y = center.y - normal.y;

        directionMove.x = Mathf.Clamp(directionMove.x,-moveSpeed, moveSpeed);
        directionMove.y = Mathf.Clamp(directionMove.y, -moveSpeed, moveSpeed);

        //Debug.Log("dir " + directionMove);
        //throw new System.AccessViolationException();
        yield return wait;
        swap = StartCoroutine(direction());
    }

    public void off(InputAction.CallbackContext c)
    {
        cache = Vector2.zero;
    }

    public void onMove(InputAction.CallbackContext c)
    {
      
        cache = c.ReadValue<Vector2>();
        cache.x  *= moveSpeed;
        cache.y *= moveSpeed;
    }


    public void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Out\a");
        StopCoroutine(swap);
      
        StartCoroutine(Recover());
    }
}

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
    public Vector3 normal = new Vector3(0,0,-.2f);
    public float accuracy;
    //[HideInInspector]
    public Vector2 directionMove;
    public Vector2 cache = Vector2.zero;
    public WaitForSeconds wait;
    public Coroutine swap;
    public LayerMask layer;

    public InputActionReference move;
    public InputActionReference fire;
    public Vector2 LowerBounds;
    public Vector2 UpperBounds;

    public void begin()
    {
        wait = new WaitForSeconds(3);
        swap = StartCoroutine(direction());
       
        normal.x = 0;
        normal.y = 0;
        normal.z = -.2f;
        t.localPosition = normal;
        cone.enabled = true;
        enabled = true;

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
        normal.y = Mathf.Clamp(normal.y, LowerBounds.y, UpperBounds.y);
        normal.x = Mathf.Clamp(normal.x, LowerBounds.x, UpperBounds.x);
        t.position = normal;

    }

    public void shoot(InputAction.CallbackContext c)
    {
        ghoot(t.position);
    }

    public void ghoot(Vector3 h)
    {
        h.y += Random.Range(-accuracy, accuracy);
        h.x += Random.Range(-accuracy, accuracy);
        if (Physics.Raycast(h, Vector3.forward, out RaycastHit hit, layer))
        {
            normal.y = h.y;
            normal.x = h.x;
            t.position = normal;
            
            hit.collider.gameObject.GetComponent<BoardCollider>().hit();
        }
       
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
        //Debug.Log("Out\a");
        if (!cone.enabled)
            return;
        StopCoroutine(swap);
      
        StartCoroutine(Recover());
    }
}

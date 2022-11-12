using UnityEngine;
using System.Collections;

public class dartBoard : MonoBehaviour
{
    public Sprite cone;
    public Transform t;
    public float speed;
    public Vector2 normal;
    public Vector2 directionMove;
    public Vector2 bounds;
    public WaitForSeconds wait;
    public Coroutine swap;
    public void Start()
    {
        begin();
    }
    public void begin()
    {
        wait = new WaitForSeconds(3);
        swap = StartCoroutine(direction());
        normal.x = t.localPosition.x;
        normal.x = t.localPosition.y;
    }

    public void Update()
    {
        normal.y = directionMove.y * Time.deltaTime;
        normal.x = directionMove.x * Time.deltaTime;
         t.Translate(normal);
        normal.x = t.localPosition.x;
        normal.y = t.localPosition.y;
        
    }

    public IEnumerator direction()
    {
        while (true)
        {
            directionMove.x = Random.Range(-speed, speed);
            directionMove.y = Random.Range(-speed, speed);
            yield return wait;

        }
    }
}

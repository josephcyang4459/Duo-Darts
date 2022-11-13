using UnityEngine;
using System.Collections;

public class DartScript : MonoBehaviour
{
    public Vector3 destination;
    public Transform thi;
    public float maxSpeed;
    public float speed;
    public float rateOfDecrease;
    public AnimationCurve curve;
    public byte points;
    public DartGame d;
    public MeshRenderer m;

    public void go(Vector3 v, byte b)
    {
        points = b;
        thi.position = new Vector3(v.x, v.y, -15);
        destination.x = v.x;
        destination.y = v.y;
        speed = maxSpeed;
        enabled = true;
    }

    public void vvvv()
    {
        thi.position = new Vector3(0, 0, -15);
        speed = maxSpeed;
    }

    public void Update()
    {
        thi.position = Vector3.MoveTowards(thi.position, destination, speed*Time.deltaTime);
        speed = maxSpeed * curve.Evaluate(thi.position.z);
        if (thi.position == destination)
        {
            enabled = false;
            Debug.Log("eee");
            StartCoroutine(wiat());
        }
    }

    public IEnumerator wiat()
    {
        d.AddPoints(points);
        yield return d.k;
        d.check(points);
    }
}

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

    public void go(Vector3 v, byte b)
    {
        points = b;
        thi.localPosition = new Vector3(v.x, v.y, -11);
        destination.x = v.x;
        destination.y = v.y;
        speed = maxSpeed;
        this.enabled = true;
    }

    public void vvvv()
    {
        thi.localPosition = new Vector3(0, 0, -11);
        speed = maxSpeed;
    }

    public void Update()
    {
        thi.localPosition = Vector3.MoveTowards(thi.localPosition, destination, speed*Time.deltaTime);
        speed = maxSpeed * curve.Evaluate(thi.localPosition.z);
        if (thi.localPosition == destination)
        {
            enabled = false;
            StartCoroutine(wiat());
        }
    }

    public IEnumerator wiat()
    {
        d.AddPoints(points);
        yield return d.k;
        d.check(points);
        vvvv();
    }
}

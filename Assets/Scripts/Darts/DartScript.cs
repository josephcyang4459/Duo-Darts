using UnityEngine;
using System.Collections;

public class DartScript : MonoBehaviour
{
    public Vector3 destination;
    public Transform thi;
    public float maxSpeed;
    public float speed;
    public AnimationCurve curve;
    public byte points;
    public DartGame d;
    public MeshRenderer m;

    public void go(Vector3 v, byte b)
    {
        if (m.material.name == "redDart")
            Debug.Log("ee4444e");
        points = b;
        thi.position = new Vector3(v.x, v.y, -18);
        destination.x = v.x;
        destination.y = v.y;
        speed = maxSpeed;
        enabled = true;
        if (m.name == "redDart")
            Debug.Log("ee22222e");
    }

    public void vvvv()
    {
        thi.position = new Vector3(0, 0, -18);
        speed = maxSpeed;
    }

    public void Update()
    {
        if (m.material.name == "redDart")
            Debug.Log("eee");
        thi.position = Vector3.MoveTowards(thi.position, destination, speed*Time.deltaTime);
        speed = maxSpeed * curve.Evaluate(thi.position.z);
        if (thi.position == destination)
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
    }
}

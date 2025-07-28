using UnityEngine;
using System.Collections.Generic;

public class DartScript : MonoBehaviour
{
    public Vector3 ResetPosition = new Vector3(0, 0, -18);
    public Vector3 destination;
    public Transform CurrentDartTransform;
    public Transform[] PlayerDarts;
    public Transform[] ChadDarts;
    public Transform[] JessDarts;
    public Transform[] FayeDarts;
    public Transform[] ElaineDarts;

    public int PartnerIndex;
    public float maxSpeed;
    public float speed;
    public AnimationCurve curve;
    public int points;
    public DartGame DartsGame;
    [SerializeField] Timer WaitTimer;
    [SerializeField] float WaitInBetweenDarts;
    [SerializeField] List<Transform> CurrentDarts;
    [SerializeField] float TwistDistance;
    [SerializeField] float RotationOffset;

    public void SetUp(int partnerIndex)
    {
        PartnerIndex = partnerIndex;
        ResetDartPositions();
    }

    public void SetCurrentDart(int dartIndex, bool playerTurn)
    {
        if (playerTurn)
        {
            CurrentDartTransform = PlayerDarts[dartIndex];
            return;
        }

        switch (PartnerIndex)
        {
            case 0:CurrentDartTransform = ChadDarts[dartIndex];return;
            case 1: CurrentDartTransform = JessDarts[dartIndex]; return;
            case 2: CurrentDartTransform = FayeDarts[dartIndex]; return;
            case 3: CurrentDartTransform = ElaineDarts[dartIndex]; return;
            default: CurrentDartTransform = PlayerDarts[dartIndex]; return;
        }
    }

    public void ShootDart(Vector3 v, int point_value)
    {
        points = point_value;
        CurrentDartTransform.position = new Vector3(v.x, v.y, -18);
        
        destination.x = v.x;
        destination.y = v.y;
        speed = maxSpeed;
        //CurrentDartTransform.rotation = Quaternion.Euler(-90, 0, Random.Range(0, 358));
      
        enabled = true;
    }

    float getDirection(float f) {
        if (f > 0)
            return 1;
        if (f < 0)
            return -1;
        return (Random.Range(0, 100) > 47) ? 1 : -1;

    }

    void CheckForDestinationDarts() {
        foreach (Transform t in CurrentDarts) {
            if (Vector3.Distance(t.position, destination) <= TwistDistance) {
                float directionx = getDirection(t.position.x - destination.x);
                float directiony = getDirection(t.position.y - destination.y);

                float xDistance = Mathf.Clamp(t.position.x - destination.x + directionx, -1.5f, 1.5f) * RotationOffset * Random.Range(.8f,.9f);
                float yDistance = Mathf.Clamp(t.position.y - destination.y + directiony , -1.5f, 1.5f) * RotationOffset * Random.Range(.8f, .9f);
                Vector3 specialRotation = new(-90 + yDistance, xDistance, 0);

                CurrentDartTransform.rotation = Quaternion.Euler(specialRotation);
            }
        }
    }

    public void ResetDartPositions()
    {
        Transform[] getPartnerArray()
        {
            switch (PartnerIndex)
            {
                case 0: return ChadDarts;
                case 1:return JessDarts;
                case 2:return FayeDarts;
                case 3: return ElaineDarts;
            }
            return PlayerDarts;
        }
        foreach (Transform t in PlayerDarts)
        {
            t.position = ResetPosition;
        }
        foreach (Transform t in getPartnerArray())
        {
            t.position = ResetPosition;
        }

        CurrentDarts.Clear();
        speed = maxSpeed;
    }

    public void Update()
    {
        CurrentDartTransform.position = Vector3.MoveTowards(CurrentDartTransform.position, destination, speed*Time.deltaTime);
        speed = maxSpeed * curve.Evaluate(CurrentDartTransform.position.z);
        if (CurrentDartTransform.position == destination)
        {
            CheckForDestinationDarts();
            CurrentDarts.Add(CurrentDartTransform);
            enabled = false;
            DartsGame.AddPoints(points);
            WaitTimer.BeginTimer(WaitInBetweenDarts);
        }
    }

    
#if UNITY_EDITOR
    [SerializeField] Transform __player;

    [SerializeField] Transform __chad;
    [SerializeField] Transform __jess;
    [SerializeField] Transform __faye;
    [SerializeField] Transform __elaine;

    [SerializeField] bool __reset;

    private void OnValidate()
    {
        if (__reset)
        {
            __reset = false;
            PlayerDarts = new Transform[3];
            ChadDarts = new Transform[3];
            JessDarts = new Transform[3];
            FayeDarts = new Transform[3];
            ElaineDarts = new Transform[3];

            for (int i = 0; i < 3; i++)
                PlayerDarts[i] = __player.GetChild(i);
            for (int i = 0; i < 3; i++)
                ChadDarts[i] = __chad.GetChild(i);
            for (int i = 0; i < 3; i++)
                JessDarts[i] = __jess.GetChild(i);
            for (int i = 0; i < 3; i++)
                FayeDarts[i] = __faye.GetChild(i);
            for (int i = 0; i < 3; i++)
                ElaineDarts[i] = __elaine.GetChild(i);
        }
    }
#endif
}

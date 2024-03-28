using UnityEngine;
using System.Collections;

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
    public WaitForSeconds WaitTime = new WaitForSeconds(3);

    public void SetUp(int partnerIndex)
    {
        PartnerIndex = partnerIndex;
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

    public void go(Vector3 v, int point_value)
    {
        points = point_value;
        CurrentDartTransform.position = new Vector3(v.x, v.y, -18);
        destination.x = v.x;
        destination.y = v.y;
        speed = maxSpeed;
        enabled = true;
    }

    public void reset_position()
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


        speed = maxSpeed;
    }

    public void Update()
    {
        CurrentDartTransform.position = Vector3.MoveTowards(CurrentDartTransform.position, destination, speed*Time.deltaTime);
        speed = maxSpeed * curve.Evaluate(CurrentDartTransform.position.z);
        if (CurrentDartTransform.position == destination)
        {
            enabled = false;
            StartCoroutine(wiat());
        }
    }

    public IEnumerator wiat()
    {
        DartsGame.AddPoints(points);
        yield return WaitTime;
        DartsGame.CheckForBust();
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

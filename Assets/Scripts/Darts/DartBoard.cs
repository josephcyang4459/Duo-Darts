using UnityEngine;

public class DartBoard : MonoBehaviour
{
    [SerializeField] DartBoardSlice[] PointValues;
    [SerializeField] DartBoardRing[] RingDistances;
    [SerializeField] Transform BoardCenter;
    [SerializeField] Vector2 CenterReference;
    [SerializeField] Vector2 CacheReference;
    
    public int GetScoreFromLocation(float x, float y) {
        CacheReference.x = x;
        CacheReference.y = y;
        CenterReference.x = BoardCenter.position.x;
        CenterReference.y = BoardCenter.position.y;
        float distance = Vector2.Distance(CacheReference, CenterReference);
        int multiplier = RingMultiplier(distance);
        if (multiplier < 0)
            return 50;
        if (multiplier == 0)
            return 0;
        int points = GetPoint(CacheReference.x- CenterReference.x, CacheReference.y - CenterReference.y);
        return points * multiplier;
    }

    int RingMultiplier(float distance) {
        foreach(DartBoardRing Ring in RingDistances) {
            if (distance < Ring.MaxDistance)
                return Ring.Multiplier;
        }
        return 0;
    }

    int GetPoint(float x, float y) {
        float targetAngle = Mathf.Atan2(x, y) * Mathf.Rad2Deg;
       
        if (targetAngle < 0)
            targetAngle = 360 + targetAngle;

        foreach (DartBoardSlice Slice in PointValues) {
            if (Slice.IsInbetween(targetAngle))
                return Slice.BasePointValue;
        }
        return 0;
    }

    [System.Serializable]
    class DartBoardSlice {
        public float MinAngle;
        public float MaxAngle;
        public int BasePointValue;

        public bool IsInbetween(float f) {
            if (MinAngle < MaxAngle)
                return (f >= MinAngle && f < MaxAngle);

            return (f >= MinAngle || f < MaxAngle);
        }
    }

    [System.Serializable]
    class DartBoardRing {
        public float MaxDistance;
        public int Multiplier;
    }
#if UNITY_EDITOR
    [Space]
    [Header("----EDITOR ONLY----")]
    readonly int[] __point = { 20, 1, 18, 4, 13, 6, 10, 15, 2, 17, 3, 19, 7, 16, 8, 11, 14, 3, 12, 5 };
    readonly int[] __MultiplierValues = { -1, 1, 3, 1, 2 };
    [SerializeField] Transform[] __RingReference;
    [SerializeField] bool __resetBoard;
    [SerializeField] Transform __Position;
    [SerializeField] bool __Test;

    private void OnValidate() {
        if (__resetBoard) {
            __resetBoard = false;
            __resetSliceAndRing();
        }
        if (__Test) {
            __Test = false;
            GetScoreFromLocation(__Position.position.x, __Position.position.y);
        }
    }

    void __resetSliceAndRing() {
        CenterReference.x = BoardCenter.position.x;
        CenterReference.y = BoardCenter.position.y;
        RingDistances = new DartBoardRing[__RingReference.Length];
        for (int i = 0; i < __RingReference.Length; i++) {
            DartBoardRing temp = new();
            CacheReference.x = __RingReference[i].position.x;
            CacheReference.y = __RingReference[i].position.y;
            temp.MaxDistance = Vector2.Distance(CacheReference, CenterReference);
            //PointValues
            temp.Multiplier = __MultiplierValues[i];
            RingDistances[i] = temp;
        }
        PointValues = new DartBoardSlice[20];
        float __referenceAngle = 360 / 20;
        for (int i = 0; i < 20; i++) {
            DartBoardSlice temp = new();
            temp.BasePointValue = __point[i];
            float baseAngle = i * __referenceAngle;
            temp.MinAngle = baseAngle - __referenceAngle / 2;
            if (temp.MinAngle < 0)
                temp.MinAngle = 360 + temp.MinAngle;
            temp.MaxAngle = baseAngle + __referenceAngle / 2;
            //PointValues
            PointValues[i] = temp;
        }
    }
#endif
}

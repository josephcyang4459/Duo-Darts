using UnityEngine;

public class DartBoardAI : MonoBehaviour
{
    [SerializeField] BoardSlice[] Slices;
    [SerializeField] Transform BullsEye;
    public Vector3 GetTarget(int basePointValue, int ring) {
        basePointValue -= 1;
        return Slices[basePointValue].EndLocations[ring].position;
    }

    public Vector3 GetTarget() {
        return BullsEye.position;
    }

   [System.Serializable]
   class BoardSlice {
        public Transform[] EndLocations;
    }

#if UNITY_EDITOR
    [SerializeField] Transform HolderHolders;
    [SerializeField] bool __reset;

    private void OnValidate() {
        if (__reset) {
            __reset = false;

            Slices = new BoardSlice[20];

            for (int holderChildIndex = 0; holderChildIndex < HolderHolders.childCount; holderChildIndex++) {
                
                int temp = int.Parse(HolderHolders.GetChild(holderChildIndex).name)-1;
                Debug.Log(temp);
                Slices[temp] = new BoardSlice();
                Transform[] loc = new Transform[4];
                for (int ringIndex = 0; ringIndex < 4; ringIndex++)
                    loc[ringIndex] = HolderHolders.GetChild(holderChildIndex).GetChild(ringIndex);
                Slices[temp].EndLocations = loc;
            }
        }
       
    }

#endif
}

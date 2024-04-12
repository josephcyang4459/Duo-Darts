using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour {
    [SerializeField] Image[] ButtonImages;
    [SerializeField] Transform[] DartLocations;
    [SerializeField] ImageFill Fill;

    public void SelectButton(int i) { Fill.SetCurrentImageToFill(ButtonImages[i], DartLocations[i].position); }

    public void ClearFill() { Fill.ClearImages(); }

#if UNITY_EDITOR
    [SerializeField] Transform __ButtonHolder;
    [SerializeField] Transform[] __TargetLocations;
    [SerializeField] Vector3 __Offset;
    [SerializeField] bool __set;

    private void OnValidate() {
        if (__set) {
            __set = false;
            //ButtonImages = __ButtonHolder.GetComponentsInChildren<Image>();
            for (int i = 0; i < ButtonImages.Length; i++)
                DartLocations[i] = __TargetLocations[i];
        }
    }
#endif
}

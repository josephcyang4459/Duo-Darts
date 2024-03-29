using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    [SerializeField] Image[] ButtonImages;
    [SerializeField] Vector3[] DartLocations;
    
    [SerializeField] ImageFill Fill;

    public void SelectButton(int i)
    {
        Fill.SetCurrentImageToFill(ButtonImages[i], DartLocations[i]);
    }
#if UNITY_EDITOR
    [SerializeField] Transform __ButtonHolder;
    [SerializeField] Transform[] __TargetLocations;
    [SerializeField] Vector3 __Offset;
    [SerializeField] bool __set;
    private void OnValidate()
    {
        if (__set)
        {
            __set = false;
            //ButtonImages = __ButtonHolder.GetComponentsInChildren<Image>();
            DartLocations = new Vector3[ButtonImages.Length];
            for (int i = 0; i < ButtonImages.Length; i++)
                DartLocations[i] = __TargetLocations[i].position+__Offset;
        }
    }

#endif
}

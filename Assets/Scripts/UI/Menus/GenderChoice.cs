using UnityEngine;
using UnityEngine.UI;

public class GenderChoice : MonoBehaviour, Caller
{
    [SerializeField] Image FemMask;
    [SerializeField] Image MascMask;
    [SerializeField] Transform FemTarget;
    [SerializeField] Transform MascTarget;
    [SerializeField] ImageFill Fill;
    [SerializeField] GameObject FirstSelected;

    public void Start() {
        UIState.inst.SetAsSelectedButton(FirstSelected);
    }

    public void SelectButton(int i) {
        if (i == 0) {
            Fill.SetCurrentImageToFill(FemMask, FemTarget.position);
        }
        else {
            Fill.SetCurrentImageToFill(MascMask, MascTarget.position);
        }
    }

    public void Ping() {
        
    }
}

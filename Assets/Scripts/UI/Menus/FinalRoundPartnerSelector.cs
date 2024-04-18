using UnityEngine;
using UnityEngine.UI;

public class FinalRoundPartnerSelector : MonoBehaviour
{
    [SerializeField] CharacterList Partners;
    [SerializeField] Schedule Schedule;
    [SerializeField] DartPartnerStoryUI PartnerSelector;
    [SerializeField] Canvas SelectionCanvas;
    [SerializeField] GameObject FirstSelected;
    [SerializeField] Button[] Buttons;
    [SerializeField] Image[] PartnerImageMasks;
    [SerializeField] Transform[] DartTargets;
    [SerializeField] ImageFill Fill;
    public void ShowUI() {
        SelectionCanvas.enabled = true;
        UIState.inst.SetAsSelectedButton(FirstSelected);
        for (int i = 0; i < (int)CharacterNames.Owner; i++) {
            Buttons[i].interactable = Partners.list[i].FinalRoundEligable();
        }

    }

    public void SelectButton(int i) {
        if (!Buttons[i].interactable)
            return;
        Fill.SetCurrentImageToFill(PartnerImageMasks[i], DartTargets[i].position);
    }

    public void HideUI() {
        SelectionCanvas.enabled = false;
    }

    public void ChoosePartner(int characterIndex) {
        PartnerSelector.ForceDartsException(characterIndex, Schedule.hour);
        DartSticker.inst.SetVisible(false);
    }

#if UNITY_EDITOR
    [SerializeField] ColorSwatch __keyColors;

    [SerializeField] bool __resetImageMaskColors;

    private void OnValidate() {
        if (__resetImageMaskColors) {
            __resetImageMaskColors = false;

            for(int i = 0; i < PartnerImageMasks.Length; i++) {
                Image[] temp = PartnerImageMasks[i].GetComponentsInChildren<Image>();
                foreach (Image imm in temp)
                    imm.color = __keyColors.colors[i];
            }
           
        }
    }

#endif

}

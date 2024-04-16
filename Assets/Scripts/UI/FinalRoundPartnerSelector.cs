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
   
    public void ShowUI() {
        SelectionCanvas.enabled = true;
        UIState.inst.SetAsSelectedButton(FirstSelected);
        for (int i = 0; i < (int)CharacterNames.Owner; i++) {
            Buttons[i].interactable = Partners.list[i].FinalRoundEligable();
        }

    }

    public void HideUI() {
        SelectionCanvas.enabled = false;
    }

    public void SelectPartner(int characterIndex) {
        PartnerSelector.ForceDartsException(characterIndex, Schedule.hour);
    }
}

using UnityEngine;
using UnityEngine.UI;

public class DartMenu_StandAlone_Options : MonoBehaviour {
    [SerializeField] Image[] GameTypeBanner;
    [SerializeField] ImageFill Fill;
    public bool TipsyPlayer;
    public bool TipsyPartner;
    [SerializeField] UIToggle PlayerToggle;
    [SerializeField] UIToggle PartnerToggle;


    public void SelectGameTypeButton(int i) {
        Fill.SetCurrentImageToFill(GameTypeBanner[i]);
    }

    public void SetPlayerTipsy() {
        TipsyPlayer = PlayerToggle.GetState();
    }

    public void SetPartnersTipsy() {
        TipsyPartner = PartnerToggle.GetState();
    }
}

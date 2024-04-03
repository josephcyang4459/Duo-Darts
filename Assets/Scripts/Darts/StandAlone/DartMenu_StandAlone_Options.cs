using UnityEngine;
using UnityEngine.UI;

public class DartMenu_StandAlone_Options : MonoBehaviour {
    public bool TipsyPlayer;
    public bool TipsyPartner;
    [SerializeField] UIToggle PlayerToggle;
    [SerializeField] UIToggle PartnerToggle;

    public void SetPlayerTipsy() {
        TipsyPlayer = PlayerToggle.GetState();
    }

    public void SetPartnersTipsy() {
        TipsyPartner = PartnerToggle.GetState();
    }
}

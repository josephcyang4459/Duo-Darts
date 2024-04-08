using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DartMen : MonoBehaviour {
    public Canvas menue;
    public DartGame dg;
    public Schedule s;
    public Partner[] p;
    public Button[] characters;
    public TMP_Text[] texts;
    public int[] indices;

    public GameObject FirstSelectButton;

    public void ShowPartnerSelectMenu() {
     

        UIState.inst.SetAsSelectedButton(FirstSelectButton);
    }
 
    public void selctPartner(int i) {
        menue.enabled = false;
        s.off();
        dg.partnerIndex = (indices[i]);
        dg.BeginGame();
    }

    /// <summary>
    /// used to begin a game with the supplied character
    /// </summary>
    /// <param name="characterIndex"></param>
    /// <param name="currentHour"></param>
    public void exception(int characterIndex, int currentHour)
    {
        s.off();
        //s.LocationCanvas.enabled = false;
        //s.EventListCanvas.enabled = false;
        dg.ScoreNeededToWin = currentHour < 7 ? 3 : 701;
        dg.partnerIndex = characterIndex;
        dg.BeginGame();
    }

    public void back() {
        menue.enabled = false;
        s.setTime(0);
    }
}

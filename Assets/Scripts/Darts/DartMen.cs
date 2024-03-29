using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DartMen : MonoBehaviour
{
    public Canvas menue;
    public DartGame dg;
    public Schedule s;
    public Partner[] p;
    public Button[] characters;
    public TMP_Text[] texts;
    public int[] indices;

    public GameObject FirstSelectButton;

    public void ShowPartnerSelectMenu()
    {
        dg.ScoreNeededToWin = s.hour < 7 ? 501 : 701;//gets correct score

        s.off();
        menue.enabled = true;
        int UIcharacterSlotUsed = 0;
       

        //turns on all slots
        for (int i = 0; i < p.Length; i++)
        {
            if (s.hour >= 8 && s.minutes >= 30)
            {
                if (p[i].RelatedCutScenes[(int)PartnerCutscenes.FinalScene].completed)
                {
                    characters[UIcharacterSlotUsed].gameObject.SetActive(true);
                    texts[UIcharacterSlotUsed].text = p[i].Name;
                    indices[UIcharacterSlotUsed] = i;//used to align the internal character list with the UI representation
                    UIcharacterSlotUsed++;
                }
            }
            else//normal 
            {
                if (p[i].RelatedCutScenes[0].completed)
                {
                    characters[UIcharacterSlotUsed].gameObject.SetActive(true);
                    texts[UIcharacterSlotUsed].text = p[i].Name;
                    indices[UIcharacterSlotUsed] = i;//used to align the internal character list with the UI representation
                    UIcharacterSlotUsed++;
                }
            }
            
        }

        //sets all other slots off
        for(int i=UIcharacterSlotUsed;i<p.Length;i++)
                    characters[i].gameObject.SetActive(false);

        UIState.inst.SetAsSelectedButton(FirstSelectButton);
    }

 
    public void selctPartner(int i)
    {
        menue.enabled = false;
        s.off();
        UIState.inst.SetInteractableUIState(false);
        dg.BeginGame(indices[i]);

    }

    /// <summary>
    /// used to begin a game with the supplied character
    /// </summary>
    /// <param name="characterIndex"></param>
    /// <param name="currentHour"></param>
    public void exception(int characterIndex, int currentHour)
    {
        dg.ScoreNeededToWin = currentHour < 7 ? 501 : 701;
        dg.BeginGame(characterIndex);
    }

    public void back()
    {
        menue.enabled = false;
        s.setTime(0);
    }
}

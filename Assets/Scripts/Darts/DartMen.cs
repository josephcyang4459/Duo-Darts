using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DartMen : MonoBehaviour
{
    [SerializeField] int EndGameCutoff;
    [SerializeField] int MaxLoveNeeded;
    public Canvas menue;
    public DartGame dg;
    public Schedule s;
    public Partner[] p;
    public Button[] characters;
    public TMP_Text[] texts;
    public int[] indices;

    public GameObject backButtonm;

    public void begin()
    {
        dg.overall = s.hour < 7 ? 501 : 701;//gets correct score
        int cutoff = s.hour < EndGameCutoff ? 1 : 15;//gets correct love cutoff

        s.off();
        menue.enabled = true;
        int UIcharacterSlotUsed = 0;
       

        //turns on all slots
        for (int i = 0; i < p.Length; i++)
        {
            if (p[i].Love >= cutoff)
            {
                characters[UIcharacterSlotUsed].gameObject.SetActive(true);
                texts[UIcharacterSlotUsed].text = p[i].Name;
                indices[UIcharacterSlotUsed] = i;//used to align the internal character list with the UI representation
                UIcharacterSlotUsed++;
            }
            
        }

        //sets all other slots off
        for(int i=UIcharacterSlotUsed;i<p.Length;i++)
                    characters[i].gameObject.SetActive(false);

        UI_Helper.SetSelectedUIElement(backButtonm);
    }

 
    public void selctPartner(int i)
    {
        menue.enabled = false;
        s.off();
        dg.BeginGame(indices[i]);

    }

    /// <summary>
    /// used to begin a game with the supplied character
    /// </summary>
    /// <param name="characterIndex"></param>
    /// <param name="currentHour"></param>
    public void exception(int characterIndex, int currentHour)
    {
        dg.overall = currentHour < 7 ? 501 : 701;
        dg.BeginGame(characterIndex);
    }

    public void back()
    {
        menue.enabled = false;
        s.setTime(0);
    }
}

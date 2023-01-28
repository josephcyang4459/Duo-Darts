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

    public GameObject backButtonm;

    public void begin()
    {
        dg.overall = s.hour < 7 ? 501 : 701;
        s.off();
        menue.enabled = true;
        int j = 0;
        for (int i = 0; i < p.Length; i++)
        {
            if (p[i].Love >= 1)
            {
                characters[j].gameObject.SetActive(true);
                texts[j].text = p[i].Name;
                indices[j] = i;
                j++;
            }
            
        }

        for(int i=j;i<p.Length;i++)
                    characters[i].gameObject.SetActive(false);

        UI_Helper.SetSelectedUIElement(backButtonm);
    }

 
    public void selctPartner(int i)
    {
        menue.enabled = false;
        s.off();
        dg.BeginGame(indices[i]);

    }


    public void exception(int i, int hour)
    {
        dg.overall = hour < 7 ? 501 : 701;
        dg.BeginGame(i);
    }
    public void back()
    {
        menue.enabled = false;
        s.setTime(0);
    }
}

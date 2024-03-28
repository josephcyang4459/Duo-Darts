using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DartMenu_StandAlone : MonoBehaviour
{


    [SerializeField] CharacterList Characters;
    [SerializeField] DartGame DartGame;
    [SerializeField] FillImage Fill;
    [SerializeField] Canvas PartnerCanvas;
    [SerializeField] Image[] PartnerButtonImages;
    [SerializeField] Image Portrait;
    [SerializeField] Image[] Borders;

    public void StartGameWithPartner(int i)
    {
        PartnerCanvas.enabled = false;
        DartGame.BeginGame(i);
        
    }

    public void ShowCharacterPortrait(int i)
    {
        Fill.SetCurrentImageToFill(PartnerButtonImages[i] ,Vector2.zero);
        Portrait.sprite = Characters.list[i].Expressions[0];
        foreach(Image image in Borders)
        {
            image.fillAmount = (float)i / (float)(Characters.list.Length - 2);
        }
    }

#if UNITY_EDITOR
    [SerializeField] Transform __buttonHolder;
    [SerializeField] bool __reset;

    private void OnValidate()
    {
        if (__reset)
        {
            __reset = false;
            TMPro.TMP_Text[] temp = __buttonHolder.GetComponentsInChildren<TMPro.TMP_Text>();
            PartnerButtonImages = new Image[temp.Length];
            for(int i = 0; i < temp.Length; i++)
            {
                temp[i].text = Characters.list[i].Name;
                PartnerButtonImages[i] = temp[i].gameObject.transform.parent.GetComponent<Image>();
            }
        }
    }

#endif

}

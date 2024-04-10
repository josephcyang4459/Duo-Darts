using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialougeBox : MonoBehaviour
{
    [SerializeField] CharacterList Characters;
    [SerializeField] Image[] ForegroundImages;
    [SerializeField] Image[] BackgroundImages;
    [SerializeField] Canvas[] ExpressionCanvases;
    [SerializeField] Canvas ThoughtCanvas;
    [SerializeField] Canvas DialougeCanvas;
    [SerializeField] Image Line;
    [SerializeField] int CurrentExpression = -1;
    [SerializeField] TypeWriterEffect TypeWriter;
    [SerializeField] TMP_Text ThoughtText;
    [SerializeField] TMP_Text DialougeText;

    public void SetThought(string dialouge) {
        HideDialougeBox();
        ThoughtCanvas.enabled = true;
        TypeWriter.Run(dialouge, ThoughtText);   
    }

    public void HideDialougeBox() {
        ThoughtCanvas.enabled = false;
        DialougeCanvas.enabled = false;
        Line.enabled = false;
        foreach(Canvas c in ExpressionCanvases) {
            c.enabled = false;
        }
       
    }

    public void SetCharacterColors(int characterIndex) {
        ThoughtCanvas.enabled = false;
        Color foreground = Characters.list[characterIndex].TextBoxColors.colors[(int)TextboxColorIndex.Foreground];
        Color background = Characters.list[characterIndex].TextBoxColors.colors[(int)TextboxColorIndex.Background];
        foreach (Image i in ForegroundImages) {
            i.color = foreground;
        }
        foreach (Image i in BackgroundImages) {
            i.color = background;
        }

        if (characterIndex == 10)
            Line.color = new Color(0, 0, 0, 0);
        else
            Line.color = background;
    }

    public void SetDialouge(string s) {
        ExpressionCanvases[CurrentExpression].enabled = true;
        Line.enabled = true;
        DialougeCanvas.enabled = true;
        TypeWriter.Run(s, DialougeText);
    }

    public void SetExpression(int Expression, bool showCanvas) {
        if (CurrentExpression == Expression)
            return;
        if (CurrentExpression >= 0)
            ExpressionCanvases[CurrentExpression].enabled = false;
        CurrentExpression = Expression;
        Line.enabled = showCanvas;
        ExpressionCanvases[CurrentExpression].enabled = showCanvas;
        DialougeCanvas.enabled = showCanvas;
    }

#if UNITY_EDITOR

    [SerializeField] Transform[] __forgroundHolders;
    [SerializeField] Transform[] __backgroundHolders;

    [SerializeField] bool __reset;

    private Image[] __setFromTransform(Transform[] list) {
        List<Image> array = new();
        for (int i = 0; i < list.Length; i++) {
            if (list[i].TryGetComponent(out Image im)) {
                array.Add(im);
            }
            array.AddRange(list[i].GetComponentsInChildren<Image>());
        }
        return array.ToArray();
    }

    private void OnValidate() {
        if (__reset) {
            __reset = false;

            ForegroundImages = __setFromTransform(__forgroundHolders);
            BackgroundImages = __setFromTransform(__backgroundHolders);
        }
    }

#endif
}

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DartsPartnerBanterDisplay : MonoBehaviour {
    [SerializeField] TMP_Text Dialouge;
    [SerializeField] Canvas DialougeCanvas;
    [SerializeField] Image PartnerImage;
    [SerializeField] Image DialougeForeGround;
    [SerializeField] Image[] DialougeBackGround;
    [SerializeField] UIAnimationElement EnterHead;
    [SerializeField] UIAnimationElement ExitHead;
    [SerializeField] Partner Partner;
    [SerializeField] bool IsFinal;

    public void SetPartner(Partner partner, bool isFinal) {
        Partner = partner;
        IsFinal = isFinal;
        DialougeCanvas.enabled = false;
        foreach(Image i in DialougeBackGround) {
            i.color = Partner.TextBoxColors.colors[(int)TextboxColorIndex.Background];
        }
        DialougeForeGround.color = Partner.TextBoxColors.colors[(int)TextboxColorIndex.Foreground];
    }

    public void GetDialougeFromScore() {
        string message = (IsFinal ? Partner.FinalsBanterLines : Partner.RegularBanterLines).GetCheckoutLine();
        SetImageFromScore(15000);
        SetDialouge(message);
    }

    void SetImageFromScore(int score) {
        if (score < 70) {
            PartnerImage.sprite = Partner.Expressions[(int)Expressions.Negative];
            return;
        }
        if (score >= 150) {
            PartnerImage.sprite = Partner.Expressions[(int)Expressions.Positive];
            return;
        }
    }

    public void GetDialougeFromScore(int score) {
        string message = (IsFinal ? Partner.FinalsBanterLines : Partner.RegularBanterLines).GetLineFromScoreGroup(score);
        SetImageFromScore(score);
        SetDialouge(message);
    }

    void SetDialouge(string message) {
        if (message == null)
            return;

        DialougeCanvas.enabled = true;
        Dialouge.text = message;
    }

    public void HideDialouge() {
        DialougeCanvas.enabled = false;
    }
}

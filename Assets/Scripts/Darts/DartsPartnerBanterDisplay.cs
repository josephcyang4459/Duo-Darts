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
    [SerializeField] DartGame DartGame;
    [SerializeField] bool IsFinal;
    [SerializeField] Timer PartnerBanterTimer;
    [SerializeField] float TimeToWait;
    [SerializeField] bool CheckOut;
    [SerializeField] float BanterBoxPadding;
    [SerializeField] Vector2 BanterBoxSize;
    [SerializeField] RectTransform BanterBoxOutline;
    [SerializeField] Image[] ToTurnOfForCheckoutbanter;
    [SerializeField] float MinBoxSize;
    [SerializeField] float MaxBoxSize;
    public void SetPartner(Partner partner, bool isFinal) {
        foreach(Image i in ToTurnOfForCheckoutbanter)
            i.enabled = true;
        Partner = partner;
        IsFinal = isFinal;
        DialougeCanvas.enabled = false;
        CheckOut = false;
        foreach(Image i in DialougeBackGround) {
            i.color = Partner.TextBoxColors.colors[(int)TextboxColorIndex.Background];
        }
        DialougeForeGround.color = Partner.TextBoxColors.colors[(int)TextboxColorIndex.Foreground];
    }

    bool NullCheck() {
        if (IsFinal && Partner.FinalsBanterLines == null)
            return true;
        if (!IsFinal && Partner.RegularBanterLines == null)
            return true;
        return false;
    }

    public void GetDialougeFromScore() {
        if (NullCheck())
            return;
        foreach (Image i in ToTurnOfForCheckoutbanter)
            i.enabled = false;
        string message = (IsFinal ? Partner.FinalsBanterLines : Partner.RegularBanterLines).GetCheckoutLine();
        SetImageFromScore(15000);
        CheckOut = true;
        SetDialouge(message);
    }

    void SetImageFromScore(int score) {
        if (score < 70) {
            PartnerImage.sprite = Partner.GetExpression((int)Expressions.Negative);
            return;
        }
        if (score >= 150) {
            PartnerImage.sprite = Partner.GetExpression((int)Expressions.Positive);
            return;
        }
        PartnerImage.sprite = Partner.GetExpression((int)Expressions.Nuetral);
    }

    public void GetDialougeFromScore(int score) {
        if (NullCheck())
            return;
        string message = (IsFinal ? Partner.FinalsBanterLines : Partner.RegularBanterLines).GetLineFromScoreGroup(score);
        SetImageFromScore(score);
        SetDialouge(message);
    }

    void SetDialouge(string message) {
        if (message == null)
            return;
        DialougeCanvas.enabled = true;
        Dialouge.text = message;
        BanterBoxSize.x = Mathf.Clamp(Dialouge.preferredWidth + BanterBoxPadding, MinBoxSize, MaxBoxSize);
        BanterBoxOutline.sizeDelta = BanterBoxSize;
        PartnerBanterTimer.BeginTimer(TimeToWait);
    }

    public void EndDisplay() {
        if (CheckOut && IsFinal)
            DartGame.GoToCorrectEnding();
        else {
            HideDialouge();
        }
    }

    public void HideDialouge() {
        DialougeCanvas.enabled = false;
    }
}

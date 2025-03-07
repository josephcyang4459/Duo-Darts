using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DartVisual : MonoBehaviour {
    [SerializeField] SpriteCollection Darts;
    [SerializeField] Canvas Canvas;
    [SerializeField] TMP_Text remainingScoreText;
    [SerializeField] TMP_Text currentScoreText;
    [SerializeField] TMP_Text turnText;
    [SerializeField] Image[] DartImages;
    [SerializeField] TMP_Text[] scoreTexts;
    [SerializeField] Fillable_SeeSaw[] DartPlaques;
    [SerializeField] [Range(1, 7)] float FillSpeed =3;
    [SerializeField] UIAnimationElement EndScreneAnimationhead;
    [SerializeField] UIAnimationElement EndScreenResetHead;
    [SerializeField] TMP_Text ResultText;
    [SerializeField] Image[] CharacterPortraits;
    [SerializeField] Image[] ColorFills;
    [SerializeField] string[] VictorySayings;
    [SerializeField] string[] LoseSayings;
    [SerializeField] Canvas ResultCanvas;
    List<Fillable_SeeSaw> CurrentlyFilling = new();

    public void RandomizeDartImages() {
        int temp = Random.Range(0,Darts.Sprites.Length);
        for (int i = 0; i < 3; i++) {
            DartImages[i].sprite = Darts.Sprites[temp];
        }
    }

    public void ShowCanvas(bool b) {
        Canvas.enabled = b;
    }

    public void SetTurnAndOverallScores(int turnScore, int overallScore, int currentTurn, int maxTurn) {
        remainingScoreText.text = overallScore.ToString();
        currentScoreText.text = turnScore.ToString();
        turnText.text = (maxTurn - (currentTurn + 1)).ToString();
    }

    public void SetDartScore(int dartIndex, int score) {
        scoreTexts[dartIndex].text = score.ToString();
        enabled = true;
        CurrentlyFilling.Add(DartPlaques[dartIndex]);
    }

    public void SetDartScore() {
        RandomizeDartImages();
        for (int i = 0; i < 3; i++) {
            DartPlaques[i].SetFill(0);
        }
        while (CurrentlyFilling.Count > 0)
            CurrentlyFilling.RemoveAt(0);
    }

    public void SetTurnScore(int turnScore) { currentScoreText.text = turnScore.ToString(); }

    public void SetResultScreen(bool win, Partner partner) {
        ResultCanvas.enabled = true;
        EndScreenResetHead.ReachEndState();
        ResultText.text = win ? VictorySayings[Random.Range(0,VictorySayings.Length)] : LoseSayings[Random.Range(0, LoseSayings.Length)];
        for(int i = 0; i < CharacterPortraits.Length; i++) {
            CharacterPortraits[i].sprite = partner.GetExpression(win ? (int)Expressions.Positive : (int)Expressions.Negative);
        }
        for (int i = 0; i < ColorFills.Length; i++) {
            ColorFills[i].color = partner.TextBoxColors.colors[(int)TextboxColorIndex.Background];
        }
        EndScreneAnimationhead.Begin(null);
    }

    public void SetResultScreen() {
        ResultCanvas.enabled = false;
    }

    private void Update() {
        if (CurrentlyFilling.Count <= 0)
            enabled = false;
        float dTime = Time.deltaTime * FillSpeed;
        for(int i=CurrentlyFilling.Count-1;i>=0;i--) {
            float temp = CurrentlyFilling[i].GetFill();
            if (temp >= 1) {
                CurrentlyFilling.RemoveAt(i);
            }
            else
                CurrentlyFilling[i].SetFill(Mathf.MoveTowards(temp, 1, dTime));
        }
        if (CurrentlyFilling.Count <= 0)
            enabled = false;
    }
}

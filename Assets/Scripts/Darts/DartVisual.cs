using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DartVisual : MonoBehaviour {
    [SerializeField] TMP_Text remainingScoreText;
    [SerializeField] TMP_Text currentScoreText;
    [SerializeField] TMP_Text turnText;
    public Image[] dartimages;
    public TMP_Text[] scoreTexts;


    public void SetTurnAndOverallScores(int turnScore, int overallScore, int currentTurn, int maxTurn) {
        remainingScoreText.text = "Remaining: " + overallScore.ToString();
        currentScoreText.text = "Current: " + turnScore.ToString();
        turnText.text = "Turns Remaining: " + (maxTurn - (currentTurn+1)); ;
    }

    public void SetDartScore(int dartIndex, int score) {
        scoreTexts[dartIndex].text = "Dart " + (dartIndex + 1).ToString() + ": " + score.ToString();
        dartimages[dartIndex].enabled = false;
    }

    public void SetDartScore() {
        for (int i = 0; i < 3; i++) {
            scoreTexts[i].text = "";
            dartimages[i].enabled = true;
        }
    }

    public void SetTurnScore(int turnScore) { currentScoreText.text = "Current: " + turnScore.ToString(); }
}

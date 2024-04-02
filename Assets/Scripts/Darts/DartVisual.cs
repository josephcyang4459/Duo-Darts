using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DartVisual : MonoBehaviour
{
    [SerializeField] TMP_Text OverallScore;
    [SerializeField] TMP_Text TurnScore;
    public Image[] dartimages;
    public TMP_Text[] scores;

    public void SetTurnAndOverallScores(int turnScore, int overallScore) {
        TurnScore.text = turnScore.ToString();
        OverallScore.text = overallScore.ToString();
    }

    public void SetDartScore(int dartIndex, int score) {
        scores[dartIndex].text = score.ToString();
        dartimages[dartIndex].enabled = false;
    }

    public void SetDartScore() {
        for (int i = 0; i < 3; i++) {
            scores[i].text = "";
            dartimages[i].enabled = true;
        }
    }

    public void SetTurnScore(int turnScore) {
        TurnScore.text = turnScore.ToString();
    }
}

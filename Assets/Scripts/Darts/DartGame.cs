using UnityEngine;

public class DartGame : MonoBehaviour, TransitionCaller {
    [SerializeField] CharacterList characters;
    [SerializeField] Player stats;
    [SerializeField] DartVisual Visuals;
    public DartScript Dart;
    [SerializeField] DartBoardAI DartBoardTarget;
    [SerializeField] DartBoard DartBoard;
    [SerializeField] DartPlayerAim Aim;
    [SerializeField] DartsPartnerBanterDisplay Banter;
    public int PartnerIndex;
    [SerializeField] DartsSettings Settings;

    public Partner CurrentPartner;
    public int turnSum = 0;
    public int ScoreNeededToWin = 501;
    public int points;
    public int currentTurn = 0;
    public int numberOfDartsThrow = 0;
    public int maxTurns;

    [SerializeField] float MaxOffset = 4;


    public BoardCollider bullseye;
    public BoardCollider Miss;

    [SerializeField] Timer EndTimer;
    [SerializeField] float WaitForEndTime;
    [SerializeField] Canvas winc;
    [SerializeField] Canvas losec;
    [SerializeField] AudioClip ac;
    [SerializeField] SpriteRenderer board;
    [SerializeField] InSceneTransition Transition;

    [SerializeField] Schedule s;
    [SerializeField] DartMenu_StandAlone StandAlone;

    bool IsFinals() {
        if (s == null)
            return false;
        if (s.hour < 8)
            return false;
        if (s.hour > 8)
            return true;
        if (s.hour == 8)
            if (s.minutes >= 30)
                return true;
        return false;
    }



    public void BeginGame() {
        CurrentPartner = characters.list[PartnerIndex];
        points = ScoreNeededToWin > 600 ? 10 : 5;
        board.enabled = true;

        UIState.inst.SetInteractable(false);
        Aim.SetUpDependants();
        Audio.inst.PlaySong(ac);

        Dart.SetUp(PartnerIndex);
        Banter.SetPartner(CurrentPartner, IsFinals());
        CurrentPartner.ResetBanterLineUsage();
        Visuals.SetDartScore();
        currentTurn = 0;
        turnSum = 0;
        Visuals.SetTurnAndOverallScores(turnSum, ScoreNeededToWin, currentTurn, maxTurns);
        numberOfDartsThrow = 0;
        Visuals.ShowCanvas(true);
        playerTurn();
    }

    public void Lose() {
        GameEnd();

        losec.enabled = true;
        EndTimer.BeginTimer(WaitForEndTime);
    }

    public void Win() {
        GameEnd();
        if (IsFinals())
            Banter.GetDialougeFromScore();
        else {
            winc.enabled = true;
            EndTimer.BeginTimer(WaitForEndTime);
        }
            
    }

    public void GoToCorrectEnding() {
        if (s != null) {
            stats.TotalPointsScoredAcrossAllDartMatches += points;
            if (IsFinals()) {
                switch ((CharacterNames)PartnerIndex) {
                    case CharacterNames.Chad:
                        TransitionManager.inst.GoToScene(SceneNumbers.ChadEnding);
                        return;
                    case CharacterNames.Jess:
                        TransitionManager.inst.GoToScene(SceneNumbers.JessEnding);
                        return;
                    case CharacterNames.Faye:
                        TransitionManager.inst.GoToScene(SceneNumbers.FayeEnding);
                        return;
                    case CharacterNames.Elaine:
                        TransitionManager.inst.GoToScene(SceneNumbers.ElaineEnding);
                        return;
                }
            }
        }
    }

    void GameEnd() {
        board.enabled = false;
        Visuals.ShowCanvas(false);
    }

    public void SwitchTurn() {
        Dart.ResetDartPositions();
        Visuals.SetDartScore();


        int currentTurnSum = turnSum;

        ScoreNeededToWin -= turnSum;
        turnSum = 0;
        numberOfDartsThrow = 0;
        currentTurn++;
        Visuals.SetTurnAndOverallScores(turnSum, ScoreNeededToWin, currentTurn, maxTurns);

        if (currentTurn >= maxTurns) {
            Lose();
            return;
        }
        if ((currentTurn - 1) % 2 == 0) {
            Banter.GetDialougeFromScore(currentTurnSum);
        }
           

        if (currentTurn % 2 == 0)
            playerTurn();
        else
            PartnerTurn();
    }

    private void playerTurn() {
        Dart.SetCurrentDart(numberOfDartsThrow, true);
        Aim.BeginPlayerAim();
    }

    //used to hit the board
    private void PartnerShootDart(Vector3 h) {
        h.z = -.2f;
        Dart.ShootDart(h, DartBoard.GetScoreFromLocation(h.x, h.y));
    }

    void Adjust(Vector3 location, float baseOffset) {
        float OffsetMath() {
            float f = Random.Range((CurrentPartner.Intoxication / -7) - baseOffset, (CurrentPartner.Intoxication / 7) + baseOffset);
            f = Mathf.Clamp(f, -MaxOffset, MaxOffset);
            return f;
        }

        float offsetX = OffsetMath();
        float offsetY = OffsetMath();
        location.x += offsetX;
        location.y += offsetY;
        PartnerShootDart(location);
    }

    public void PartnerTarget(int score, int ring, float baseOffset) { Adjust(DartBoardTarget.GetTarget(score,ring), baseOffset); }

    /// <summary>
    /// Bullseye
    /// </summary>
    /// <param name="baseOffset"></param>
    public void PartnerTarget(float baseOffset) {
        Adjust(DartBoardTarget.GetTarget(), baseOffset);
    }

    private void PartnerTurn() {
        Dart.SetCurrentDart(numberOfDartsThrow, false);
        int tempScore = ScoreNeededToWin - turnSum;
        CurrentPartner.AI.SelectTarget(tempScore, this);
        return;
    }

    /// <summary>
    /// called by DartScript for some reason
    /// </summary>
    public void AddPoints(int newPoints) {
        if (newPoints == 50) {
            Audio.inst.PlayDartClipReverb(DartAudioClips.Medium, AudioReverbPreset.Cave);
        }
        else if (newPoints == 60) {
            Audio.inst.PlayDartClipReverb(DartAudioClips.Hard, AudioReverbPreset.Drugged);
        }
        else if (newPoints == 0) {
            Audio.inst.PlayDartClipReverb(DartAudioClips.Soft, AudioReverbPreset.Bathroom);
        }
        else {
            Audio.inst.PlayClip(AudioClips.RandomDart);
        }

        turnSum += newPoints;
        Visuals.SetTurnScore(turnSum);
        Visuals.SetDartScore(numberOfDartsThrow, newPoints);
    }

    /// <summary>
    /// Called By Darts Timer
    /// </summary>
    public void CheckForBust() {
        //Banter.HideDialouge();
        if (ScoreNeededToWin - turnSum < 0) {
            turnSum = 0;
            SwitchTurn();
            return;
        }

        if (ScoreNeededToWin - turnSum == 0) {
            Dart.ResetDartPositions();
            Win();
            return;
        }

        numberOfDartsThrow++;

        if (numberOfDartsThrow >= 3) {
            SwitchTurn();
            return;
        }
        else
           if (currentTurn % 2 == 0) {
            playerTurn();
            return;
        }
        else {
            PartnerTurn();
            return;
        }
    }

    public void NowHidden() {
        if (s != null)
            s.SetTime(TimeBlocks.Short);
        else
            StandAlone.BeginSetUp();
    }

    /// <summary>
    /// Called By End Timer
    /// </summary>
    public void EndDartsGame() {
        winc.enabled = false;
        losec.enabled = false;
        board.enabled = false;
        Banter.HideDialouge();
        // PauseMenu.inst.SetEnabled(true);
        Transition.BeginTransition(this);
    }
}
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class DartGame : MonoBehaviour {
    public CharacterList characters;
    public Player stats;
    public DartVisual Visuals;
    public DartScript Dart;
    public DartPlayerAim Aim;

    [SerializeField] DartsSettings Settings;

    public int partnerIndex = 0;
    public int turnSum = 0;
    public int ScoreNeededToWin = 501;
    public int points;
    public int currentTurn = 0;
    public int numberOfDartsThrow = 0;
    public int maxTurns;
    public bool firstTimePlaying;

    [SerializeField] float MaxOffset = 4;
    public Canvas dartCanvas;

    [SerializeReference] public BoardSlice[] c;
    public BoardCollider bullseye;
    public BoardCollider Miss;

    [SerializeField] Timer EndTimer;
    [SerializeField] float WaitForEndTime;
    public Canvas winc;
    public Canvas losec;
    public AudioClip ac;
    public SpriteRenderer board;

    public Schedule s;
    public DartMenu_StandAlone StandAlone;
    [SerializeField] GameObject Tutorial;

#if UNITY_EDITOR
    [Header("||-----EDITOR ONLY-----||")]
    public byte[] order;
    public byte[] multiplication;
    public bool reset;
    public GameObject g;
    public GameObject slice;

    public void OnValidate() {
        if (reset) {
            c = new BoardSlice[20];

            for (int i = 0; i < 20; i++) {
                GameObject obj = Instantiate(slice, g.transform);
                obj.transform.rotation = Quaternion.Euler(-18 * i, -90, 0);

                for (int j = 0; j < 4; j++) {
                    obj.transform.GetChild(j).GetComponent<BoardCollider>().point = (order[i] * multiplication[j]);
                    obj.transform.GetChild(j).GetComponent<BoardCollider>().gameState = this;
                    obj.transform.GetChild(j).GetComponent<BoardCollider>().mr = obj.transform.GetChild(j).GetComponent<MeshRenderer>();
                }

                c[i] = obj.GetComponent<BoardSlice>();
            }

            System.Array.Sort(c, new comparer());
            reset = false;
        }
    }

    public class comparer : IComparer<BoardSlice> {
        public int Compare(BoardSlice a, BoardSlice b) {
            return a.colliders[0].point - b.colliders[0].point;
        }
    }
#endif

    public void BeginGame() {
        UIState.inst.SetInteractable(true);
        if (firstTimePlaying) {
            showTutorial();
            return;
        }

        Aim.SetUpDependants();
        /*DartSticker.inst.SetVisible(false);
        PauseMenu.inst.SetEnabled(false);*/
        Audio.inst.PlaySong(ac);
        //UI_Helper.SetSelectedUIElement(s.c.voiddd);
        board.enabled = true;
        points = ScoreNeededToWin > 600 ? 10 : 5;
        //Debug.Log(Accuracy);
        //float Stability = Math.Clamp((30/stats.Skill) + ((stats.Intoxication/3) / 10), 1,100);// gooffy ass
        //aim.driftSpeed = driftDefault * Stability;
        //aim.driftSpeed = driftDefault;
        //aim.moveSpeed = (1.35f -(stats.Intoxication / 5)) / 10 * aim.driftSpeed;// more goofy ass math
        
        Dart.SetUp(partnerIndex);
        Dart.reset_position();
        Visuals.SetDartScore();
        currentTurn = 0;
        turnSum = 0;
        Visuals.SetTurnAndOverallScores(turnSum, ScoreNeededToWin, currentTurn, maxTurns);
        numberOfDartsThrow = 0;
        dartCanvas.enabled = true;

        playerTurn();
    }

    public void Lose() {
        GameEnd();
        losec.enabled = true;
        if (s != null) {
            if (s.hour == 8)
                if (s.minutes > 50) {
                    Debug.Log("PLAY BAD ENDING HERE");
                    TransitionManager.inst.GoToScene(SceneNumbers.DidNotWinTheTournament);
                }
        }

        EndTimer.BeginTimer(WaitForEndTime);
    }

    public void Win() {
        GameEnd();
        winc.enabled = true;
        if (s != null) {
            stats.TotalPointsScoredAcrossAllDartMatches += points;
            if (s.hour == 8)
                if (s.minutes >= 30) {
                    switch ((CharacterNames)partnerIndex) {
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
        EndTimer.BeginTimer(WaitForEndTime);
    }

    void GameEnd() {
        board.enabled = false;
        dartCanvas.enabled = false;
    }

    public void SwitchTurn() {
        Dart.reset_position();
        //Debug.Log("swap");
        Visuals.SetDartScore();

        ScoreNeededToWin -= turnSum;
        turnSum = 0;
        numberOfDartsThrow = 0;
        currentTurn++;
        Visuals.SetTurnAndOverallScores(turnSum, ScoreNeededToWin, currentTurn, maxTurns);

       
       
        if (currentTurn >= maxTurns) {
            Lose();
            return;
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
        if (Physics.Raycast(h, Vector3.forward, out RaycastHit hit, 12, Settings.DartsLayerMask)) {
            ///Debug.Log(aim.t.position);
            hit.collider.gameObject.GetComponent<BoardCollider>().hit(h);
        }
        else
            Miss.hit(h);
    }

    void Adjust(Vector3 location, float baseOffset) {
        float OffsetMath() {
            float f = Random.Range((characters.list[partnerIndex].Intoxication / -7) - baseOffset, (characters.list[partnerIndex].Intoxication / 7) + baseOffset);
            f = Mathf.Clamp(f, -MaxOffset, MaxOffset);
            return f;
        }

        float offsetX = OffsetMath();
        float offsetY = OffsetMath();
        location.x += offsetX;
        //Mathf.Clamp(location.x, -MissClamp.x, MissClamp.x);
        location.y += offsetY;
        //Mathf.Clamp(location.y, -MissClamp.y, MissClamp.y);
        //location.z = -15;
        PartnerShootDart(location);
    }

    public void PartnerTarget(int score, int ring, float baseOffset) { Adjust(c[score - 1].colliders[ring].target.position, baseOffset); }

    /// <summary>
    /// Bullseye
    /// </summary>
    /// <param name="baseOffset"></param>
    public void PartnerTarget(float baseOffset) {
        Debug.Log(50);
        Adjust(bullseye.transform.position, baseOffset);
    }

    private void PartnerTurn() //wow really hideous
    {
        Dart.SetCurrentDart(numberOfDartsThrow, false);
        int tempScore = ScoreNeededToWin - turnSum;
        characters.list[partnerIndex].AI.SelectTarget(tempScore, this);
        return;
    }

    /// <summary>
    /// called by DartScript for some reason
    /// </summary>
    public void AddPoints(int newPoints) {
        if (newPoints == 50) {
            Audio.inst.PlayDartClipReverb(DartAudioClips.Medium, AudioReverbPreset.Cave);
        }
        else if(newPoints == 60) {
            Audio.inst.PlayDartClipReverb(DartAudioClips.Hard, AudioReverbPreset.Drugged);
        }
        else if(newPoints==0){
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
    /// called by DartScript for some reason
    /// </summary>
    public void CheckForBust() {
        if (ScoreNeededToWin - turnSum < 0) {
            turnSum = 0;
            SwitchTurn();
            return;
        }

        if (ScoreNeededToWin - turnSum == 0) {
            Dart.reset_position();
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

    /// <summary>
    /// Called By Timer
    /// </summary>
    public void EndDartsGame() {
        winc.enabled = false;
        losec.enabled = false;
        board.enabled = false;
        // PauseMenu.inst.SetEnabled(true);
        if (s != null)
            s.SetTime(TimeBlocks.Short);
        else
            StandAlone.BeginSetUp();
    }

    public void showTutorial() { Tutorial.SetActive(true); }

    public void hideTutorial() {
        Tutorial.SetActive(false);
        firstTimePlaying = false;
        UIState.inst.SetInteractable(false);
        BeginGame();
    }
}
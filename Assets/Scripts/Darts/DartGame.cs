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


    [SerializeField] Vector2Int NuetralAITargetRange = new Vector2Int(16, 19);
    [SerializeField] float BaseOffset = .1f;
    [SerializeField] float MaxOffset = 4;
    public Canvas dartCanvas;

    [SerializeReference] public BoardSlice[] c;
    public BoardCollider bullseye;
    public BoardCollider Miss;

    public WaitForSeconds sec = new WaitForSeconds(5);
    public Canvas winc;
    public Canvas losec;
    public AudioClip ac;
    public SpriteRenderer board;

    public Schedule s;
    public DartMenu_StandAlone StandAlone;

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
        Aim.SetUpDependants();
        DartSticker.inst.SetVisible(false);
        PauseMenu.inst.SetEnabled(false);
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
                    UnityEngine.SceneManagement.SceneManager.LoadScene((int)SceneNumbers.DidNotWinTheTournament);
                }
        }


        StartCoroutine(WaitToEnd());
    }

    public void Win() {
        GameEnd();
        winc.enabled = true;
        if (s != null) {
            stats.TotalPointsScoredAcrossAllDartMatches += points;
            if (s.hour == 8)
                if (s.minutes > 30) {
                    switch ((Characters)partnerIndex) {
                        case Characters.Chad:
                            UnityEngine.SceneManagement.SceneManager.LoadScene((int)SceneNumbers.ChadEnding);
                            return;
                        case Characters.Elaine:
                            UnityEngine.SceneManagement.SceneManager.LoadScene((int)SceneNumbers.ElaineEnding);
                            return;
                        case Characters.Jess:
                            UnityEngine.SceneManagement.SceneManager.LoadScene((int)SceneNumbers.JessEnding);
                            return;
                        case Characters.Faye:
                            UnityEngine.SceneManagement.SceneManager.LoadScene((int)SceneNumbers.FayeEnding);
                            return;
                    }
                }
        }
        StartCoroutine(WaitToEnd());
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

        if (currentTurn % 2 == 0) {
            playerTurn();
        }
        else {
            PartnerTurn();
        }
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

    public void PartnerTarget(int score, int ring, float baseOffset) {
        Adjust(c[score - 1].colliders[ring].target.position, baseOffset);
    }
    /// <summary>
    /// Bullseye
    /// </summary>
    /// <param name="baseOffset"></param>
    public void PartnerTarget(float baseOffset) {
        Debug.Log(50);
        Adjust(bullseye.transform.position, baseOffset);
    }

    private void PartnerTurn()//wow really hideous
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
        Audio.inst.PlayClip(AudioClips.Dart);
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

    public IEnumerator WaitToEnd() {
        yield return sec;

        winc.enabled = false;
        losec.enabled = false;
        board.enabled = false;
        PauseMenu.inst.SetEnabled(true);
        if (s != null) {
            s.setTime(TimeBlocks.Short);
        }
        else {
            StandAlone.BeginSetUp();
        }

    }
}
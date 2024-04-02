using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class DartGame : MonoBehaviour {
    public CharacterList characters;
    public Player stats;
    public DartVisual Visuals;
    public AimCone aim;
    public DartScript Dart;


    public int partnerIndex = 0;
    public int turnSum = 0;
    public int ScoreNeededToWin = 501;
    public int points;
    public int currentTurn = 0;

    public int numberOfDartsThrow = 0;
    public int maxTurns;


    [SerializeField] Vector2Int NuetralAITargetRange = new Vector2Int(16, 19);
    [SerializeField] float BaseOffset = .1f;
    [SerializeField] float MaxOffset = 4;
    public Canvas dartCanvas;

    public float driftDefault = 1f;

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

    public void BeginGame(int partner) {
        PauseMenu.inst.SetEnabled(false);
        Audio.inst.PlaySong(ac);
        //UI_Helper.SetSelectedUIElement(s.c.voiddd);
        board.enabled = true;

        points = ScoreNeededToWin > 600 ? 10 : 5;
        aim.accuracy = (Mathf.Clamp((stats.Intoxication * 2) - (stats.Skill + stats.Luck), 0, 100)) / 10;// crazy f+ucking math
        //Debug.Log(Accuracy);
        //float Stability = Math.Clamp((30/stats.Skill) + ((stats.Intoxication/3) / 10), 1,100);// gooffy ass
        //aim.driftSpeed = driftDefault * Stability;
        aim.driftSpeed = driftDefault;
        //aim.moveSpeed = (1.35f -(stats.Intoxication / 5)) / 10 * aim.driftSpeed;// more goofy ass math
        partnerIndex = partner;
        Dart.SetUp(partnerIndex);
        Dart.reset_position();
        Visuals.SetDartScore();
        currentTurn = 0;
        turnSum = 0;
        Visuals.SetTurnAndOverallScores(turnSum, ScoreNeededToWin);
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
        Visuals.SetTurnAndOverallScores(turnSum, ScoreNeededToWin);

        numberOfDartsThrow = 0;
        currentTurn++;
        if (currentTurn >= maxTurns) {
            Lose();
            return;
        }

        if (currentTurn % 2 == 0) {

            playerTurn();
        }
        else {
            partnerTurn();
        }
    }

    private void playerTurn() {
        Dart.SetCurrentDart(numberOfDartsThrow, true);
        aim.begin();
    }

    //used to hit the board
    private void gahoot(Vector3 h) {
        aim.normal.x = h.x;
        aim.normal.y = h.y;
        aim.t.position = aim.normal;
        if (Physics.Raycast(aim.t.position, Vector3.forward, out RaycastHit hit, aim.layer)) {
            ///Debug.Log(aim.t.position);
            hit.collider.gameObject.GetComponent<BoardCollider>().hit();
        }
        else
            Miss.hit();
    }

    private void Adjust(Vector3 location) {
        float OffsetMath() {
            float f = UnityEngine.Random.Range((characters.list[partnerIndex].Intoxication / -7) - BaseOffset, (characters.list[partnerIndex].Intoxication / 7) + BaseOffset);
            f = Mathf.Clamp(f, -MaxOffset, MaxOffset);
            return f;
        }

        float offsetX = OffsetMath();
        float offsetY = OffsetMath();
        Debug.Log(offsetX + " " + offsetY);
        location.x += offsetX;
        //Mathf.Clamp(location.x, -MissClamp.x, MissClamp.x);
        location.y += offsetY;
        //Mathf.Clamp(location.y, -MissClamp.y, MissClamp.y);
        Debug.Log(location);
        //location.z = -15;
        gahoot(location);
    }

    private void partnerTurn()//wow really hideous
    {
        Dart.SetCurrentDart(numberOfDartsThrow, false);
        void OverSixtyPick() {

            if (characters.list[partnerIndex].bias == DartTargetBias.Bullseye) {
                Adjust(bullseye.transform.position);
                return;
            }

            if (characters.list[partnerIndex].bias == DartTargetBias.Sixty) {
                Adjust(c[19].colliders[2].target.position);
                return;
            }
            //nuetral AI
            int pick = Random.Range(NuetralAITargetRange.x, NuetralAITargetRange.y + 1);

            int randomNumberBetween0_9 = Random.Range(0, 10);
            if (characters.list[partnerIndex].Composure < 4)
            {
                if (randomNumberBetween0_9 > 5)// 9,8,7,6 40% chance to go for bullseye
                {
                    Adjust(bullseye.transform.position);
                    return;
                }
                Adjust(c[pick].colliders[(int)PointValueTarget.Double].target.position);
                return;
            }

            if (characters.list[partnerIndex].Composure >= 4) {
                if (randomNumberBetween0_9 > 5)// 7,8,9,6 40% chance to go for triple
                {
                    Adjust(c[19].colliders[(int)PointValueTarget.Triple].target.position);
                    return;
                }
                if (randomNumberBetween0_9 < 3)// 0,1,2 30% chance to go for bullseye
                {
                    Adjust(bullseye.transform.position);
                    return;
                }
                Adjust(c[pick].colliders[(int)PointValueTarget.Double].target.position);
                return;
            }

            PointValueTarget temp = Random.Range(0, 10) > 4 ? PointValueTarget.OuterSingle : PointValueTarget.InnerSingle;
            Adjust(c[pick].colliders[(int)temp].target.position);
            return;
        }

        int tempScore = ScoreNeededToWin - turnSum;
        if (tempScore >= 60)// this is where they would go for big numbers
        {
            OverSixtyPick();
            return;
        }

        if (tempScore >= 50)// always goes for bullseye
        {
            Adjust(bullseye.transform.position);
            return;
        }

        if (tempScore > 20)// goes for random small
        {
            int temp = Random.Range(4, 7);//from 4 to 6 so points from  15, 18, 21
            Adjust(c[temp].colliders[(int)PointValueTarget.Triple].target.position);
            return;
        }

        if (tempScore <= 20)//goes for single to win
        {// Whether we Inner or Outer Just to add to the presentation
            int target = Random.Range(0, 10) > 4 ? (int)PointValueTarget.OuterSingle : (int)PointValueTarget.InnerSingle;
            //must adjust -1 to get correct target
            Adjust(c[tempScore - 1].colliders[target].target.position);
            return;
        }
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
            partnerTurn();
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
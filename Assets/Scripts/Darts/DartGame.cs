using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using System;

public class DartGame : MonoBehaviour
{


    public int turnSum = 0;
    public int overall = 501;
    public int points;
    public int currentTurn = 0;

    public int partnerIndex =0;

    public AimCone aim;
    [SerializeField] Vector2Int NuetralAITargetRange = new Vector2Int(17, 19);

    public int numberOfDartsThrow = 0;
    public int maxTurns;
    public TMP_Text overallScore;
    public TMP_Text turnScore;
    public WaitForSeconds k = new WaitForSeconds(3);

    public Material flash;
    public Material gone;
    public DartScript[] Dart;
    public Image[] dartimages;
    public TMP_Text[] scores;
    public Canvas dartCanvas;

    public Material green;
    public Material red;

    public float driftDefault = 1f;

    [SerializeReference] public BoardSlice[] c;
    public BoardCollider bullseye;
    public Player stats;
    public Partner[] partners;

    public Schedule s;

    public WaitForSeconds sec = new WaitForSeconds(5);
    public Canvas winc;
    public Canvas losec;
    public AudioClip ac;
    public AudioClip hit;
    public SpriteRenderer board;

   

#if UNITY_EDITOR
    public byte[] order;
    public byte[] multiplication;
    public bool reset;
    public GameObject g;
    public GameObject slice;

    
    public void OnValidate()
    {
        if (reset)
        {
            c = new BoardSlice[20];
            for (int i = 0; i < 20; i++)
            {

                
                GameObject obj = Instantiate(slice, g.transform);
                
                obj.transform.rotation = Quaternion.Euler(-18 * i, -90, 0);
                for (int j = 0; j < 4; j++)
                {

                    obj.transform.GetChild(j).GetComponent<BoardCollider>().point = (byte)(order[i] * multiplication[j]);
                    obj.transform.GetChild(j).GetComponent<BoardCollider>().gameState = this;
                    obj.transform.GetChild(j).GetComponent<BoardCollider>().mr = obj.transform.GetChild(j).GetComponent<MeshRenderer>();
                }
                c[i] = obj.GetComponent<BoardSlice>();
            }

            Array.Sort(c, new comparer());

            reset = false;
        }
    }

    public class comparer : IComparer<BoardSlice>
    {
        public int Compare(BoardSlice x, BoardSlice y)
        {
            return x.colliders[0].point - y.colliders[0].point;
        }
    }
#endif

    public void BeginGame(int partner)
    {
        PauseMenu.inst.SetEnabled(false);
        //UI_Helper.SetSelectedUIElement(s.c.voiddd);
        ControlTutuorialUI.inst.SetControl((int)Controls.DartsGame, true);
        board.enabled = true;
        DartC(green);
        Audio.inst.PlaySong(ac);
        points = overall > 600 ? 10 : 5;
        aim.accuracy = (Mathf.Clamp((stats.Intoxication * 2) - (stats.Skill + stats.Luck),0,100)) / 10;// crazy f+ucking math
        //Debug.Log(Accuracy);
        //float Stability = Math.Clamp((30/stats.Skill) + ((stats.Intoxication/3) / 10), 1,100);// gooffy ass
        //aim.driftSpeed = driftDefault * Stability;
        aim.driftSpeed = driftDefault;
        //aim.moveSpeed = (1.35f -(stats.Intoxication / 5)) / 10 * aim.driftSpeed;// more goofy ass math

        partnerIndex = partner;

        for (int i = 0; i < 3; i++)
        {
            dartimages[i].enabled = true;
            scores[i].text = "";
            Dart[i].reset_position();
        }

        turnSum = 0;
        turnScore.text = turnSum.ToString();
        overallScore.text = overall.ToString();


        numberOfDartsThrow = 0;

        dartCanvas.enabled = true;
        numberOfDartsThrow = 0;
        currentTurn = 0;
        turnScore.text = turnSum.ToString();
        overallScore.text = overall.ToString();
        playerTurn();
    }

    public void Lose()
    {
        //Debug.Log("lose");
        GameEnd();
        losec.enabled = true;

        if (s.hour == 8)
            if (s.minutes > 50)
            {
                Debug.Log("PLAY BAD ENDING HERE");
                UnityEngine.SceneManagement.SceneManager.LoadScene((int)SceneNumbers.DidNotWinTheTournament);
            }

        StartCoroutine(WaitToEnd());
    }

    public void Win()
    {
        //Debug.Log("win");
        GameEnd();
        stats.TotalPointsScoredAcrossAllDartMatches += points;
        winc.enabled = true;
        if (s.hour == 8)
            if (s.minutes > 30)
            {
                Debug.Log("PLAY GOOD ENDING HERE");
                switch ((Characters)partnerIndex)
                {
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
               
        StartCoroutine(WaitToEnd());
    }

    void GameEnd()
    {
        board.enabled = false;
        dartCanvas.enabled = false;
    }

    public void SwitchTurn()
    {
        //Debug.Log("swap");
        for( int i = 0; i < 3; i++)
        {
            dartimages[i].enabled = true;
            scores[i].text = "";
            Dart[i].reset_position();
        }
        
        overall -= turnSum;
        turnSum = 0;
        turnScore.text = turnSum.ToString();
        overallScore.text = overall.ToString();

        
        numberOfDartsThrow = 0;
        currentTurn++;
        if (currentTurn >= maxTurns)
        {
            Lose();
            return;
        }

        if (currentTurn % 2 == 0)
        {
            DartC(green);
            playerTurn();
        }
        else
        {
            DartC(red);
            partnerTurn();
        }    
    }

    private void playerTurn()
    {
       
        aim.begin();
    }

    private void DartC(Material m)
    {
        for (int i = 0; i < 3; i++)
            Dart[i].m.material = m;
    }
    
    //used to hit the board
    private void gahoot(Vector3 h)
    {
        aim.normal.y = h.y;
        aim.normal.x = h.x;
        aim.t.position = aim.normal;
        if (Physics.Raycast(aim.t.position, Vector3.forward, out RaycastHit hit, aim.layer))
        {
            ///Debug.Log(aim.t.position);
            hit.collider.gameObject.GetComponent<BoardCollider>().hit();
        }
    }

    private void Adjust(Vector3 location, float offset)
    {
        
        location.x += offset;
        location.y += offset;
        //location.z = -15;
        gahoot(location);
    }

    private void partnerTurn()//wow really hideous
    {
        float offset = UnityEngine.Random.Range(((partners[partnerIndex].Intoxication) / -7) - .1f, ((partners[partnerIndex].Intoxication) / 7) + .1f) *  ((partners[partnerIndex].Intoxication) / 2);
        //Debug.Log(offset);

        void OverSixtyPick()
        {

            if (partners[partnerIndex].bias == DartTargetBias.Bullseye)//chad
            {

                Adjust(bullseye.transform.position, offset);
                return;
            }

            if (partners[partnerIndex].bias == DartTargetBias.Sixty)//elaine
            {
                Adjust(c[19].colliders[2].target.position, offset);
                return;
            }


            int pick = UnityEngine.Random.Range(NuetralAITargetRange.x, NuetralAITargetRange.y+1);

            PointValueTarget temp = PointValueTarget.OuterSingle;

            if (partners[partnerIndex].Composure >= 5)
            {
                temp = 0;
            }

            if (partners[partnerIndex].Composure >= 10)// high composure
            {
                int trye = UnityEngine.Random.Range(0, 10);

                if (trye > 7)// 8,9 20% chance to go for triple
                {
                    Adjust(c[19].colliders[(int)PointValueTarget.Triple].target.position, offset);
                    return;
                }

                if (trye > 4)// 5,6,7 30% chance to go for bullseye
                {
                    Adjust(bullseye.transform.position, offset);
                    return;
                }

                temp = PointValueTarget.Double;

            }



            Adjust(c[pick].colliders[(int)temp].target.position, offset);

            return;
        }

        int tempScore = overall - turnSum;
        if (tempScore >= 60)// this is where they would go for big numbers
        {
            OverSixtyPick();
            return;
        }

        if (tempScore >= 50)// always goes for bullseye
        {
            Adjust(bullseye.transform.position, offset);
            return;
        }

        if (tempScore > 20)// goes for random small
        {
            int temp = UnityEngine.Random.Range(4, 7);//from 4 to 6 so points from  15, 18, 21
            Adjust(c[temp].colliders[(int)PointValueTarget.Triple].target.position, offset);
            return;
        }

        if (tempScore <= 20)//goes for single to win
        {
            
            int temp = UnityEngine.Random.Range(0, 10);// random for whether we go for inner or outer to add to the presentation
            int target;
            if (temp > 4)
                target = (int)PointValueTarget.OuterSingle;
            else
                target = (int)PointValueTarget.InnerSingle;

            //must adjust -1 to get correct target
            Adjust(c[tempScore - 1].colliders[target].target.position, offset);
            return;
        }
    }

    /// <summary>
    /// called by DartScript for some reason
    /// </summary>
    public void AddPoints(int newPoints)
    {
        Audio.inst.PlayClip(hit);
        turnSum += newPoints;
        turnScore.text = turnSum.ToString();
        scores[numberOfDartsThrow].text = newPoints.ToString();
        dartimages[numberOfDartsThrow].enabled = false;
    }

    /// <summary>
    /// called by DartScript for some reason
    /// </summary>
    public void CheckForBust()
    {

        if (overall - turnSum < 0)
        {
            turnSum = 0;
            SwitchTurn();
            return;
        }

        if (overall - turnSum == 0)
        {
            for (int i = 0; i < 3; i++)
            {
                Dart[i].reset_position();
            }
            Win();
            return;
        }

      
        numberOfDartsThrow++;

        if (numberOfDartsThrow >= 3)
        {
            SwitchTurn();
            return;
        }
        else
           if (currentTurn % 2 == 0)
        {
            playerTurn();
            return;
        }
        else
        {
            partnerTurn();
            return;
        }
    }

    public IEnumerator WaitToEnd()
    {
        ControlTutuorialUI.inst.SetControl((int)Controls.DartsGame, false);
        yield return sec;
        
        winc.enabled = false;
        losec.enabled = false;
        board.enabled = false;
        PauseMenu.inst.SetEnabled(true);
        s.setTime(TimeBlocks.Short);
    }
}

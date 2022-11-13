using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using System;

public class DartGame : MonoBehaviour
{


    public short turnSum = 0;
    public short overall = 501;

    public byte currentTurn = 0;

    public int partnerIndex =0;

    public AimCone aim;

    public byte numberOfDartsThrow = 0;
    public byte maxTurns;
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

    [SerializeReference]
    public BoardSlice[] c;
    public BoardCollider bullseye;
    public Player stats;
    public Partner[] partners;
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
                for(int j = 0; j < 4; j++)
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

    public void Start()
    {
        Application.targetFrameRate = 60;
        BeginGame(0);
    }

    public void BeginGame(int partner)
    {
        float Accuracy = ((stats.Skill + stats.Luck) - (stats.Intoxication * 2)) / 100;
        float Stability = (30/stats.Skill) + ((stats.Intoxication/3) / 10);
        aim.driftSpeed = driftDefault * Stability;
        aim.moveSpeed = (1.35f -((stats.Intoxication / 5) / 10)) * aim.driftSpeed;
        float Synergy = (partners[partner].Love) + (stats.Charisma / 3);

        partnerIndex = partner;

        dartCanvas.enabled = true;
        numberOfDartsThrow = 0;
        currentTurn = 0;
        turnSum = 0;
        turnScore.text = turnSum.ToString();
        overallScore.text = overall.ToString();
        playerTurn();
    }

    public void lose()
    {
        Debug.Log("lose");
        dartCanvas.enabled = false;
    }

    public void win()
    {
        Debug.Log("win");
        dartCanvas.enabled = false;
    }

    public void switchTurn()
    {
        Debug.Log("swap");
        for( int i = 0; i < 3; i++)
        {
            dartimages[i].enabled = true;
            scores[i].text = "";
        }
        
        overall -= turnSum;
        turnSum = 0;
        turnScore.text = turnSum.ToString();
        overallScore.text = overall.ToString();

        
        numberOfDartsThrow = 0;
        currentTurn++;
        if (numberOfDartsThrow >+ maxTurns)
        {
            lose();
        }

        if (currentTurn % 2 == 0)
        {
            mr.material = green;
            playerTurn();
        }
        else
        {
            mr.material = red;
            partnerTurn();
        }    
    }

    private void playerTurn()
    {
       
        aim.begin();
    }

    private void DartC(Material m)
    {
        for( int i=0;i<3;i++)
            Dart[i].
    }

    private void partnerTurn()
    {
        Vector3 location;
        if (overall > 180)
        {
            int pick = UnityEngine.Random.Range(17, 19);

            int temp = 1;

            if (partners[partnerIndex].Composure >= 5)
            {
                temp = 0;
            }

            if (partners[partnerIndex].Composure >= 10)
            {
                temp = 2;
               
            }

            

            location = c[pick].colliders[temp].target.position;

            float offset = UnityEngine.Random.Range(((partners[partnerIndex].Intoxication / 5) / 10), (partners[partnerIndex].Intoxication / 5) / -10);

            location.x += offset;
            location.y += offset;

            aim.ghoot(location);
            return;


        }
        return;
    }

    public void AddPoints(byte b)
    {
        turnSum += b;
        turnScore.text = turnSum.ToString();
        scores[numberOfDartsThrow].text = b.ToString();
        dartimages[numberOfDartsThrow].enabled = false;
    }

    public void check(byte b)
    {
        Debug.Log("check");
        if (overall - turnSum < 0)
        {
            turnSum = 0;
            Debug.Log("BUST");
            switchTurn();
            return;
        }

        if (overall - turnSum == 0)
        {
            win();
            return;
        }

      
        numberOfDartsThrow++;

        if (numberOfDartsThrow >= 3)
        {
            switchTurn();
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
}

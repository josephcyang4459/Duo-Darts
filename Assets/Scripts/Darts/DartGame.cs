using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using System;

public class DartGame : MonoBehaviour
{


    public short turnSum = 0;
    public int overall = 501;
    public int points;
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

    public void BeginGame(int partner)
    {
        UI_Helper.SetSelectedUIElement(s.c.voiddd);
        board.enabled = true;
        DartC(green);
        s.ass.clip = ac;
        s.ass.Play();
        points = overall > 600 ? 10 : 5;
        aim.accuracy = (Mathf.Clamp((stats.Intoxication * 2) - (stats.Skill + stats.Luck),0,100)) / 10;
        //Debug.Log(Accuracy);
        float Stability = (30/stats.Skill) + ((stats.Intoxication/3) / 10);
        aim.driftSpeed = driftDefault * Stability;
        aim.moveSpeed = (1.35f -((stats.Intoxication / 5) / 10)) * aim.driftSpeed;

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

    public void lose()
    {
        //Debug.Log("lose");
        dartCanvas.enabled = false;
        losec.enabled = true;

        if (s.hour == 8)
            if (s.minutes > 50)
                UnityEngine.SceneManagement.SceneManager.LoadScene(0);

        StartCoroutine(condition());
    }

    public void win()
    {
        //Debug.Log("win");
        dartCanvas.enabled = false;
        stats.points += points;
        winc.enabled = true;
        if (s.hour == 8)
            if (s.minutes > 50)
                UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        StartCoroutine(condition());
    }

    public void switchTurn()
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
            lose();
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

        int tempSc = overall - turnSum;
        if (tempSc >= 60)
        {

            if (partners[partnerIndex].bias > -1)
            {
                if(partners[partnerIndex].bias == 0)
                {

                    Adjust(bullseye.transform.position, offset);
                    return;
                }

                if(partners[partnerIndex].bias == 1)
                {
                    Adjust(c[19].colliders[2].target.position, offset);
                    return;
                }
            }

            int pick = UnityEngine.Random.Range(17, 19);

            int temp = 1;

            if (partners[partnerIndex].Composure >= 5)
            {
                temp = 0;
            }

            if (partners[partnerIndex].Composure >= 10)
            {
                int trye  = UnityEngine.Random.Range(0, 10);

                if (trye >7)
                {
                    Adjust(c[19].colliders[2].target.position, offset);
                    return;
                }

                if (trye > 4)
                {
                    Adjust(bullseye.transform.position, offset);
                    return;
                }

                temp = 2;
               
            }



            Adjust(c[pick].colliders[temp].target.position, offset);

            return;
        }

        if (tempSc >= 50)
        {
            //Debug.Log("50");
            Adjust(bullseye.transform.position, offset);
            return;
        }

        if (tempSc > 20)
        {
            //Debug.Log("30");
            Adjust(c[6].colliders[2].target.position, offset);
            return;
        }

        if (tempSc <= 20)
        {
            //Debug.Log("20");
            Adjust(c[tempSc - 1].colliders[1].target.position, offset);
            return;
        }




        return;
    }

    public void AddPoints(byte b)
    {
        s.ass.PlayOneShot(hit);
           turnSum += b;
        turnScore.text = turnSum.ToString();
        scores[numberOfDartsThrow].text = b.ToString();
        dartimages[numberOfDartsThrow].enabled = false;
    }

    public void check(byte b)
    {

        if (overall - turnSum < 0)
        {
            turnSum = 0;
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

    public IEnumerator condition()
    {

        yield return sec;
        
        winc.enabled = false;
        losec.enabled = false;
        board.enabled = false;
        s.setTime(5);
    }
}

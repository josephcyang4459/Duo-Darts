using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class DartGame : MonoBehaviour
{


    public short turnSum = 0;
    public short overall = 501;

    public byte currentTurn = 0;

    public AimCone aim;

    public byte numberOfDartsThrow = 0;
    public byte maxTurns;
    public TMP_Text overallScore;
    public TMP_Text turnScore;
    public WaitForSeconds k = new WaitForSeconds(3);

    public Material flash;
    public Material gone;
    public DartScript Dart;
    public Image[] dartimages;
    public TMP_Text[] scores;
    public Canvas dartCanvas;
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
            }
            reset = false;
        }
    }

#endif

    public void Start()
    {
        
        BeginGame();
    }

    public void BeginGame()
    {
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
            playerTurn();
        }
        else
        {
            //ai
            partnerTurn();
        }    
    }

    private void playerTurn()
    {
        aim.begin();
    }

    private void partnerTurn()
    {
        playerTurn();
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

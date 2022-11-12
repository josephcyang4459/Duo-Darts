using UnityEngine;
using System.Collections;
using TMPro;

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
    public WaitForSeconds k;

    public Material flash;
    public Material gone;
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
                    obj.transform.GetChild(j).GetComponent<BoardCollider>().g = this;
                }
            }
            reset = false;
        }
    }

#endif

    public void Start()
    {
        k = new WaitForSeconds(3);
        BeginGame();
    }

    public void BeginGame()
    {
        numberOfDartsThrow = 0;
        currentTurn = 0;
        turnSum = 0;
        turnScore.text = turnSum.ToString();
        overallScore.text = overall.ToString();
        playerTurn();
    }

    public void lose()
    {

    }

    public void win()
    {

    }

    public void switchTurn()
    {
        Debug.Log("swap");
        turnSum = 0;
        overall -= turnSum;
        turnScore.text = turnSum.ToString();
        overallScore.tag = overall.ToString();
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

    public void gainPoints(byte b)
    {
        Debug.Log(b);
        turnSum -= b;
        turnScore.text = turnSum.ToString();
        if (overall - turnSum < 0)
        {
            Debug.Log("BUST");
            switchTurn();

        }

        if(overall - turnSum == 0)
        {
            win();
        }

        numberOfDartsThrow++;
        StartCoroutine(wait());
    }

    public IEnumerator wait()
    {
        
        yield return k;
        if (numberOfDartsThrow > 3)
        {
            switchTurn();
        }
        else
           if (currentTurn % 2 == 0)
        {
            playerTurn();
        }
        else
            partnerTurn();
    }
}

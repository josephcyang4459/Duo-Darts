using UnityEngine;

public class DartGame : MonoBehaviour
{


    public short turnSum = 0;
    public short overall = 501;

    public byte currentTurn = 0;

    public AimCone aim;

    public byte number = 0;

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

    public void BeginGame()
    {

        playerTurn();
    }

    public void switchTurn()
    {
        number = 0;
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

    }

    public void gainPoints(byte b)
    {
        Debug.Log(b);
        number++;
        if (number > 3)
        {
            switchTurn();
        }
            
    }
}

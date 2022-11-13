using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class CutsceneHandler : MonoBehaviour
{
    public CutScene cs;
    public Dialogue dh;

    public int index;

    public int responseIndex = 0;
    public int responseIndexIndex = 0;
    public InputActionReference interact;
    public Response respon;
    public bool responding;
    public Canvas dialougeCanvas;
    public Canvas responseCanvas;
    public TMP_Text[] responses;
    public byte characterIndex;

    public Partner[] partners;
    public Image character;
    public TMP_Text characterName;
    public Sprite[] bgs;

    public Image cutSceneBackGround;

    public string loseScene;

    public Schedule sc;

    public void Start()
    {
        
        //tart(cs, 0);
    }

    public void tart(CutScene c, byte b)
    {
        

        decideChar(c.defaultCharacter);
        background(b);
        cs = c;

        dialougeCanvas.enabled = true;
        interact.action.Enable();
        interact.action.performed += takeAction;
        cs.blocks[index].action(this);
    }

    public void off()
    {
        dialougeCanvas.enabled = false;
        responseCanvas.enabled = false;
        interact.action.Disable();
        interact.action.performed -= takeAction;
        sc.setTime(10);
    }

    public void takeAction(InputAction.CallbackContext c)
    {
        if (responding)
        {
            if (responseIndex < 0)
                return;
            if (dh.Script.typer != null)
            {
                dh.Script.Stop();
                return;
            }
            dialouge(respon.responses[responseIndex].responses[responseIndexIndex]);
            responseIndexIndex++;
            if(responseIndexIndex>= respon.responses[responseIndex].responses.Length)
            {
                if (respon.responses[responseIndex].adjust < 0)
                {
                    off();
                    
                    return;
                }

                partners[characterIndex].Love += respon.responses[responseIndex].adjust;

                responseIndexIndex = 0;
                responseIndex = -1;
                responding = false;
               

                
            }
           
        }
            
        if (dh.Script.typer != null)
        {
            dh.Script.Stop();
            return;
        }
        index++;

        if(index>= cs.blocks.Length)
        {
            off();
            return;
        }

        cs.blocks[index].action(this);
    }

    public void dialouge(string message)
    {
        dh.WriteDialogue(message);
    }

    public void response(Response r)
    {
        respon = r;
        responding = true;
        responseIndex = -1;
        for (int i = 0; i < 3; i++)
            responses[i].text = respon.responses[i].answer;

        responseCanvas.enabled = true;

    }

    public void changeChar(string character)
    {
        decideChar(character);
        index++;

        cs.blocks[index].action(this);
    }

    public void UI_Response(int i)
    {
        responseCanvas.enabled = false;
        responseIndex = i;
        dialouge(respon.responses[responseIndex].responses[responseIndexIndex]);
    }

    public void changeExpression(string b)
    {

    }

    public void changeBackground(string s)
    {
        decideBackGround(s);
    }

    private void partner(int i)
    {
        characterIndex = (byte)i;
        character.sprite = partners[i].Expressions[0];
        characterName.text = partners[i].Name;
        characterName.font = partners[i].Font;
        dh.textLabel.font = partners[i].Font;

    }

    private void background(int i)
    {
        cutSceneBackGround.sprite = bgs[i];
    }

    private void decideBackGround(string s)
    {
        switch (s.ToLower())
        {
            case "lounge":
                background(0);
                break;
            case "bar":
                background(1);
                break;
            case "dance":
                background(2);
                break;
            case "elaine":
                background(0);

                break;

            case "owner":
                background(0);
                break;
            default:
                characterName.text = s;
                break;
        }
    }

    private void decideChar(string s)
    {
        switch (s.ToLower())
        {
            case "faye":
                partner(2);
                break;
            case "chad":
                partner(0);
                break;
            case "jess":
                partner(1);
                break;
            case "elaine":
                partner(3);

                break;

            case "owner":
                partner(4);

                break;
            default:
                partner(5);
                characterName.text = s;
                break;
        }


    }
}

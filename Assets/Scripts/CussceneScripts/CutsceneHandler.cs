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

    public void Start()
    {
        tart(cs);
    }

    public void tart(CutScene c)
    {
        change(c.defaultCharacter);
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
                responseIndexIndex = 0;
                responseIndex = -1;
                responding = false;
            }
           
        }
            
        Debug.Log("gg");
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
        //Debug.Log(message);
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
        Debug.Log("change");
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

    private void partner(int i)
    {
        character.sprite = partners[i].Expressions[0];
        characterName.text = partners[i].Name;
        characterName.font = partners[i].Font;
        dh.textLabel.font = partners[i].Font;

    }
    private void change(string s)
    {
        switch (s.ToLower())
        {
            case "faye":
                partner(0);
                break;
            case "chad":
                partner(0);
                break;
            case "jess":
                partner(0);
                break;
            case "elaine":
                partner(0);

                break;

            case "owner":
                partner(0);

                break;
            default:
                characterName.text = s;
                break;
        }


    }
}

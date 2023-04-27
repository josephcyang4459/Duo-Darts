using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class CutsceneHandler : MonoBehaviour
{
    public CutScene cutscene;
    public Dialogue dh;

    public int index;
    public Player p;

    public int responseIndex = 0;
    public int responseIndexIndex = 0;
    public InputActionReference interact;
    public Response respon;
    public bool responding;
    public Canvas dialougeCanvas;
    public Canvas responseCanvas;
    public TMP_Text[] responses;
    public int characterIndex;
    public DartMen dm;

    public Partner[] partners;
    public Image character;
    public TMP_Text characterName;
    public Sprite[] bgs;

    public Image cutSceneBackGround;

    public Schedule sc;

    public Image text;
    public Image textLine;

    public GameObject responseButton;
    public AttributeUpdate au;

    public Canvas ac;
    public GameObject bsss;
    public GameObject voiddd;

    public void tart(CutScene c, byte b)
    {
        index = 0;
        cutscene = c;
        bbbbb();
        decideChar(c.defaultCharacter);
        background(b);
        if (c.AnotherFuckingException)
        {
            UI_Helper.SetSelectedUIElement(bsss);
            ac.enabled = true;
            return;
        }

        UI_Helper.SetSelectedUIElement(voiddd);

        dialougeCanvas.enabled = true;
        interact.action.Enable();
        interact.action.performed += takeAction;
        cutscene.blocks[index].action(this);
    }
    void bbbbb()
    {
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < partners[i].RelatedCutScenes.Length; j++)
                if (partners[i].RelatedCutScenes[j].CutScene == cutscene)
                {
                    partners[i].RelatedCutScenes[j].completed = true;
                    return;
                }
        }
    }

    public void choice(int i)
    {
        if (i == 0)
            cutscene = partners[characterIndex].DefaultCutScene;
        else
            cutscene = partners[characterIndex].DefaultDrinkingCutScene;

        sc.enabled = false;
        dialougeCanvas.enabled = true;
        interact.action.Enable();
        interact.action.performed += takeAction;
        UI_Helper.SetSelectedUIElement(voiddd);
        cutscene.blocks[index].action(this);
        
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
        if (responding)// resnponding to quesation
        {
            if (responseIndex < 0)
                return;

            if (dh.Script.typer != null)
            {
                dh.Script.Stop();
                return;
            }

            responseIndexIndex++;

            if (responseIndexIndex>= respon.responses[responseIndex].responses.Length)
            {
                if (respon.responses[responseIndex].adjust < 0)
                {
                    partners[characterIndex].Love -= 99999;
                    off();
                    return;
                }

                partners[characterIndex].Love += respon.responses[responseIndex].adjust;

                responseIndexIndex = 0;
                responseIndex = -1;
                responding = false;
            }
            else
            {
                if (!respon.responses[responseIndex].exemption)
                    dialouge(respon.responses[responseIndex].responses[responseIndexIndex]);
                else
                    Thought(respon.responses[responseIndex].responses[responseIndexIndex]);
            }
            return;
        }
            
        if (dh.Script.typer != null)
        {
            dh.Script.Stop();
            return;
        }
        index++;

        if(index>= cutscene.blocks.Length)
        {

            off();
            if (cutscene.exception)// force play darts
                dm.exception(characterIndex, sc.hour);

            for (int i = 0; i < cutscene.partnerS.stats.Length; i++)
            {
                partners[characterIndex].stateChange(cutscene.partnerS.stats[i], cutscene.partnerS.values[i]);
            }

            for (int i = 0; i < cutscene.playerS.stats.Length;i++)
            {
                au.UpdateAttribute(cutscene.playerS.stats[i], cutscene.playerS.values[i]);
            }
            return;
        }

        cutscene.blocks[index].action(this);
    }

    public void dialouge(string message)
    {
        textLine.enabled = true;
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
        UI_Helper.SetSelectedUIElement(responseButton);
    }

    public void changeChar(string character)
    {
        decideChar(character);
        index++;

        cutscene.blocks[index].action(this);
    }

    public void UI_Response(int i)
    {
        UI_Helper.SetSelectedUIElement(voiddd);
        responseCanvas.enabled = false;
        responseIndex = i;
        responseIndexIndex = 0;

        if (!respon.responses[responseIndex].exemption)
            dialouge(respon.responses[responseIndex].responses[responseIndexIndex]);
        else
            Thought(respon.responses[responseIndex].responses[responseIndexIndex]);
    }

    public void changeExpression(string b)
    {
        int getExpression(string b)
        {
            switch (b.ToLower())
            {
                case "lounge":
                    return 0;
                case "bar":
                    return 1;
                case "dance":
                    return 2;
                case "elaine":
                    return 3;
            }
#if UNITY_EDITOR
            Debug.Log("COULD NOT FIND EXPRESSION " + b);
#endif
            return 0;
        }
        character.sprite = partners[characterIndex].Expressions[getExpression(b)];
    }

    public void Thought(string s)
    {
        textLine.enabled = false;
        dh.WriteDialogue(s);
    }

    public void changeBackground(string s)
    {
        decideBackGround(s);
    }

    private void partner(int i)
    {
        characterIndex = i;
        character.sprite = partners[i].Expressions[0];
        characterName.text = partners[i].Name;
        characterName.font = partners[i].Font;
        characterName.fontSize = partners[i].textSize;
        dh.textLabel.font = partners[i].Font;

        text.sprite = partners[i].TextBox;
        textLine.sprite = partners[i].textLineTHing;

        dh.textLabel.fontSize = partners[i].textSize;
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

            case "bar guy":
                partner(5);
                break;
            case "charming girl":
                partner(6);
                break;
            case "charming guy":
                partner(7);
                break;
            case "dance girl":
                partner(8);
                break;
            case "lounge guy":
                partner(9);
                break;

            default:
#if UNITY_EDITOR
                Debug.Log("UNLESS THIS IS BATHROOM WALL SOMETHING WENT WRONG");
#endif
                partner(10);
                characterName.text = s;
                break;
        }


    }
}

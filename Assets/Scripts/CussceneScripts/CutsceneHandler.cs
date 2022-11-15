using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class CutsceneHandler : MonoBehaviour
{
    public CutScene cs;
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
    public byte characterIndex;
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
        cs = c;
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
        cs.blocks[index].action(this);
    }
    void bbbbb()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < partners[i].RelatedCutScenes.Length; j++)
                if (partners[i].RelatedCutScenes[j].CutScene == cs)
                {
                    partners[i].RelatedCutScenes[j].completed = true;
                    return;
                }
        }
    }

    public void choice(int i)
    {
        if (i == 0)
            cs = partners[characterIndex].DefaultCutScene;
        else
            cs = partners[characterIndex].DefaultDrinkingCutScene;

        sc.enabled = false;
        dialougeCanvas.enabled = true;
        interact.action.Enable();
        interact.action.performed += takeAction;
        UI_Helper.SetSelectedUIElement(voiddd);
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

            if (!respon.responses[responseIndex].exemption)
                dialouge(respon.responses[responseIndex].responses[responseIndexIndex]);
            else
                Thought(respon.responses[responseIndex].responses[responseIndexIndex]);

            responseIndexIndex++;

            if(responseIndexIndex>= respon.responses[responseIndex].responses.Length)
            {
                if (respon.responses[responseIndex].adjust < 0)
                {
                    off();
                    partners[characterIndex].Love -= 99999;


                    return;
                }

                partners[characterIndex].Love += respon.responses[responseIndex].adjust;

                responseIndexIndex = 0;
                responseIndex = -1;
                responding = false;

                
                
            }
            return;
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
            if (cs.exception)
                dm.exception(characterIndex, sc.hour);

            for (int i = 0; i < cs.partnerS.stats.Length; i++)
            {
                partners[characterIndex].stateChange(cs.partnerS.stats[i], cs.partnerS.values[i]);
            }

            for (int i = 0; i < cs.playerS.stats.Length;i++)
            {
                au.UpdateAttribute(cs.playerS.stats[i], cs.playerS.values[i]);
            }
            return;
        }

        cs.blocks[index].action(this);
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

        cs.blocks[index].action(this);
    }

    public void UI_Response(int i)
    {
        UI_Helper.SetSelectedUIElement(voiddd);
        responseCanvas.enabled = false;
        responseIndex = i;
        if (!respon.responses[responseIndex].exemption)
            dialouge(respon.responses[responseIndex].responses[responseIndexIndex]);
        else
            Thought(respon.responses[responseIndex].responses[responseIndexIndex]);
    }

    public void changeExpression(string b)
    {

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
        characterIndex = (byte)i;
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
            default:
                partner(5);
                characterName.text = s;
                break;
        }


    }
}

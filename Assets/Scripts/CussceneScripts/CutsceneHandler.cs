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

    public Canvas DefaultCanvass;
    public GameObject bsss;
    public GameObject voiddd;

    public void PlayCutScene(CutScene c, int BackgroundIndex)
    {
        PauseMenu.inst.SetEnabled(false);
        ControlTutuorialUI.inst.SetControl((int)Controls.Cutscene, true);
        index = 0;
        cutscene = c;
        CompleteThisCutscene();
        DecideCharacter(c.defaultCharacter);
        background(BackgroundIndex);

        dialougeCanvas.enabled = true;
        interact.action.Enable();
        interact.action.performed += takeAction;
        cutscene.blocks[index].action(this);
    }
    void CompleteThisCutscene()
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
        index = 0;
        DefaultCanvass.enabled = false;
        dialougeCanvas.enabled = true;
        interact.action.Enable();
        interact.action.performed += takeAction;
        cutscene.blocks[index].action(this);
    }

    public void off()
    {
        PauseMenu.inst.SetEnabled(true);
        ControlTutuorialUI.inst.SetControl((int)Controls.Cutscene, false);
        dialougeCanvas.enabled = false;
        responseCanvas.enabled = false;
        interact.action.Disable();
        interact.action.performed -= takeAction;
        sc.setTime(10);
    }

    public void PresentChoices()
    {
        DefaultCanvass.enabled = true;
        dialougeCanvas.enabled = false;
        dialougeCanvas.enabled = false;
        interact.action.Disable();
        interact.action.performed += takeAction;
    }


    public void takeAction(InputAction.CallbackContext c)
    {
        Debug.Log(index);
        if (responding)// resnponding to quesation
        {
            if (responseIndex < 0)
                return;

            if (dh.Script.writing)
            {
                dh.Script.Stop();
                return;
            }

            responseIndexIndex++;

            if (responseIndexIndex >= respon.responses[responseIndex].responses.Length)
            {
                responseIndexIndex = 0;
                responseIndex = -1;
                responding = false;
                //Debug.Log("Done resonding");
                dh.Script.Stop();
                nextBlock();
            }
            else
            {
                displayResponse();
            }
            return;
        }
            
        if (dh.Script.writing)
        {
            dh.Script.Stop();
            return;
        }

        nextBlock();
    }

    public void nextBlock()
    {
        index++;

        if (index >= cutscene.blocks.Length)
        {

            off();
            if (cutscene.exception)// force play darts
                dm.exception(characterIndex, sc.hour);
            /*

            for (int i = 0; i < cutscene.partnerS.stats.Length; i++)
            {
                partners[characterIndex].stateChange(cutscene.partnerS.stats[i], cutscene.partnerS.values[i]);
            }

            for (int i = 0; i < cutscene.playerS.stats.Length; i++)
            {
                au.UpdateAttribute(cutscene.playerS.stats[i], cutscene.playerS.values[i]);
            }
            
            */
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
        //UI_Helper.SetSelectedUIElement(responseButton);
    }

    public void changeChar(string character)
    {
        DecideCharacter(character);
        nextBlock();
    }

    public void UI_Response(int i)
    {
        //UI_Helper.SetSelectedUIElement(voiddd);
        responseCanvas.enabled = false;
        responseIndex = i;
        responseIndexIndex = 0;

        displayResponse();
    }

    private void displayResponse()
    {
        respon.responses[responseIndex].responses[responseIndexIndex].Adjust(this);
        if (!respon.responses[responseIndex].responses[responseIndexIndex].exemption)
            dialouge(respon.responses[responseIndex].responses[responseIndexIndex].Message);
        else
            Thought(respon.responses[responseIndex].responses[responseIndexIndex].Message);
    }

    public void changeExpression(int ExpressionIndex)
    {
        if (character.enabled == false)
            character.enabled = true;
        character.sprite = partners[characterIndex].Expressions[ExpressionIndex];
        nextBlock();
    }

    public void Thought(string s)
    {
        textLine.enabled = false;
        dh.WriteDialogue(s);
    }

    public void changeBackground(string s)
    {
        decideBackGround(s);
        nextBlock();
    }

    private void partner(int i)
    {
        characterIndex = i;

        character.sprite = partners[i].Expressions[0];
        character.enabled = true;
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

    private void DecideCharacter(string s)
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

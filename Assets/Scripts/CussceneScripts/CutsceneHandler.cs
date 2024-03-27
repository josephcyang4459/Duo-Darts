using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class CutsceneHandler : MonoBehaviour
{
    public static CutsceneHandler inst;
    [SerializeField] CutScene cutscene;
    [SerializeField] Dialogue dh;
    [SerializeField] Player p;
    [SerializeField] public CharacterList characters;
    [SerializeField] InputActionReference interact;
    [Space]
    [SerializeField] Canvas dialougeCanvas;
    [SerializeField] Image characterImg;
    [SerializeField] TMP_Text characterName;
    [SerializeField] Image TextBox;
    [SerializeField] Image TextLine;
    [SerializeField] Canvas responseCanvas;
    [SerializeField] Image ResponsePortrait;
    [SerializeField] GameObject responseButton;
    [SerializeField] TMP_Text[] responses;
    [SerializeField] Canvas DefaultCanvass;
    [SerializeField] Image cutSceneBackGround;
    [SerializeField] Sprite[] bgs;
    [SerializeField] Sprite[] CharacterPortraits;
    [Space]
    [SerializeField] int characterIndex;
    [SerializeField] int index;
    [SerializeField] Response respon;
    [SerializeField] bool responding;
    [SerializeField] int responseIndex = 0;
    [SerializeField] int responseIndexIndex = 0;

    public DartMen DartsMenu;
    public Schedule Schedule;

    public void Awake()
    {
        if (inst != null)
        {
            DestroyImmediate(this);
            return;
        }
        inst = this;
        DontDestroyOnLoad(this);
    }

    public void SetCharacterSprite(int i)
    {
        ResponsePortrait.sprite = CharacterPortraits[i];
    }

    public void SetUpForMainGame(DartMen dartsMenu,Schedule schedule)
    {
        DartsMenu = dartsMenu;
        Schedule = schedule;
    }

    public void PlayCutScene(CutScene c, int BackgroundIndex)
    {
        PauseMenu.inst.SetEnabled(false);
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
            for (int j = 0; j < characters.list[i].RelatedCutScenes.Length; j++)
                if (characters.list[i].RelatedCutScenes[j].CutScene == cutscene)
                {
                    characters.list[i].RelatedCutScenes[j].completed = true;
                    return;
                }
        }
    }

    public void choice(int i)
    {
        if (i == 0)
            cutscene = characters.list[characterIndex].DefaultRepeatingScene;
        else
            cutscene = characters.list[characterIndex].DefaultDrinkingCutScene;

        Schedule.enabled = false;
        index = 0;
        DefaultCanvass.enabled = false;
        dialougeCanvas.enabled = true;
        interact.action.Enable();
        interact.action.performed += takeAction;
        cutscene.blocks[index].action(this);
    }

    public void EndCutscene()
    {
        PauseMenu.inst.SetEnabled(true);
        dialougeCanvas.enabled = false;
        responseCanvas.enabled = false;
        interact.action.Disable();
        interact.action.performed -= takeAction;
        Schedule!.setTime(TimeBlocks.Long);
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

            EndCutscene();
            if (cutscene.exception)// force play darts
                DartsMenu.exception(characterIndex, Schedule.hour);
            return;
        }

        cutscene.blocks[index].action(this);
    }

    public void dialouge(string message)
    {
        TextLine.enabled = true;
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
        if (characterImg.enabled == false)
            characterImg.enabled = true;
        characterImg.sprite = characters.list[characterIndex].Expressions[ExpressionIndex];
        nextBlock();
    }

    public void Thought(string s)
    {
        TextLine.enabled = false;
        dh.WriteDialogue(s);
    }

    public void changeBackground(string s)
    {
        decideBackGround(s);
        nextBlock();
    }

    private void character(int i)
    {
        characterIndex = i;

        characterImg.sprite = characters.list[i].Expressions[0];
        characterImg.enabled = true;
        characterName.text = characters.list[i].Name;
        characterName.font = characters.list[i].Font;
        characterName.fontSize = characters.list[i].textSize;

        TextBox.sprite = characters.list[i].TextBox;
        TextLine.sprite = characters.list[i].textLineTHing;
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
            case "chad":
                character(0);
                break;
            case "jess":
                character(1);
                break;
            case "faye":
                character(2);
                break;
            case "elaine":
                character(3);
                break;
            case "owner":
                character(4);
                break;
            case "bar guy":
                character(5);
                break;
            case "charming girl":
                character(6);
                break;
            case "charming guy":
                character(7);
                break;
            case "dance girl":
                character(8);
                break;
            case "lounge guy":
                character(9);
                break;
            default:
#if UNITY_EDITOR
                Debug.Log("UNLESS THIS IS BATHROOM WALL SOMETHING WENT WRONG");
#endif
                character(10);
                characterName.text = s;
                break;
        }
    }
}

using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class CutsceneHandler : MonoBehaviour {
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

    public void Awake() {
        if (inst != null) {
            Destroy(gameObject);
            return;
        }
        inst = this;
        DontDestroyOnLoad(this);
    }

    public void PlayClickSound() {
        Audio.inst.PlayClip(AudioClips.Click);
    }

    public void SetCharacterSprite(int i) {
        ResponsePortrait.sprite = CharacterPortraits[i];
    }

    public void SetUpForMainGame(DartMen dartsMenu, Schedule schedule) {
        DartsMenu = dartsMenu;
        Schedule = schedule;
    }

    public void PlayCutScene(CutScene c, int BackgroundIndex) {
        PauseMenu.inst.SetEnabled(false);
        UIState.inst.SetInteractable(false);
        index = 0;
        cutscene = c;
        CompleteThisCutscene();
        DecideCharacter(c.defaultCharacter, false);
        SetBackGroundVisual(BackgroundIndex);

        dialougeCanvas.enabled = true;
        interact.action.Enable();
        interact.action.performed += takeAction;
        cutscene.blocks[index].action(this);
    }

    void CompleteThisCutscene() {
        for (int i = 0; i < 5; i++) {
            for (int j = 0; j < characters.list[i].RelatedCutScenes.Length; j++)
                if (characters.list[i].RelatedCutScenes[j].CutScene == cutscene) {
                    characters.list[i].RelatedCutScenes[j].completed = true;
                    return;
                }
        }
    }

    public void choice(int i) {
        if (i == 0)
            cutscene = characters.list[characterIndex].DefaultRepeatingScene;
        else
            cutscene = characters.list[characterIndex].DefaultDrinkingCutScene;
        UIState.inst.SetInteractable(false);
        Schedule.enabled = false;
        index = 0;
        DefaultCanvass.enabled = false;
        dialougeCanvas.enabled = true;
        interact.action.Enable();
        interact.action.performed += takeAction;
        cutscene.blocks[index].action(this);
    }

    public void EndCutscene() {
        PauseMenu.inst.SetEnabled(true);
        UIState.inst.SetInteractable(true);
        dialougeCanvas.enabled = false;
        responseCanvas.enabled = false;
        interact.action.Disable();
        interact.action.performed -= takeAction;
        Schedule!.setTime(TimeBlocks.Long);
    }

    public void PresentChoices() {
        UIState.inst.SetInteractable(true);
        //-----------------------------------------------------------------------------------------******-----+++++++------------Set BUTTON HERE
        DefaultCanvass.enabled = true;
        dialougeCanvas.enabled = false;
        dialougeCanvas.enabled = false;
        interact.action.Disable();
        interact.action.performed += takeAction;
    }


    public void takeAction(InputAction.CallbackContext c) {
        if (responding)// resnponding to quesation
        {
            if (responseIndex < 0)
                return;

            if (dh.Script.writing) {
                dh.Script.Stop();
                return;
            }

            responseIndexIndex++;

            if (responseIndexIndex >= respon.responses[responseIndex].responses.Length) {
                responseIndexIndex = 0;
                responseIndex = -1;
                responding = false;
                //Debug.Log("Done resonding");
                dh.Script.Stop();
                nextBlock();
            }
            else {
                displayResponse();
            }
            return;
        }

        if (dh.Script.writing) {
            dh.Script.Stop();
            return;
        }

        nextBlock();
    }

    public void nextBlock() {
        index++;

        if (index >= cutscene.blocks.Length) {

            EndCutscene();
            if (cutscene.ForceDarts)// force play darts
                DartsMenu.exception(characterIndex, Schedule.hour);
            return;
        }

        cutscene.blocks[index].action(this);
    }

    public void dialouge(string message) {
        TextLine.enabled = true;
        dh.WriteDialogue(message);
    }

    public void response(Response r) {
        //-------------------------**********------------------------------+++++++++++++--------------------------------------Set BUTTON HERE
        UIState.inst.SetInteractable(true);
        respon = r;
        responding = true;
        responseIndex = -1;
        for (int i = 0; i < 3; i++)
            responses[i].text = respon.responses[i].answer;

        responseCanvas.enabled = true;
    }

    public void ChangeCharacter(string character, Expressions expression) {
        DecideCharacter(character);
        ChangeExpression((int)expression);
    }

    public void UI_Response(int i) {
        UIState.inst.SetInteractable(false);
        responseCanvas.enabled = false;
        responseIndex = i;
        responseIndexIndex = 0;

        displayResponse();
    }

    private void displayResponse() {
        respon.responses[responseIndex].responses[responseIndexIndex].Adjust(this);
        if (!respon.responses[responseIndex].responses[responseIndexIndex].ResponseIsPlayerThought)
            dialouge(respon.responses[responseIndex].responses[responseIndexIndex].Message);
        else
            Thought(respon.responses[responseIndex].responses[responseIndexIndex].Message);
    }

    public void ChangeExpression(int ExpressionIndex, bool GoToNextBlock =true) {
        if (characterImg.enabled == false)
            characterImg.enabled = true;
        characterImg.sprite = characters.list[characterIndex].Expressions[ExpressionIndex];
        if (GoToNextBlock)
            nextBlock();
    }

    public void Thought(string s) {
        TextLine.enabled = false;
        dh.WriteDialogue(s);
    }

    public void changeBackground(string s) {
        decideBackGround(s);
        nextBlock();
    }

    private void SetPartnerVisual(int i, bool showPartnerVisual) {
        characterIndex = i;

        characterImg.sprite = characters.list[i].Expressions[0];
        characterImg.enabled = showPartnerVisual;
        characterName.text = characters.list[i].Name;
        characterName.font = characters.list[i].Font;
        characterName.fontSize = characters.list[i].textSize;

        TextBox.sprite = characters.list[i].TextBox;
        TextLine.sprite = characters.list[i].textLineTHing;
    }

    private void SetBackGroundVisual(int i) {
        cutSceneBackGround.sprite = bgs[i];
    }

    private void decideBackGround(string s) {
        switch (s.ToLower()) {
            case "lounge":
                SetBackGroundVisual(0);
                break;
            case "bar":
                SetBackGroundVisual(1);
                break;
            case "dance":
                SetBackGroundVisual(2);
                break;
            case "elaine":
                SetBackGroundVisual(0);
                break;
        }
    }

    private void DecideCharacter(string s, bool showPartner =true) {
        switch (s.ToLower()) {
            case "faye":
                SetPartnerVisual(2, showPartner);
                break;
            case "chad":
                SetPartnerVisual(0, showPartner);
                break;
            case "jess":
                SetPartnerVisual(1, showPartner);
                break;
            case "elaine":
                SetPartnerVisual(3, showPartner);
                break;
            case "owner":
                SetPartnerVisual(4, showPartner);
                break;
            case "bar guy":
                SetPartnerVisual(5, showPartner);
                break;
            case "charming girl":
                SetPartnerVisual(6, showPartner);
                break;
            case "charming guy":
                SetPartnerVisual(7, showPartner);
                break;
            case "dance girl":
                SetPartnerVisual(8, showPartner);
                break;
            case "lounge guy":
                SetPartnerVisual(9, showPartner);
                break;
            default:
#if UNITY_EDITOR
                Debug.Log("UNLESS THIS IS BATHROOM WALL SOMETHING WENT WRONG");
#endif
                SetPartnerVisual(10, showPartner);
                characterName.text = s;
                break;
        }
    }
}

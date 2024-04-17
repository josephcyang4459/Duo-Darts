using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CutsceneHandler : MonoBehaviour {
    public static CutsceneHandler inst;
    [SerializeField] CutScene cutscene;
    [SerializeField] Dialogue dh;
    [SerializeField] Player p;
    public CharacterList characters;
    [SerializeField] InputActionReference interact;
    [Space]
    [SerializeField] CharacterPortraitAnimation PortraitAnimation;
    [SerializeField] Canvas DialougeCanvas;
    [SerializeField] Image CharacterPortrait;
    [SerializeField] TMP_Text CharacterName;
    [SerializeField] GameObject CharacterNamePlate;
    [SerializeField] DialougeBox DialougeBox;
    [SerializeField] Canvas responseCanvas;
    [SerializeField] Image ResponsePortrait;
    [SerializeField] GameObject responseButton;
    [SerializeField] TMP_Text[] responses;
    [SerializeField] Canvas DefaultCanvass;
    public Image cutSceneBackGround;
    [SerializeField] SpriteCollection BackgroundSprites;
    [SerializeField] Sprite[] CharacterPortraits;
    [Space]
    [SerializeField] int CurrentCharacterIndex;
    [SerializeField] int index;
    [SerializeField] Response respon;
    [SerializeField] bool responding;
    [SerializeField] int responseIndex = 0;
    [SerializeField] int responseIndexIndex = 0;
    public bool InCutscene;
    [SerializeField] DartPartnerStoryUI DartsMenu;
    [SerializeField] Schedule Schedule;
    [SerializeField] EndingScene EndingScene;

    public void Awake() {
        if (inst != null) {
            Destroy(gameObject);
            return;
        }
        inst = this;
        DontDestroyOnLoad(this);
    }

    public int PlayerPortriatIndex() {
        return ResponsePortrait.sprite == CharacterPortraits[0] ? 0 : 1;
    }

    public void PlayClickSound() {
        Audio.inst.PlayClip(AudioClips.Click);
    }

    public void SetCharacterSprite(int i) {
        ResponsePortrait.sprite = CharacterPortraits[i];
    }

    public void SetUpForMainGame(DartPartnerStoryUI dartsMenu, Schedule schedule) {
        DartsMenu = dartsMenu;
        Schedule = schedule;
    }

    public void SetUpForEnding(EndingScene endingScene) {
        EndingScene = endingScene;
    }

    public void PlayCutScene(CutScene c, int BackgroundIndex) {
        InCutscene = true;
        //PauseMenu.inst.SetEnabled(false);
        UIState.inst.SetInteractable(false);
        DartSticker.inst.SetVisible(false);
        index = 0;
        cutscene = c;
        CompleteThisCutscene();
        DialougeBox.HideDialougeBox();
        DecideCharacter(c.defaultCharacter, false);
        SetBackGroundVisual(BackgroundIndex);

        DialougeCanvas.enabled = true;
        interact.action.Enable();
        interact.action.performed += TakeAction;
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
            cutscene = characters.list[CurrentCharacterIndex].DefaultRepeatingScene;
        else
            cutscene = characters.list[CurrentCharacterIndex].DefaultDrinkingCutScene;
        UIState.inst.SetInteractable(false);
        Schedule.enabled = false;
        index = 0;
        DefaultCanvass.enabled = false;
        DialougeCanvas.enabled = true;
        interact.action.Enable();
        interact.action.performed += TakeAction;
        cutscene.blocks[index].action(this);
    }

    public void TakeAction(InputAction.CallbackContext c) {
        if (PauseMenu.inst.CurrentState)
            return;
        if (responding)// resnponding to quesation
        {
            if (responseIndex < 0)
                return;

            if (dh.Script.Writing) {
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
                NextBlock();
            }
            else {
                DisplayResponse();
            }
            return;
        }

        if (dh.Script.Writing) {
            dh.Script.Stop();
            return;
        }

        NextBlock();
    }

    public void HideUI() {
        InCutscene = false;
        DialougeCanvas.enabled = false;
        responseCanvas.enabled = false;
        DialougeBox.HideDialougeBox();
        UIState.inst.SetInteractable(true);
        interact.action.Disable();
        interact.action.performed -= TakeAction;
    }


    public void EndCutscene() {
        HideUI();

        if (Schedule != null)
            Schedule.SetTime(cutscene.TimeLength);
        if (EndingScene != null)
            EndingScene.CutsceneComplete();
    }

    public void PresentChoices() {
        UIState.inst.SetInteractable(true);
        //-----------------------------------------------------------------------------------------******-----+++++++------------Set BUTTON HERE
        DefaultCanvass.enabled = true;
        DialougeCanvas.enabled = false;
        DialougeCanvas.enabled = false;
        interact.action.Disable();
        interact.action.performed += TakeAction;
    }

    public void NextBlock() {
        index++;

        if (index >= cutscene.blocks.Length) {

            EndCutscene();
            if (cutscene.ForceDarts)// force play darts
                DartsMenu.ForceDartsException(CurrentCharacterIndex, Schedule.hour);
            return;
        }

        cutscene.blocks[index].action(this);
    }

    public void Dialouge(string message) {
        DialougeBox.SetCharacterColors(CurrentCharacterIndex);
        DialougeBox.SetDialouge(message);
    }

    public void Response(Response r) {
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
        if(DecideCharacter(character))
            ChangeExpression((int)expression);

    }

    public void UI_Response(int i) {
        UIState.inst.SetInteractable(false);
        responseCanvas.enabled = false;
        responseIndex = i;
        responseIndexIndex = 0;

        DisplayResponse();
    }

    private void DisplayResponse() {
        respon.responses[responseIndex].responses[responseIndexIndex].Adjust(this);
        if (!respon.responses[responseIndex].responses[responseIndexIndex].ResponseIsPlayerThought)
            Dialouge(respon.responses[responseIndex].responses[responseIndexIndex].Message);
        else
            Thought(respon.responses[responseIndex].responses[responseIndexIndex].Message);
    }

    public void ChangeExpression(int expressionIndex, bool GoToNextBlock = true) {
        if (CharacterPortrait.enabled == false)
            CharacterPortrait.enabled = true;
        CharacterNamePlate.SetActive(true);
        CharacterPortrait.sprite = characters.list[CurrentCharacterIndex].Expressions[expressionIndex];
        PortraitAnimation.ChangeExpression((Expressions)expressionIndex);
        DialougeBox.SetExpression(expressionIndex, true);
        if (GoToNextBlock)
            NextBlock();
    }

    public void Thought(string s) {
        DialougeBox.SetThought(s);
    }

    public void ChangeBackground(string s) {
        decideBackGround(s);
        NextBlock();
    }

    private void SetPartnerVisual(int partnerIndex, bool showPartnerVisual) {
        CurrentCharacterIndex = partnerIndex;

        CharacterPortrait.sprite = characters.list[partnerIndex].Expressions[0];
        DialougeBox.SetExpression(0, showPartnerVisual);
        DialougeBox.SetCharacterColors(partnerIndex);
        CharacterPortrait.enabled = showPartnerVisual;
        CharacterName.text = characters.list[partnerIndex].Name;
        CharacterName.font = characters.list[partnerIndex].Font;
        CharacterName.fontSize = characters.list[partnerIndex].textSize;
        CharacterNamePlate.SetActive(showPartnerVisual);


    }

    private void SetBackGroundVisual(int i) {
        cutSceneBackGround.sprite = BackgroundSprites.Sprites[i];
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
            case "bathroom":
                SetBackGroundVisual(3);
                break;
            case "darts":
                SetBackGroundVisual(4);
                break;
        }
    }

    private bool DecideCharacter(string s, bool showPartner = true) {
        switch (s.ToLower()) {
            case "faye":
                SetPartnerVisual(2, showPartner);
                return true;
            case "chad":
                SetPartnerVisual(0, showPartner);
                return true;
            case "jess":
                SetPartnerVisual(1, showPartner);
                return true;
            case "elaine":
                SetPartnerVisual(3, showPartner);
                return true;
            case "owner":
                SetPartnerVisual(4, showPartner);
                return true;
            case "bar guy":
                SetPartnerVisual(5, showPartner);
                return true;
            case "charming girl":
                SetPartnerVisual(6, showPartner);
                return true;
            case "charming guy":
                SetPartnerVisual(7, showPartner);
                return true;
            case "dance girl":
                SetPartnerVisual(8, showPartner);
                return true;
            case "lounge guy":
                SetPartnerVisual(9, showPartner);
                return true;
            default:
                if (s.Length <= 0) {
                    Debug.Log("HERE");
                    SetPartnerVisual(10, false);
                }
                else {
                    SetPartnerVisual(10, showPartner);
                    CharacterName.text = s;
                }
                return false;
        }
    }
}

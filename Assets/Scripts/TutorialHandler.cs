using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TutorialHandler : MonoBehaviour, Caller {
    [SerializeField] public static TutorialHandler inst;
    [SerializeField] public InputActionReference PauseInput;
    [SerializeField] GameObject ExitButton;
    [SerializeField] UIAnimationElement TutorialAnimation;
    [SerializeField] Canvas TutorialCanvas;
    [SerializeField] Image TutorialImage;
    [SerializeField] Canvas TutorialOvershadow;
    [SerializeField] Sprite DartsTutorialImage;
    [SerializeField] Sprite StoryTutorialImage;
    [SerializeField] TMP_Text TutorialText;
    [SerializeField] TextAsset DartsTutorialText;
    [SerializeField] TextAsset StoryTutorialText;
    [SerializeField] Caller Caller;

    public void Start() {
        if (inst != null) {
            Destroy(gameObject);
            return;
        }
        inst = this;
        DontDestroyOnLoad(this);

    }

    public bool ShouldDisplayTutorial() {
        return PlayerPrefs.GetInt("hasReadDartsTutorial") == 0;
    }

    public void EnableTutorial(bool enable, Sprite sprite, TextAsset textFile = null) {
        TutorialImage.sprite = sprite;
        TutorialCanvas.enabled = enable;
        TutorialOvershadow.enabled = enable;
        TutorialAnimation.Begin(this);

        if (textFile != null)
            TutorialText.text = textFile.text;

        if (enable) {
            PauseInput.action.performed += DisableTutorials;
            PauseMenu.inst.UnenablePause();
            PauseInput.action.Enable();
            UIState.inst.SetAsSelectedButton(ExitButton);// for controller compatibility
            DartSticker.inst.SetVisible(false);// to remove old sticker from screen
        } else {
            PauseInput.action.performed -= DisableTutorials;
            PauseMenu.inst.SetEnabled(true);

            if (Caller != null) {
                Caller.Ping();// pause menue handles all of this itself with ping()
                //PauseMenu.inst.BackGround.enabled = true;
               // PauseMenu.inst.CurrentState = !PauseMenu.inst.CurrentState;
                Caller = null;
            }
            if (sprite == StoryTutorialImage)// makes sure we are only setting flag if player reads Darts and not story tutorial
                return;
            if (PlayerPrefs.GetInt("hasReadDartsTutorial") != 1)
                PlayerPrefs.SetInt("hasReadDartsTutorial", 1);
        }
    }

    public void EnableStoryTutorial(bool enable, Caller caller = null) {
        if (caller != null) { Caller = caller; }
        EnableTutorial(enable, StoryTutorialImage, StoryTutorialText);
    }

    public void EnableDartsTutorial(bool enable, Caller caller = null) {
        if (caller != null) { Caller = caller; }
        EnableTutorial(enable, DartsTutorialImage, DartsTutorialText);
    }

    public void EnableStoryTutorial(bool enable) { EnableTutorial(enable, StoryTutorialImage, StoryTutorialText); }

    public void EnableDartsTutorial(bool enable) { EnableTutorial(enable, DartsTutorialImage, DartsTutorialText); }

    public void DisableTutorials() {EnableTutorial(false, DartsTutorialImage); }

    // This overload method is for player input
    public void DisableTutorials(InputAction.CallbackContext c) { DisableTutorials(); }

    // Empty method just for Caller purposes
    public void Ping() { }
}

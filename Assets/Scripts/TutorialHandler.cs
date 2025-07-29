using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TutorialHandler : MonoBehaviour, Caller {
    [SerializeField] public static TutorialHandler inst;
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
    [SerializeField] ControlVisual Visual;
    [SerializeField] InputActionReference FireButton;
    [SerializeField] bool DartsTutorial;
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

    public void TrySetFlag(Sprite sprite) {
        if (sprite == StoryTutorialImage)// makes sure we are only setting flag if player reads Darts and not story tutorial
            return;
        if (PlayerPrefs.GetInt("hasReadDartsTutorial") != 1)
            PlayerPrefs.SetInt("hasReadDartsTutorial", 1);
    }

    public void EnableTutorial(bool enable, Sprite sprite, TextAsset textFile = null) {
        DartSticker.inst.SetVisible(false);// to remove old sticker from screen
        TutorialImage.sprite = sprite;
        TutorialCanvas.enabled = enable;
        TutorialOvershadow.enabled = enable;
        TutorialAnimation.Begin(this);
       
        if (textFile != null) {
            if(textFile == DartsTutorialText) {
                UsingController(ControlState.inst.IsUsingController());
            }  
            else
            TutorialText.text = textFile.text;
        }
            

        if (enable) {
            if(textFile == DartsTutorialText) {
                if (!DartsTutorial) {
                    ControlState.UsingController += UsingController;
                    DartsTutorial = true;
                }
                
            }
            Visual.Begin();
            TrySetFlag(sprite);
            PauseMenu.inst.UnenablePause();
            UIState.inst.SetAsSelectedButton(ExitButton);// for controller compatibility
            DartSticker.inst.SetVisible(false);// to remove old sticker from screen
        } else {
            if (DartsTutorial) {
                DartsTutorial = false;
                ControlState.UsingController -= UsingController;
            }
            PauseMenu.inst.SetEnabled(true);
            Visual.End();
            if (Caller != null) {
                Caller.Ping();// pause menue handles all of this itself with ping()
                //PauseMenu.inst.BackGround.enabled = true;
               // PauseMenu.inst.CurrentState = !PauseMenu.inst.CurrentState;
                Caller = null;
            }
           
        }
    }

    public void UsingController(bool usingController) {
#if UNITY_EDITOR
        Debug.Log("FIRe");
#endif
        string moveControl = usingController ? "Left Stick" : "WASD";
        string fireControl = FireButton.action.GetBindingDisplayString(ControlState.inst.DefaultOptions, ControlState.inst.GetControlString());
        TutorialText.text = string.Format(DartsTutorialText.text, moveControl, fireControl);
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

    // Empty method just for Caller purposes
    public void Ping() { }
    private void OnDestroy() {
        ControlState.UsingController -= UsingController;
    }
}

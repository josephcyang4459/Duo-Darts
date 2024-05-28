using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TutorialHandler : MonoBehaviour, Caller {
    [SerializeField] public static TutorialHandler inst;
    [SerializeField] public InputActionReference PauseInput;
    [SerializeField] UIAnimationElement TutorialAnimation;
    [SerializeField] Canvas TutorialCanvas;
    [SerializeField] Image TutorialImage;
    [SerializeField] Canvas TutorialOvershadow;
    [SerializeField] Sprite DartsTutorialImage;
    [SerializeField] Sprite StoryTutorialImage;
    [SerializeField] Caller Caller;

    // TODO We still need to do the story tutorial
    public void Start() {
        if (inst != null) {
            Destroy(gameObject);
            return;
        }
        inst = this;
        DontDestroyOnLoad(this);

        SceneNumbers currentSceneName = TransitionManager.inst.GetCurrentScene();
        if (PlayerPrefs.GetInt("hasReadDartsTutorial") == 0) {
            switch(currentSceneName) {
                case SceneNumbers.Story: EnableTutorial(true, StoryTutorialImage); return;
                case SceneNumbers.Darts: EnableTutorial(true, DartsTutorialImage); return;
            }
        }
    }

    public void EnableTutorial(bool enable, Sprite sprite) {
        TutorialImage.sprite = sprite;
        TutorialCanvas.enabled = enable;
        TutorialOvershadow.enabled = enable;
        TutorialAnimation.Begin(this);

        if (enable) {
            PauseInput.action.performed += DisableTutorials;
            PauseMenu.inst.SetEnabled(false);
            PauseInput.action.Enable();
        }
    }

    public void EnableStoryTutorial(bool enable, Caller caller = null) {
        if (caller != null) { Caller = caller; }
        EnableTutorial(enable, StoryTutorialImage);
    }

    public void EnableDartsTutorial(bool enable, Caller caller = null) {
        if (caller != null) { Caller = caller; }
        EnableTutorial(enable, DartsTutorialImage);
    }

    public void EnableStoryTutorial(bool enable) { EnableTutorial(enable, StoryTutorialImage); }

    public void EnableDartsTutorial(bool enable) { EnableTutorial(enable, DartsTutorialImage); }


    public void DisableTutorials() {
        EnableTutorial(false, DartsTutorialImage);
        PauseMenu.inst.SetEnabled(true);

        if (Caller != null) {
            Caller.Ping();
            PauseMenu.inst.BackGround.enabled = true;
            PauseMenu.inst.CurrentState = !PauseMenu.inst.CurrentState;
            Caller = null;
        }

        PauseInput.action.performed -= DisableTutorials;
        if (PlayerPrefs.GetInt("hasReadDartsTutorial") != 1)
            PlayerPrefs.SetInt("hasReadDartsTutorial", 1);
    }

    // This overload method is for player input
    public void DisableTutorials(InputAction.CallbackContext c) { DisableTutorials(); }

    // Empty method just for Caller purposes
    public void Ping() { }
}

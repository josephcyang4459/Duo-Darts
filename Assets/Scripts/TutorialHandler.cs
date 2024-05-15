using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialHandler : MonoBehaviour {
    [SerializeField] public static TutorialHandler inst;
    [SerializeField] public InputActionReference PauseInput;
    [SerializeField] Canvas DartsTutorialCanvas;
    [SerializeField] Canvas StoryTutorialCanvas;
    [SerializeField] Canvas TutorialChoicesCanvas;
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
                case SceneNumbers.Story: EnableStoryTutorial(true); return;
                case SceneNumbers.Darts: EnableDartsTutorial(true); return;
            }
        }
    }

    public void EnableStoryTutorial(bool enable) {
        StoryTutorialCanvas.enabled = enable;

        if (enable) {
            PauseInput.action.performed += DisableTutorials;
            PauseMenu.inst.SetEnabled(false);
            PauseInput.action.Enable();
        }
    }

    public void EnableDartsTutorial(bool enable) {
        DartsTutorialCanvas.enabled = enable;

        if (enable) {
            PauseInput.action.performed += DisableTutorials;
            PauseMenu.inst.SetEnabled(false);
            PauseInput.action.Enable();
        }
    }

    public void EnableTutorialChoices(bool enable) {
        TutorialChoicesCanvas.enabled = enable;

        if (enable) {
            PauseInput.action.performed += DisableTutorials;
            PauseMenu.inst.SetEnabled(false);
            PauseInput.action.Enable();
        }
    }

    public void EnableStoryTutorial(bool enable, Caller caller = null) {
        if (caller != null) { Caller = caller; }
        EnableStoryTutorial(enable);
    }

    public void EnableDartsTutorial(bool enable, Caller caller = null) {
        if (caller != null) { Caller = caller; }
        EnableDartsTutorial(enable);
    }

    public void EnableTutorialChoices(bool enable, Caller caller = null) {
        if (caller != null) { Caller = caller; }
        EnableTutorialChoices(enable);
    }

    public void DisableTutorials() {
        EnableStoryTutorial(false);
        EnableDartsTutorial(false);
        EnableTutorialChoices(false);
        PauseMenu.inst.SetEnabled(true);

        if (Caller != null) {
            Caller.Ping();
            PauseMenu.inst.BackGround.enabled = true;
            PauseMenu.inst.CurrentState = !PauseMenu.inst.CurrentState;
            Caller = null;
        }

        PauseInput.action.performed -= DisableTutorials;
        PlayerPrefs.SetInt("hasReadDartsTutorial", 1);
    }

    // This overload method is for player input
    public void DisableTutorials(InputAction.CallbackContext c) { DisableTutorials(); }
}

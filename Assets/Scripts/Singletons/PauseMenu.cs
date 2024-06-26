using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour, Caller {
    public static PauseMenu inst;
    [SerializeField] public InputActionReference PauseInput;
    [SerializeField] public bool Enabled;
    [SerializeField] public Canvas PauseOptionsCanvas;
    [SerializeField] public Canvas StoryOptionsCanvas;
    [SerializeField] public Canvas BackGround;
    [SerializeField] PauseUI[] UI;
    public bool CurrentState;
    [SerializeField] SceneNumbers CurrentScene;
    [SerializeField] GameObject FirstSelected;
    [SerializeField] GameObject StoryFirstSelected;
    GameObject returnGameObjectButton;
    bool returnState;

    public void SetCurrentScene(SceneNumbers currentScene) { CurrentScene = currentScene; }

    public void SetDartsTutorialActive(bool active) { TutorialHandler.inst.EnableDartsTutorial(active, this); }

    public void SetStoryTutorialActive(bool active) { TutorialHandler.inst.EnableStoryTutorial(active, this); }

    public void Awake() {
        if (inst != null) {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(this);
        inst = this;
    }

    public void SetEnabled(bool enabled, bool setFirstButtonUponUnenable = true) {
        if (enabled)
            EnablePause();
        else {
            UnenablePause();
            CurrentState = false;
            ConsequencesOfCurrentState(setFirstButtonUponUnenable);
        }
        

        Enabled = enabled;
    }

    public void UnenablePause() {
        PauseInput.action.Disable();
        PauseInput.action.performed -= ActivatePauseMenu;
    }

    public void EnablePause() {
        //ControlTutuorialUI.inst.SetControl((int)Controls.Pause, true);
        PauseInput.action.Enable();
        PauseInput.action.performed += ActivatePauseMenu;
    }


    void ActivatePauseMenu(InputAction.CallbackContext c) {PauseMenueStateChange(); }

    void PauseMenueStateChange() {
        CurrentState = !CurrentState;
        ConsequencesOfCurrentState();
    }

    void SetCorrectCanvas(bool state) {
        Canvas canvas = (CurrentScene == SceneNumbers.Story) ? StoryOptionsCanvas : PauseOptionsCanvas;
        canvas.enabled = state;
    }

    void ConsequencesOfCurrentState(bool setFirstButtonUponUnenable = true) {
       // UIState.inst.ResetClick();
        BackGround.enabled = CurrentState;
        SetCorrectCanvas(CurrentState);

        if (CurrentState) {
            returnGameObjectButton = UIState.inst.GetCurrentSelected();
            returnState = UIState.inst.GetCurrentState();
            if (CutsceneHandler.Instance.InCutscene)
                CutsceneHandler.Instance.UnenableControls();
            OptionsMenu.inst.HideOptionsNoCall();
            UIState.inst.SetInteractable(true);
            DartSticker.inst.SetVisible(false);

            GameObject firstSelected = (CurrentScene == SceneNumbers.Story) ? StoryFirstSelected : FirstSelected;
            UIState.inst.SetAsSelectedButton(firstSelected);
        }
        else {
            UI[0].ClearFill();
            UI[1].ClearFill();
            DartSticker.inst.SetVisible(false);
            OptionsMenu.inst.HideOptionsNoCall();
            if (CutsceneHandler.Instance.InCutscene)
                CutsceneHandler.Instance.EnableControls();
            if (setFirstButtonUponUnenable)
                UIState.inst.SetAsSelectedButton(returnGameObjectButton);
            UIState.inst.SetInteractable(returnState);
        }
    }

    private void OnDestroy() {
        if (Enabled) {
            PauseInput.action.Disable();
            PauseInput.action.performed -= ActivatePauseMenu;
        }
    }

    public void ShowOptions() {
        SetCorrectCanvas(false);
        OptionsMenu.inst.ShowOptions(this);
    }

    public void ExitToMain() {
        if (CutsceneHandler.Instance.InCutscene)
            CutsceneHandler.Instance.HideUI();

        TransitionManager.inst.GoToScene(SceneNumbers.MainMenu);
    }

    public void ExitToDesktop() { Application.Quit(); }

    public void Save() {
        SaveHandler.inst.BeginShowSaveMenu();
    }

    public void Ping() {
        SetCorrectCanvas(true);
        GameObject firstSelected = (CurrentScene == SceneNumbers.Story) ? StoryFirstSelected : FirstSelected;
        UIState.inst.SetAsSelectedButton(firstSelected);
    }
}

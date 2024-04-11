using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour, Caller {
    public static PauseMenu inst;
    [SerializeField] InputActionReference PauseInput;
    [SerializeField] bool Enabled;
    [SerializeField] Canvas PauseOptionsCanvas;
    [SerializeField] Canvas StoryOptionsCanvas;
    [SerializeField] Canvas BackGround;
    [SerializeField] PauseUI[] UI;
    public bool CurrentState;
    [SerializeField] bool IsInStory;
    [SerializeField] GameObject FirstSelected;
    [SerializeField] GameObject StoryFirstSelected;
    GameObject returnGameObjectButton;
    bool returnState;

    public void IsInStoryScene(bool isInStory) {
        IsInStory = isInStory;
    }

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
        else
            UnenablePause(setFirstButtonUponUnenable);
        Enabled = enabled;
    }

    void EnablePause() {
        //ControlTutuorialUI.inst.SetControl((int)Controls.Pause, true);
        PauseInput.action.Enable();
        PauseInput.action.performed += ActivatePauseMenu;
    }

    void UnenablePause(bool setFirstButtonUponUnenable = true) {
        //ControlTutuorialUI.inst.SetControl((int)Controls.Pause, false);
        CurrentState = false;
        ConsequencesOfCurrentState(setFirstButtonUponUnenable);
        PauseInput.action.Disable();
        PauseInput.action.performed -= ActivatePauseMenu;
    }

    void ActivatePauseMenu(InputAction.CallbackContext c) {
        PauseMenueStateChange();
    }

    void PauseMenueStateChange() {
        CurrentState = !CurrentState;
        ConsequencesOfCurrentState();
    }

    void SetCorrectCanvas(bool state) {
        if (IsInStory) {
            StoryOptionsCanvas.enabled = state;
        }
        else {
            PauseOptionsCanvas.enabled = state;
        }
    }

    void ConsequencesOfCurrentState(bool setFirstButtonUponUnenable = true) {
        BackGround.enabled = CurrentState;
        SetCorrectCanvas(CurrentState);

        if (CurrentState) {

            returnGameObjectButton = UIState.inst.GetCurrentSelected();
            returnState = UIState.inst.GetCurrentState();
            OptionsMenu.inst.HideOptionsNoCall();
            UIState.inst.SetInteractable(true);
            DartSticker.inst.SetVisible(false);
            if (IsInStory) {
                UIState.inst.SetAsSelectedButton(StoryFirstSelected);
            }
            else {
                UIState.inst.SetAsSelectedButton(FirstSelected);
            }
        }
        else {
            UI[0].ClearFill();
            UI[1].ClearFill();
            DartSticker.inst.SetVisible(false);
            OptionsMenu.inst.HideOptionsNoCall();
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
        if (CutsceneHandler.inst.InCutscene)
            CutsceneHandler.inst.HideUI();
        TransitionManager.inst.GoToScene(SceneNumbers.MainMenu);
    }

    public void ExitToDesktop() {
        Application.Quit();
    }

    public void Save() {
        if (!CutsceneHandler.inst.InCutscene)
            SaveHandler.inst.BeginShowSaveMenu();
        else
            Audio.inst.PlayClip(AudioClips.Click);
    }

    public void Ping() {
        SetCorrectCanvas(true);
        UIState.inst.SetAsSelectedButton(FirstSelected);
    }
}

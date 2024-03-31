using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour, Caller
{
    public static PauseMenu inst;
    [SerializeField] InputActionReference PauseInput;
    [SerializeField] bool Enabled;
    [SerializeField] Canvas PauseOptionsCanvas;
    [SerializeField] Canvas StoryOptionsCanvas;
    [SerializeField] Canvas BackGround;
    
    [SerializeField] bool CurrentState;

    [SerializeField] GameObject FirstSelected;
    GameObject returnGameObjectButton;
    bool returnState;
    public void Awake()
    {
        if(inst !=null)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(this);
        inst = this;
    }

    public void SetEnabled(bool enabled, bool setFirstButtonUponUnenable =true)
    {
        if (enabled)
            EnablePause();
        else
            UnenablePause(setFirstButtonUponUnenable);
        Enabled = enabled;
    }

    void EnablePause()
    {
        //ControlTutuorialUI.inst.SetControl((int)Controls.Pause, true);
        PauseInput.action.Enable();
        PauseInput.action.performed += ActivatePauseMenu;
    }

    void UnenablePause(bool setFirstButtonUponUnenable = true)
    {
        //ControlTutuorialUI.inst.SetControl((int)Controls.Pause, false);
        CurrentState = false;
        ConsequencesOfCurrentState(setFirstButtonUponUnenable);
        PauseInput.action.Disable();
        PauseInput.action.performed -= ActivatePauseMenu;
    }

    void ActivatePauseMenu(InputAction.CallbackContext c)
    {
        PauseMenueStateChange();
    }

    void PauseMenueStateChange()
    {
        CurrentState = !CurrentState;
        ConsequencesOfCurrentState();
    }


    void ConsequencesOfCurrentState(bool setFirstButtonUponUnenable = true) {
        PauseOptionsCanvas.enabled = CurrentState;
        BackGround.enabled = CurrentState;
        if(TransitionManager.inst!=null)
        if (TransitionManager.inst.GetCurrentScene() == SceneNumbers.Story)
            StoryOptionsCanvas.enabled = CurrentState;
        if (CurrentState) {
            returnGameObjectButton = UIState.inst.GetCurrentSelected();
            returnState = UIState.inst.GetCurrentState();
            UIState.inst.SetInteractable(true);
            OptionsMenu.inst.HideOptions();
            UIState.inst.SetAsSelectedButton(FirstSelected);
        }
        else {
            if (setFirstButtonUponUnenable)
                UIState.inst.SetAsSelectedButton(returnGameObjectButton);
            UIState.inst.SetInteractable(returnState);
        }
    }

    private void OnDestroy()
    {
        if (enabled) {
            PauseInput.action.Disable();
            PauseInput.action.performed -= ActivatePauseMenu;
        }
    }


    public void ShowOptions()
    {
        PauseOptionsCanvas.enabled = false;
        OptionsMenu.inst.ShowOptions(this);
    }

    public void ExitToMain()
    {
        //PauseMenueStateChange();
        TransitionManager.inst.GoToScene(SceneNumbers.MainMenu);
    }

    public void ExitToDesktop()
    {
        Application.Quit();
    }

    public void Ping()
    {
        UIState.inst.SetAsSelectedButton(FirstSelected);
        PauseOptionsCanvas.enabled = true;
    }
}

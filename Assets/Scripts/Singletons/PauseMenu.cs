using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour, Caller
{
    public static PauseMenu inst;
    [SerializeField] InputActionReference PauseInput;
    [SerializeField] bool Enabled;
    [SerializeField] Canvas PauseOptionsCanvas;
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

    public void SetEnabled(bool enabled)
    {
        if (enabled)
            EnablePause();
        else
            UnenablePause();
        Enabled = enabled;
    }

    void EnablePause()
    {
        //ControlTutuorialUI.inst.SetControl((int)Controls.Pause, true);
        PauseInput.action.Enable();
        PauseInput.action.performed += ActivatePauseMenu;
    }

    void UnenablePause()
    {
        //ControlTutuorialUI.inst.SetControl((int)Controls.Pause, false);
        CurrentState = false;
        ConsequencesOfCurrentState();
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


    void ConsequencesOfCurrentState() {
        PauseOptionsCanvas.enabled = CurrentState;
        BackGround.enabled = CurrentState;
        if (CurrentState) {
            returnGameObjectButton = UIState.inst.GetCurrentSelected();
            returnState = UIState.inst.GetCurrentState();
            UIState.inst.SetInteractable(true);
            OptionsMenu.inst.HideOptions();
            UIState.inst.SetAsSelectedButton(FirstSelected);
        }
        else {
            UIState.inst.SetAsSelectedButton(returnGameObjectButton);
            UIState.inst.SetInteractable(returnState);
        }
    }

    private void OnDestroy()
    {
        if (enabled)
            UnenablePause();
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

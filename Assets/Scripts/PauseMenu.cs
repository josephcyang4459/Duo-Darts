using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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
        PauseOptionsCanvas.enabled = CurrentState;
        BackGround.enabled = CurrentState;
        if (CurrentState)
        {
            returnGameObjectButton = UIState.inst.GetCurrentSelected();
            OptionsMenu.inst.HideOptions();
            UIState.inst.SetAsSelectedButton(FirstSelected);
        }
        else
        {
            UIState.inst.SetAsSelectedButton(returnGameObjectButton);
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
        PauseMenueStateChange();
        SceneManager.LoadScene(0);
        System.GC.Collect();
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

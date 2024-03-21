using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu inst;
    [SerializeField] InputActionReference PauseInput;
    [SerializeField] bool Enabled;
    [SerializeField] Canvas PauseCanvas;

    public void Awake()
    {
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
        ControlTutuorialUI.inst.SetControl((int)Controls.Pause, true);
        PauseInput.action.Enable();
        PauseInput.action.performed += ActivatePauseMenu;
    }

    void UnenablePause()
    {
        ControlTutuorialUI.inst.SetControl((int)Controls.Pause, false);
        PauseInput.action.Disable();
        PauseInput.action.performed -= ActivatePauseMenu;
    }

    void ActivatePauseMenu(InputAction.CallbackContext c)
    {
        PauseCanvas.enabled = !PauseCanvas.enabled;
    }

    public void ExitToMain()
    {
        SceneManager.LoadScene(0);
        System.GC.Collect();
    }

    public void ExitToDesktop()
    {
        Application.Quit();
    }
}

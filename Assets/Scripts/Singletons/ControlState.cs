using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ControlState : MonoBehaviour, Caller {
    public static ControlState inst;
    [SerializeField] ControllerState UseConnectedState;
    [SerializeField] bool ControllerConnected;
    [SerializeField] Sprite ControllerSprite;
    [SerializeField] Sprite MouseSprite;
    [SerializeField] Image ControlImage;
    [SerializeField] UIAnimationElement AnimationHead;
    bool CurrentlyAnimating;

    public delegate void State(bool state);
    public static event State UsingController;

    public InputBinding.DisplayStringOptions DefaultOptions;

    void Awake() {
        if (inst != null) {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(this);
        inst = this;
        InputSystem.onDeviceChange += DeviceChange;
        CurrentlyAnimating = false;
        CheckForControllerConnected();
    }

    public void SetControlState(ControllerState newState) {
        UseConnectedState = newState;
        ShowControlAnimation();
    }

    public void ShowControlAnimation() {
        if (CurrentlyAnimating) {
            AnimationHead.ReachEndState();
        }
        CurrentlyAnimating = true;
        ControlImage.sprite = IsUsingController() ? ControllerSprite : MouseSprite;
        AnimationHead.Begin(this);
    }

    public bool IsUsingController() {
        switch (UseConnectedState) {
            case ControllerState.UseIfConnected: return ControllerConnected;
            case ControllerState.ForceKeyboard: return false;
            case ControllerState.ForceController: return true;
        }
        return false;
    }

    void DeviceChange(InputDevice device, InputDeviceChange change) {
        bool connected = IsUsingController();
        switch (change) {
            case InputDeviceChange.Added:
                // New Device.
               // Debug.Log("Added");
                if (ControllerConnected)
                    break;
                if (IsController(device)) {
                    ControllerConnected = true;
                    UsingController(IsUsingController());
                }
                break;
            case InputDeviceChange.Disconnected:
               // Debug.Log("Disconnect");
                // Device got unplugged.
                CheckForControllerConnected();
                break;
            case InputDeviceChange.Reconnected:
               // Debug.Log("Reconnect");
                // Plugged back in.
                CheckForControllerConnected();
                break;
        }

        if (connected != IsUsingController())
            ShowControlAnimation();
    }

    void CheckForControllerConnected() {
        ControllerConnected = false;

        for (int i = 0; i < InputSystem.devices.Count; i++) {
            if (IsController(InputSystem.devices[i])) {
                ControllerConnected = true;
                if (!IsUsingController())
                    return;
            }
        }
        if (UIState.inst != null)
            UsingController(ControllerConnected);
    }

    bool IsController(InputDevice type) {
        if (type.displayName.Contains("Contro")) {
            return true;
        }
        if (type.displayName.Contains("Gamepa")) {
            return true;
        }
        if (type.displayName.Contains("Joys")) {
            return true;
        }
        return false;
    }

    public void Ping() {
        CurrentlyAnimating = false;
    }
}

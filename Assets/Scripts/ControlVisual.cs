using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlVisual : MonoBehaviour
{
    [Header("The Parent Of these object should proably have Horizontal Group")]
    //this is incredibly simplistic as we do not support specialized graphics for each button
    [SerializeField] GameObject GamepadControlElement;
    [SerializeField] GameObject KeyboardControlElement;
    [SerializeField] InputActionReference InputAction;
    [SerializeField] TMP_Text ControlText;

    public void Begin() {
        ControlState.UsingController += CheckController;
        CheckController(ControlState.inst.IsUsingController());
    }

    public void CheckController(bool isUsingController) {
        if (InputAction != null)
            ControlText.text = InputAction.action.GetBindingDisplayString(ControlState.inst.DefaultOptions,"Gamepad");
        GamepadControlElement.SetActive(isUsingController);
        KeyboardControlElement.SetActive(!isUsingController);
    }

    public void End() {
        ControlState.UsingController -= CheckController;
    }

    public void OnDestroy() {
        End();
    }
}

using UnityEngine;

public class ControlVisual : MonoBehaviour
{
    [Header("The Parent Of these object should proably have Horizontal Group")]
    //this is incredibly simplistic as we do not support specialized graphics for each button
    [SerializeField] GameObject GamepadControlElement;
    [SerializeField] GameObject KeyboardControlElement;

    public void Begin() {
        ControlState.UsingController += CheckController;
        CheckController(ControlState.inst.IsUsingController());
    }

    public void CheckController(bool isUsingController) {
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

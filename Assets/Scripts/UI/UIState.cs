using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.InputSystem.UI;

public class UIState : MonoBehaviour {
    public static UIState inst;
    [SerializeField] EventSystem EventSystem;
    [SerializeField] InputSystemUIInputModule UI;
    [SerializeField] GameObject CurrentFirstSelected;
    [SerializeField] bool IsUsingController;
    public void Awake()
    {
        if(inst != null)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(this);
        ControlState.UsingController += ControllerConnected;
        inst = this;
    }


    public void SetAsSelectedButton(GameObject gameObject) {
        CurrentFirstSelected = gameObject;
        if (IsUsingController) {
            EventSystem.SetSelectedGameObject(gameObject);
        }
    }

    public void ControllerConnected(bool state) {
        IsUsingController = state;
        if (!state)
            return;
        if (EventSystem.enabled) {
            EventSystem.SetSelectedGameObject(CurrentFirstSelected);
        }
    }

    public GameObject GetCurrentSelected() {  return EventSystem.currentSelectedGameObject != null ? EventSystem.currentSelectedGameObject : CurrentFirstSelected; }

    public void SetInteractable(bool enabled) {
        EventSystem.enabled = enabled;
        if (enabled)
            UI.leftClick.action.Enable();
        else
            UI.leftClick.action.Disable();
    }

    public bool GetCurrentState() { return EventSystem.enabled; }
}

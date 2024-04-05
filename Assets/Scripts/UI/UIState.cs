using UnityEngine.EventSystems;
using UnityEngine;

public class UIState : MonoBehaviour
{
    public static UIState inst;
    [SerializeField] EventSystem EventSystem;
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

    public void ControllerConnected(bool state)
    {
        IsUsingController = state;
        if (!state)
            return;
        if (EventSystem.enabled) {
            EventSystem.SetSelectedGameObject(EventSystem.currentSelectedGameObject != null ? EventSystem.currentSelectedGameObject : CurrentFirstSelected);
        }
            
    }

    public GameObject GetCurrentSelected()
    {
        return EventSystem.currentSelectedGameObject != null ? EventSystem.currentSelectedGameObject : CurrentFirstSelected;
    }

    public void SetInteractable(bool enabled)
    {
        EventSystem.enabled = enabled;
    }

    public bool GetCurrentState()
    {
        return EventSystem.enabled;
    }
}

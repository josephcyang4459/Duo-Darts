using UnityEngine.EventSystems;
using UnityEngine;

public class UIState : MonoBehaviour
{
    public static UIState inst;
    [SerializeField] EventSystem EventSystem;
    [SerializeField] GameObject CurrentFirstSelected;
    public void Awake()
    {
        if(inst != null)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(this);
        inst = this;
    }

    public void SetAsSelectedButton(GameObject gameObject)
    {
        if (!ControlState.inst.IsUsingController())
        {
            CurrentFirstSelected = gameObject;
            return;
        }
        EventSystem.SetSelectedGameObject(gameObject);
    }

    public void ControllerConnected()
    {
        if (EventSystem.enabled)
            EventSystem.SetSelectedGameObject(CurrentFirstSelected);
    }

    public GameObject GetCurrentSelected()
    {
        return CurrentFirstSelected;
    }

    public void SetInteractable(bool enabled)
    {
        //EventSystem.enabled = enabled;
    }

    public bool GetCurrentState()
    {
        return EventSystem.enabled;
    }
}

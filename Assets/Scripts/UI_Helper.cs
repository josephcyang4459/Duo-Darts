using UnityEngine;
using UnityEngine.EventSystems;

public static class UI_Helper
{
    /// <summary>
    /// Sets Given GameObject as the selected UI Element. Please Make Sure This GameObject Has A UI Component.
    /// </summary>
    /// <param name="UIElementToStartSelected">Button, Slider, Text Field, etc. Just Make Sure It Has A UI Element As A Component Thanks</param>
    public static void SetSelectedUIElement(GameObject UIElementToStartSelected)
    {
        var eventSystem = EventSystem.current;
        if (eventSystem.currentSelectedGameObject != UIElementToStartSelected)
            eventSystem.SetSelectedGameObject(UIElementToStartSelected, new BaseEventData(eventSystem));
    }
}

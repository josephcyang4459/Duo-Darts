using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class UIToggle : MonoBehaviour, Caller
{
    [SerializeField] UIA_MultiImageFill OnAnimation;
    [SerializeField] UIA_MultiImageFill OffAnimation;
    [SerializeField] Image[] OnImages;
    [SerializeField] Image[] OffImages;
    [SerializeField] bool state;
    [SerializeField] UnityEvent ChangeState;
    [SerializeField] Button Button;
    [SerializeField] TMP_Text Text;
    [SerializeField] ColorSwatch InteractableStateColors;
    [SerializeField] Color SelectedColor;

    public bool GetState() {
        return state;
    }

    public void SetInteractable(bool b) {
        Button.interactable = b;
        SetInteractableColor();
    }

    void SetInteractableColor() {
        Text.color = InteractableStateColors.colors[Button.interactable ? 0 : 1];
    }

    public void SelectButton() {
        Text.color = SelectedColor;
    }

    public void DeselectButton() {
        SetInteractableColor();
    }

    public void SetInitialState(bool b) {
        state = b;
        if (state)
            OnAnimation.ReachEndState();
        else
            OffAnimation.ReachEndState();
    }



    void RemoveOldImages(Image[] image) {
        foreach (Image i in image)
            i.fillAmount = 0;
    }

    public void InvokeChangeState() {
        ChangeState.Invoke();
    }

    public void ChangeStateNoInvoke(bool b) {
        VisualChangeState(b);    
    }

    void VisualChangeState(bool b) {
        RemoveOldImages(state ? OnImages : OffImages);
        state = b;
        if (state)
            OnAnimation.Begin(this);
        else
            OffAnimation.Begin(this);
        
    }

    public void ToggleStateInvoke() {
        VisualChangeState(!state);
        ChangeState.Invoke();
    }

    public void Ping() {

    }

#if UNITY_EDITOR
    [SerializeField] Color __OnColor;
    [SerializeField] Color __OffColor;
    [SerializeField] bool __useSwatch;
    [SerializeField] ColorSwatch __Swatch;
    [SerializeField] bool __reset;

    void __setImagesColor(Image[] images, Color color) {
        foreach (Image i in images)
            i.color = color;
    }

    void OnValidate() {
        if (!__reset)
            return;
        __reset = false;
        __setImagesColor(OnImages, __useSwatch ? __Swatch.colors[0] : __OnColor);
        __setImagesColor(OffImages, __useSwatch ? __Swatch.colors[1] : __OffColor);
    }
#endif
}

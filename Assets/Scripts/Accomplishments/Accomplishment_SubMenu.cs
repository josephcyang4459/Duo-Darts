using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public abstract class Accomplishment_SubMenu : MonoBehaviour
{
    [SerializeField] protected AccomplishmentsUI Accomplishents;
    [SerializeField] protected InputActionReference Back;
    [SerializeField] Color SelectedColor;
    [SerializeField] Image[] ToColor;

    public Color GetColor() {
        return SelectedColor;
    }

    public void SetColor(Color c) {
        foreach (Image i in ToColor) {
            i.color = c;
        }
    }

    public abstract void Begin();
    public abstract void End();
}

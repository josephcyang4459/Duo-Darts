using UnityEngine;

public class ButtonEventPassThrough : MonoBehaviour {
    [SerializeField] ButtonEventReceiver Reciever;
    [SerializeField] int Index;

    public void SetReciever(ButtonEventReceiver receiver, int index) {
        Reciever = receiver;
        Index = index;
    }

    public void Click() {
        Reciever.ButtonClick(Index);
    }
    public void PointerEnter() {
        Reciever.PointerEnter(Index);
    }
    public void PointerExit() {
        Reciever.PointerExit(Index);
    }
    public void Select() {
        Reciever.Select(Index);
    }
    public void Deselect() {
        Reciever.Deselect(Index);
    }
}

public interface ButtonEventReceiver {
    public void ButtonClick(int i);
    public void PointerEnter(int i);
    public void PointerExit(int i);
    public void Select(int i);
    public void Deselect(int i);
}

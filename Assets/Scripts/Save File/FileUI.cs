using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FileUI : MonoBehaviour, Caller {
    [SerializeField] FileHandler FileHandler;
    [SerializeField] Canvas LoadMenuCanvas;
    [SerializeField] Transform[] DartPositions;
    [SerializeField] TMP_Text[] SlotInfo;
    [SerializeField] Button[] SlotButtons;
    [SerializeField] bool SetStateOnNoFile;
    [SerializeField] UIAnimationElement EnterAnimationHead;
    [SerializeField] UIAnimationElement ExitAnimationHead;
    [SerializeField] GroupImageFill Fill;
    [SerializeField] Fillable_Image[] ImageFills;
    [SerializeField] GameObject FirstSelected;
    [SerializeField] AnimationState State;
    [SerializeField] Caller Caller;
    readonly string NoFile = "NO FILE";

    public void BeginShowLoadMenu(Caller caller) {
        Caller = caller;
        DartSticker.inst.SetVisible(false);
        UIState.inst.SetInteractable(false);
        State = AnimationState.Entering;
        EnterAnimationHead.Begin(this);
        LoadMenuCanvas.enabled = true;
        for (int i = 0; i < SlotInfo.Length; i++) {
            SaveFile file = FileHandler.LoadSaveFile(i);
            if (file != null) {
                SlotInfo[i].text = file.GetDisplayData();
            }
            else {
                SlotButtons[i].interactable = SetStateOnNoFile;
                SlotInfo[i].text = NoFile;
            }

        }
    }

    void ShowLoadMenu() {
        UIState.inst.SetInteractable(true);
        UIState.inst.SetAsSelectedButton(FirstSelected);
    }

    public void BeginHideLoadMenu() {
        UIState.inst.SetInteractable(false);
        DartSticker.inst.SetVisible(false);
        State = AnimationState.Exiting;
        ExitAnimationHead.Begin(this);
    }

    void HideLoadMenu() {
        Fill.ClearImages();
        UIState.inst.SetInteractable(true);
        LoadMenuCanvas.enabled = false;
        Caller.Ping();
    }

    public void SelectButton(int index) {
        Fill.SetCurrentImageToFill(ImageFills[index], DartPositions[index].position);
    }

    public void Ping() {

        if (State == AnimationState.Entering)
            ShowLoadMenu();
        if (State == AnimationState.Exiting)
            HideLoadMenu();
    }

    enum AnimationState {
        Entering,
        Exiting
    }

}

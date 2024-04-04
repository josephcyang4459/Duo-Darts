using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadFile : MonoBehaviour, Caller
{
    [SerializeField] FileHandler FileHandler;
    [SerializeField] Canvas LoadMenuCanvas;
    [SerializeField] Transform[] DartPositions;
    [SerializeField] Vector3 Offset;
    [SerializeField] TMP_Text[] SlotInfo;
    [SerializeField] GameObject[] SlotGameObjects;
    [SerializeField] Image[] SlotMasks;
    [SerializeField] UIAnimationElement EnterAnimationHead;
    [SerializeField] UIAnimationElement ExitAnimationHead;
    [SerializeField] UIAnimationElement[] SlotEnterAnimationHeads;
    [SerializeField] UIAnimationElement[] SlotExitAnimationHeads;
    [SerializeField] GroupImageFill Fill;
    [SerializeField] Fillable_Image[] ImageFills;
    [SerializeField] GameObject FirstSelected;
    [SerializeField] MainMenu MainMenu;
    [SerializeField] int WaitngFor;
    [SerializeField] AnimationState State;

    public void BeginShowLoadMenu() {
        DartSticker.inst.SetVisible(false);
        UIState.inst.SetInteractable(false);
        State = AnimationState.Entering;
        WaitngFor = 1;
        EnterAnimationHead.Begin(this);
        LoadMenuCanvas.enabled = true;
        for (int i = 0; i < SlotInfo.Length; i++) {
            string s = FileHandler.GetInfoFromSystemFileLine(i);
            if (s != null) {
                SlotInfo[i].text = FormatedSystemFileLine(s);
                SlotGameObjects[i].SetActive(true);
                WaitngFor++;
                SlotMasks[i].fillOrigin = WaitngFor % 2;
                SlotEnterAnimationHeads[i].Begin(this);
            }
            else
                SlotGameObjects[i].SetActive(false);
        }
        ShowLoadMenu();
    }
    
    void ShowLoadMenu() {
        UIState.inst.SetInteractable(true);
        UIState.inst.SetAsSelectedButton(FirstSelected);
    }

    public void BeginHideLoadMenu() {
        UIState.inst.SetInteractable(false);
        DartSticker.inst.SetVisible(false);
        State = AnimationState.Exiting;
        WaitngFor = 1;
        ExitAnimationHead.Begin(this);
        for (int i = 0; i < SlotInfo.Length; i++) {
            if (SlotGameObjects[i].activeInHierarchy) {
                WaitngFor++;
                SlotExitAnimationHeads[i].Begin(this);
            }
 
        }
    }

    void HideLoadMenu() {
        UIState.inst.SetInteractable(true);
        LoadMenuCanvas.enabled = false;
        MainMenu.Ping();
    }

    public void SelectButton(int index) {
        Fill.SetCurrentImageToFill(ImageFills[index], DartPositions[index].position + Offset);
    }

    /// <summary>
    /// Joseph's code from schedule :)
    /// </summary>
    /// <param name="hour"></param>
    /// <param name="minutes"></param>
    /// <returns></returns>
    private string TimeAsString(int hour, int minutes ) {
        string minutesString = (minutes < 10) ? "0" : "";
        minutesString += minutes.ToString();
        return hour + ":" + minutesString + "PM";
    }

    string FormatedSystemFileLine(string s) {
        string result = "";
        string[] info = s.Split('|');
        result = TimeAsString(int.Parse(info[(int)SystemFileParse.Hour]), int.Parse(info[(int)SystemFileParse.Minute]));
        return result;
    }

    public void SetLoadFile(int index) {
        TransitionManager.inst.SetFileIndex(index);
        TransitionManager.inst.GoToScene(SceneNumbers.Story);
    }

    public void Ping() {
        WaitngFor--;
        if (WaitngFor <= 0) {
            if (State == AnimationState.Entering)
                ShowLoadMenu();
            if (State == AnimationState.Exiting)
                HideLoadMenu();
        }
            
        //throw new System.NotImplementedException();
    }

    enum AnimationState {
        Entering,
        Exiting
    }

}

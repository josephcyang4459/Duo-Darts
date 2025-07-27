using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchivementsPopup : MonoBehaviour, Caller
{
    [SerializeField] List<Achievement> PopupQueue;
    [SerializeField] bool Animating;
    [SerializeField] UIAnimationElement EnterAnimationHead;
    [SerializeField] Image Icon;
    [SerializeField] TMP_Text AchivementName;
    [SerializeField] AudioClip Clip;


    public void AddToQueue(Achievement achivement) {
        PopupQueue.Add(achivement);
        if (Animating)
            return;
        Animating = true;
        BeginNewPopup();
    }

    void BeginNewPopup() {
        Audio.inst.PlayDartClipReverb(DartAudioClips.Hard, AudioReverbPreset.Psychotic);
        Icon.sprite = PopupQueue[0].GetCompletedIcon();
        AchivementName.text = PopupQueue[0].GetDisplayName();
        EnterAnimationHead.Begin(this);
    }

    public void Ping() {
        PopupQueue.RemoveAt(0);
        if (PopupQueue.Count > 0) {
            BeginNewPopup();
            return;
        }
        Animating = false;
    }
}

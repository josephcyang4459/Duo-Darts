using UnityEngine;
using UnityEngine.UI;

public class CreditScreen : MonoBehaviour, Caller, SceneEntrance
{

    [SerializeField] Vector3 Offset;

    [SerializeField] ImageFill LeftFill;// lol this predates group image fill so i raw dogged this shit with 2 seperate image fills
    [SerializeField] ImageFill RightFill;// this could be migrated to a single GroupImageFill but... why bother at this point lol
    [SerializeField] Image Left;
    [SerializeField] Image Right;
    [SerializeField] Image Hidden0;
    [SerializeField] Image Hidden1;
    [SerializeField] UIAnimationElement AnimationHead;
    [SerializeField] GameObject FirstSelectedButton;
    [SerializeField] TextCycle[] TextCycles;
    [SerializeField] Transform MusicCreditsDartTarget;
    [SerializeField] Canvas MusicCredits;
    [SerializeField] GameObject CloseMusicButton;
    [SerializeField] Canvas LicenseCanvas;
    [SerializeField] GameObject CloseLicenseButton;
    [SerializeField] LicenseReader Reader;

    public void Start()
    {
        TransitionManager.inst.ReadyToEnterScene(this);
        Audio.inst.PlaySong(MusicTrack.Credits);
    }

    public void EnterScene() {
        UIState.inst.SetAsSelectedButton(FirstSelectedButton);
        foreach (TextCycle t in TextCycles)
            t.SetText(0);
        AnimationHead.Begin(this);
    }

    public void HoverShowMusicCredits() {
        DartSticker.inst.NewLocation(MusicCreditsDartTarget.position);
    }

    public void ShowLicense() {
        Reader.Open();
        LicenseCanvas.enabled = true;
        UIState.inst.SetAsSelectedButton(CloseLicenseButton);
    }

    public void HideLisence() {
        Reader.Open();
        LicenseCanvas.enabled = false;
        UIState.inst.SetAsSelectedButton(CloseMusicButton);
    }

    public void ShowMusicCredits() {
        MusicCredits.enabled = true;
        DartSticker.inst.SetVisible(false);
        UIState.inst.SetAsSelectedButton(CloseMusicButton);
    }

    public void HideMusicCredits() {
        MusicCredits.enabled = false;
        UIState.inst.SetAsSelectedButton(FirstSelectedButton);
    }

    public void SelectButton()
    {
        Audio.inst.PlayClip(AudioClips.Click);
        LeftFill.SetCurrentImageToFill(Left);
        RightFill.SetCurrentImageToFill(Right, Right.transform.position+Offset);
    }

    public void UnSelectButton()
    {
        LeftFill.SetCurrentImageToFill(Hidden0);
        RightFill.SetCurrentImageToFill(Hidden1);
    }


    public void ReturnToMain()
    {
        TransitionManager.inst.GoToScene(SceneNumbers.MainMenu);
    }

    public void Ping()
    {
        foreach (TextCycle t in TextCycles)
            t.Begin();
    }
}

using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour, Caller, SceneEntrance {
    [SerializeField] Color[] Colors;
    [SerializeField] int Index;
    [SerializeField] float Speed;
    [SerializeField] TMP_Text text;
    [SerializeField] Canvas MainMenueCanvas;
    [SerializeField] Vector3 ButtonOffset;
    [SerializeField] FileUI FileUI;
    [SerializeField] Fillable_Image[] ButtonImages;
    [SerializeField] GroupImageFill FillSection;

    [SerializeField] GameObject FirstSelected;
    public void HoverButton(int i)
    {
        Audio.inst.PlayClip(AudioClips.Click);
        FillSection.SetCurrentImageToFill(ButtonImages[i], (ButtonImages[i].Images[1].transform).position + ButtonOffset);
    }


    public void Start()
    {
        PauseMenu.inst.SetEnabled(false);
        Application.targetFrameRate = 60;
        TransitionManager.inst.ReadyToEnterScene(this);
    }

    public void EnterScene() {
        PauseMenu.inst.SetEnabled(false);
        UIState.inst.SetInteractable(true);
        Ping();
    }

    public void ShowOptions()
    {
        DartSticker.inst.SetVisible(false);
        MainMenueCanvas.enabled = false;
        OptionsMenu.inst.ShowOptions(this);
    }

    public void ShowCredits()
    {
        TransitionManager.inst.GoToScene(SceneNumbers.Credits);
    }

    public void PlayDarts()
    {
        TransitionManager.inst.GoToScene(SceneNumbers.Darts);
    }

    public void PlayGame() {
        FileUI.BeginShowLoadMenu(this);
    }

    public void SetLoadFileAndGoToStory(int index) {
        TransitionManager.inst.SetFileIndex(index);
        TransitionManager.inst.GoToScene(SceneNumbers.Story);
    }

    public void QuitGame() {
        Application.Quit();
    }

    private void Update()
    {
       text.color= Vector4.MoveTowards(text.color, Colors[Index], Speed*Time.deltaTime);
        if(Vector4.Distance(text.color, Colors[Index]) < 0.005)
        {
            Index++;
            if (Index >= Colors.Length)
                Index = 0;
        }

    }

    public void Ping()
    {
        MainMenueCanvas.enabled = true;
        UIState.inst.SetAsSelectedButton(FirstSelected);
    }

#if UNITY_EDITOR
    [SerializeField] Color __Color;
    [SerializeField] bool __ChangeDirection;
    [SerializeField] bool __setColor;


    void __swapDirection()
    {
        int newRightDirection = ButtonImages[0].Images[0].fillOrigin;
        int newLeftDirection = ButtonImages[0].Images[1].fillOrigin;

        for (int i = 0; i < ButtonImages.Length; i++)
        {
            ButtonImages[i].Images[0].fillOrigin = newLeftDirection;
            ButtonImages[i].Images[1].fillOrigin = newRightDirection;
        }
    }

    private void OnValidate()
    {
        if (__ChangeDirection)
        {
            __ChangeDirection = false;
            __swapDirection();

        }

        if (__setColor)
        {
            __setColor = false;
            for (int i = 0; i < ButtonImages.Length; i++)
            {
                ButtonImages[i].Images[0].color = __Color;
                ButtonImages[i].Images[1].color = __Color;
            }
        }
    }
#endif
}

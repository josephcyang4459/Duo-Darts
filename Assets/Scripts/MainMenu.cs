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
    [SerializeField] Image[] ButtonImagesLeft;
    [SerializeField] Image[] ButtonImagesRight;
    [SerializeField] ImageFill FillSectionLeft;
    [SerializeField] ImageFill FillSectionRight;

    [SerializeField] GameObject FirstSelected;
    public void HoverButton(int i)
    {
        Audio.inst.PlayClip(AudioClips.Click);
        FillSectionLeft.SetCurrentImageToFill(ButtonImagesLeft[i], ((RectTransform)ButtonImagesLeft[i].transform).position + ButtonOffset);
        FillSectionRight.SetCurrentImageToFill(ButtonImagesRight[i]);
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
        int newRightDirection = ButtonImagesLeft[0].fillOrigin;
        int newLeftDirection = ButtonImagesRight[0].fillOrigin;

        for (int i = 0; i < ButtonImagesLeft.Length; i++)
        {
            ButtonImagesLeft[i].fillOrigin = newLeftDirection;
        }
        for (int i = 0; i < ButtonImagesRight.Length; i++)
        {
            ButtonImagesRight[i].fillOrigin = newRightDirection;
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
            for (int i = 0; i < ButtonImagesLeft.Length; i++)
            {
                ButtonImagesLeft[i].color = __Color;
            }
            for (int i = 0; i < ButtonImagesRight.Length; i++)
            {
                ButtonImagesRight[i].color = __Color;
            }
        }
    }
#endif
}

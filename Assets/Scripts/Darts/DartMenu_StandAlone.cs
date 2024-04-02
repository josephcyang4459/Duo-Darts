using UnityEngine;
using UnityEngine.UI;

public class DartMenu_StandAlone : MonoBehaviour, Caller, SceneEntrance
{
    [SerializeField] CharacterList Characters;
    [SerializeField] DartGame DartGame;
    [SerializeField] ImageFill Fill;
    [SerializeField] ImageSlide Slide;
    [SerializeField] ImageSmoothFill[] BorderFills;
    [SerializeField] UIAnimationElement ScoreAnimationEnterHead;
    [SerializeField] UIAnimationElement ScoreAnimationLeaveHead;
    [SerializeField] Canvas PartnerCanvas;
    [SerializeField] Canvas ScoreCanvas;
    [SerializeField] Image[] PartnerButtonImages;
    [SerializeField] Vector3 PartnerLocationOffset;
    [SerializeField] Image[] ScoreButtonImages;
    [SerializeField] Vector3 ScoreLocationOffset;
    [SerializeField] Image Portrait;
    [SerializeField] int PartnerIndex;
    [SerializeField] bool TurnOffScoreCanvas;
    [SerializeField] GameObject FirstPartnerButton;
    [SerializeField] GameObject FirstScoreButton;
    public void Start()
    {
        TransitionManager.inst.ReadyToEnterScene(this);
      
    }

    public void EnterScene() {
        PauseMenu.inst.SetEnabled(true);
        BeginSetUp();
    }

    public void BeginSetUp()
    {
        Audio.inst.StopSong();
        PartnerCanvas.enabled = true;
        //EventSystem.current.enabled = true;
        UIState.inst.SetInteractable(true);
        UIState.inst.SetAsSelectedButton(FirstPartnerButton);
    }

    public void StartGameWithPartner(int i)
    {
        PartnerIndex = i;
        ScoreCanvas.enabled = true;
        ScoreAnimationEnterHead.Begin(this);
        DartSticker.inst.SetVisible(false);
        Fill.ClearImages();
        TurnOffScoreCanvas = false;
    }

    public void ShowCharacterPortrait(int i)
    {
        if (Portrait.sprite == Characters.list[i].Expressions[0])
            return;
        Audio.inst.PlayClip(AudioClips.Click);
        Portrait.sprite = Characters.list[i].Expressions[0];
        Slide.BeginSlide();
        Fill.SetCurrentImageToFill(PartnerButtonImages[i] , ((RectTransform)PartnerButtonImages[i].transform).position + PartnerLocationOffset);

        float fill = (float)i / (float)(Characters.list.Length - 2);
        foreach (ImageSmoothFill image in BorderFills)
        {
            image.FillTo(fill);
        }
    }

    public void HoverScoreButton(int i)
    {
        Audio.inst.PlayClip(AudioClips.Click);
        Fill.SetCurrentImageToFill(ScoreButtonImages[i], ((RectTransform)ScoreButtonImages[i].transform).position + ScoreLocationOffset);
    }

    public void BackToPartnerScreen()
    {
        Fill.ClearImages();
        DartSticker.inst.SetVisible(false);
        TurnOffScoreCanvas = true;
        ScoreAnimationLeaveHead.Begin(this);
    }

    public void SetScore(int i)
    {
        DartGame.ScoreNeededToWin = (i == 0 ? 501 : 701);
        PartnerCanvas.enabled = false;
        ScoreCanvas.enabled = false;
        ScoreAnimationLeaveHead.ReachEndState();
        UIState.inst.SetInteractable(false);
        DartSticker.inst.SetVisible(false);
        DartGame.BeginGame(PartnerIndex);
        
    }
    public void Ping()
    {
        if (TurnOffScoreCanvas)
        {
            ScoreCanvas.enabled = false;
            UIState.inst.SetAsSelectedButton(FirstPartnerButton);
        }
        else
            UIState.inst.SetAsSelectedButton(FirstScoreButton);
    }

#if UNITY_EDITOR
    [SerializeField] Transform __buttonHolder;
    [SerializeField] bool __reset;

    private void OnValidate()
    {
        if (__reset)
        {
            __reset = false;
            TMPro.TMP_Text[] temp = __buttonHolder.GetComponentsInChildren<TMPro.TMP_Text>();
            PartnerButtonImages = new Image[temp.Length];
            for(int i = 0; i < temp.Length; i++)
            {
                temp[i].text = Characters.list[i].Name;
                PartnerButtonImages[i] = temp[i].gameObject.transform.parent.GetComponent<Image>();
            }
        }
    }

   

#endif

}


using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class AchievementUI : MonoBehaviour, ButtonEventReceiver
{
    [SerializeField] AccomplishmentsUI Accomplishents;
    [SerializeField] ButtonEventPassThrough[] Buttons;
    [SerializeField] Image[] ButtonFills;
    [SerializeField] Image[] AchivementIcons;
    [SerializeField] GameObject[] ButtonGameObjects;
    [SerializeField] Canvas AchivementCanvas;
    [SerializeField] ImageFill Fill;
    [SerializeField] Canvas DescriptionCanvas;
    [SerializeField] Image AchievementImage;
    [SerializeField] TMP_Text Name;
    [SerializeField] TMP_Text Description;
    [SerializeField] InputActionReference Back;
    [SerializeField] CurrentUIState State;
    [SerializeField] GameObject CloseButton;
    [SerializeField] int LastSelected;
    public void __SaveCompletion() {
        Achievements.Instance.SaveLocalToDisk();
    }


    public void Begin() {
       
        State = CurrentUIState.Full;
        Back.action.Enable();
        Back.action.performed += BackFunction;
        AchivementCanvas.enabled = true;
        Achievement[] achievements = Achievements.Instance.GetAllAchivements();
        for (int i = 0; i < Buttons.Length; i++) {
            AchivementIcons[i].sprite = achievements[i].GetCorrectIcon();
            Buttons[i].SetReciever(this, i);
        }
        UIState.inst.SetAsSelectedButton(ButtonGameObjects[0]);
    }

    public void End() {
        Fill.ClearImages();
        Accomplishents.End();
        DescriptionCanvas.enabled = false;
        AchivementCanvas.enabled = false;
        Back.action.Disable();
        Back.action.performed -= BackFunction;
    }

    public void BackFunction(InputAction.CallbackContext c) {
        switch (State) {
            case CurrentUIState.Full:
                End();
                return;
            case CurrentUIState.Description: 
              
                return;
        }
    }

    public void CloseDescriptionUI() {
        DescriptionCanvas.enabled = false;
        State = CurrentUIState.Full;
        UIState.inst.SetAsSelectedButton(ButtonGameObjects[LastSelected]);
    }

    public void ButtonClick(int i) {
        LastSelected =i;
        UIState.inst.SetAsSelectedButton(CloseButton);
        Fill.ClearImages();
        State = CurrentUIState.Description;
        DescriptionCanvas.enabled = true;
        Achievement a = Achievements.Instance.GetAllAchivements()[i];
        AchievementImage.sprite = a.GetCorrectIcon();
        Name.text = a.GetDisplayName();
        Description.text = a.GetDescription();
        //  throw new System.NotImplementedException();
    }

    public void Deselect(int i) {
       // throw new System.NotImplementedException();
    }

    public void PointerEnter(int i) {
        Fill.SetCurrentImageToFill(ButtonFills[i]);
    }

    public void PointerExit(int i) {
       // throw new System.NotImplementedException();
    }

    public void Select(int i) {
        Fill.SetCurrentImageToFill(ButtonFills[i]);
    }

    enum CurrentUIState {
        Full,
        Description,
    }

    private void OnDestroy() {
        End();
    }

#if UNITY_EDITOR
    [SerializeField] bool __SpawnButtons;
    [SerializeField] GameObject __ButtonPrefab;
    [SerializeField] Transform __holder;
    [SerializeField] Achievements __oops;
    [SerializeField] int __colomnsInGrid;
    private void OnValidate() {
        if (__SpawnButtons) {
            __SpawnButtons = false;
            Achievement[] ach = __oops.GetAllAchivements();
            Buttons = new ButtonEventPassThrough[ach.Length];
            ButtonFills = new Image[ach.Length];
            AchivementIcons = new Image[ach.Length];
            ButtonGameObjects = new GameObject[ach.Length];
            for(int i = __holder.childCount - 1; i >= 0; i--) {
                StartCoroutine(__HELPER_FUNCTIONS.__delete_gameObject(__holder.GetChild(i).gameObject));
            }
            for(int i = 0; i < ach.Length; i++) {
                GameObject g = Instantiate(__ButtonPrefab, __holder);
                Buttons[i] = g.GetComponent<ButtonEventPassThrough>();
                ButtonFills[i] = g.GetComponent<Image>();
                AchivementIcons[i] = g.transform.GetChild(0).GetComponent<Image>();
                ButtonGameObjects[i] = ButtonFills[i].GetComponentInChildren<Button>().gameObject;
            }

            for (int i = 0; i < ach.Length; i++) {
                Navigation temp = new Navigation();
                temp.mode = Navigation.Mode.Explicit;
                if (i > 0)
                    temp.selectOnLeft = ButtonGameObjects[i - 1].GetComponent<Button>();
                if (i + 1 < ach.Length)
                    temp.selectOnRight = ButtonGameObjects[i + 1].GetComponent<Button>();
                if (i >= __colomnsInGrid)
                    temp.selectOnUp = ButtonGameObjects[i - __colomnsInGrid].GetComponent<Button>();
                if (i + __colomnsInGrid < ach.Length)
                    temp.selectOnDown = ButtonGameObjects[i + __colomnsInGrid].GetComponent<Button>();
                ButtonGameObjects[i].GetComponent<Button>().navigation = temp;
            }
           
        }
    }

#endif
}

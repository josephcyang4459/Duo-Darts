using UnityEngine.InputSystem;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CutsceneLog : MonoBehaviour
{
    [SerializeField] float ScrollSpeed;
    [SerializeField] InputActionReference OpenLog;
    [SerializeField] InputActionReference ScrollLog;
    [SerializeField] Transform Position;
    [SerializeField] Canvas LogCanvas;
    [SerializeField] GameObject[] LogEntry;
    [SerializeField] TMP_Text[] NameTexts;
    [SerializeField] TMP_Text[] MessageTexts;
    [SerializeField] RectTransform Mask;
    [SerializeField] float StartY;
    [SerializeField] float YMin;
    [SerializeField] float YMax;
    [SerializeField] float MaxMultiplier;
    [SerializeField] Image UpArrow;
    [SerializeField] Image DownArrow;
    [SerializeField] int NumberOfLogsOnOneScreen;
    public bool State = false;
    [SerializeField] int CurrentLog;
    [SerializeField] int Direction;
    [SerializeField] Vector3 CachePosition;

    public void ResetLog() {
        State = false;

        for (int i = 0; i < CurrentLog; i++) {
            LogEntry[i].SetActive(false);
        }
        CurrentLog = 0;
    }

    public void EnableLog() {
        OpenLog.action.Enable();
        OpenLog.action.performed += Open;
    }

    public void UnenableLog() {
        OpenLog.action.Disable();
        OpenLog.action.performed -= Open;
    }

    void EnableScroll() {
        ScrollLog.action.Enable();
        ScrollLog.action.performed += Scroll;
        ScrollLog.action.canceled += Scroll;
    }

    void UnenableScroll() {
        ScrollLog.action.Disable();
        ScrollLog.action.performed -= Scroll;
        ScrollLog.action.canceled -= Scroll;
    }

    void Open(InputAction.CallbackContext c) {
        State = !State;
        LogCanvas.enabled = State;
        if (State) {
            YMax = YMin + (MaxMultiplier * CurrentLog) - Mask.rect.height;
            if (YMax < YMin)
                YMax = YMin;
            CachePosition.y = YMax;
            Position.position = CachePosition;
            UpArrow.enabled = (CachePosition.y != YMax);
            DownArrow.enabled = (CachePosition.y != YMin);
            EnableScroll();
        }
        else {
            UnenableScroll();
        }
    }

    void Scroll(InputAction.CallbackContext c) {
        Direction = Mathf.RoundToInt(c.ReadValue<Vector2>().y);
    }

    public void SetLog(string characterName, string message) {
        NameTexts[CurrentLog].text = characterName;
        MessageTexts[CurrentLog].text = message;
        LogEntry[CurrentLog].SetActive(true);
        CurrentLog++;
        YMax = YMin + (MaxMultiplier * CurrentLog) - Mask.rect.height;
        if (YMax < YMin)
            YMax = YMin;
    }

    public void Update() {
        if (Direction != 0) {
            CachePosition.y += Direction * ScrollSpeed * Time.deltaTime;
            CachePosition.y = Mathf.Clamp(CachePosition.y, YMin, YMax);
            Position.position = CachePosition;
            UpArrow.enabled = (CachePosition.y != YMax);
            DownArrow.enabled = (CachePosition.y != YMin);
        }
    }
#if UNITY_EDITOR
    [SerializeField] bool __reset;

    private void OnValidate() {

        if (__reset) {
         
            __reset = false;
            LogEntry = new GameObject[Position.childCount];
            NameTexts = new TMP_Text[Position.childCount];
            MessageTexts = new TMP_Text[Position.childCount];
            LogCanvas.enabled = true;
            CachePosition = Position.position;
            YMax = Position.position.y;
            YMin = Position.position.y;
           
            for (int i = 0; i < Position.childCount; i++) {
                LogEntry[i] = Position.GetChild(i).gameObject;
                LogEntry[i].SetActive(false);
                NameTexts[i] = LogEntry[i].transform.GetChild(0).GetComponent<TMP_Text>();
                MessageTexts[i] = LogEntry[i].transform.GetChild(1).GetComponent<TMP_Text>();
            }

            Debug.Log(Vector3.Distance(LogEntry[0].transform.position, LogEntry[1].transform.position));

            LogCanvas.enabled = false;
        }
    }

#endif
}

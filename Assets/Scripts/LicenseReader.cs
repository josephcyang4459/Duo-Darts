using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class LicenseReader : MonoBehaviour
{
    [SerializeField] float ScrollSpeed;
    [SerializeField] float StartY;
    [SerializeField] Transform StartPosition;
    [SerializeField] float YMin;
    [SerializeField] float YMax;
    [SerializeField] Transform Position;
    [SerializeField] Canvas LogCanvas;
    [SerializeField] InputActionReference ScrollLog;
    [SerializeField] int Direction;
    [SerializeField] Image UpArrow;
    [SerializeField] Image DownArrow;
    [SerializeField] Vector3 CachePosition;
    public bool State = false;

    void EnableScroll() {
        enabled = true;
        ScrollLog.action.Enable();
        ScrollLog.action.performed += Scroll;
        ScrollLog.action.canceled += Scroll;
    }

    void UnenableScroll() {
        enabled = false;
        ScrollLog.action.Disable();
        ScrollLog.action.performed -= Scroll;
        ScrollLog.action.canceled -= Scroll;
    }

    void Scroll(InputAction.CallbackContext c) {
        Direction = Mathf.RoundToInt(c.ReadValue<Vector2>().y);
    }

    public void Open() {
        State = !State;
        LogCanvas.enabled = State;
        if (State) {
            YMin = StartPosition.position.y;
            CachePosition.x = StartPosition.position.x;
            CachePosition.y = StartPosition.position.y;
            Position.position = CachePosition;
            UpArrow.enabled = (CachePosition.y != YMax);
            DownArrow.enabled = (CachePosition.y != YMin);
            EnableScroll();
        }
        else {
            UnenableScroll();
        }
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
}

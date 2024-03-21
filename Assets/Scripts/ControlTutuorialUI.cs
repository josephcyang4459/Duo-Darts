using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlTutuorialUI : MonoBehaviour
{
    public static ControlTutuorialUI inst;
    public GameObject FullShow;
    public GameObject PartialShow;
    public GameObject[] UI_ControlGroups;
    public InputActionReference ShowHideInput;
    [SerializeField] bool Showing;
    [SerializeField] bool[] ShouldShow;
    private void Awake()
    {
        inst = this;
        Showing = true;
        ShowHideInput.action.Enable();
        ShowHideInput.action.performed += ShowHide;
    }

    public void ShowHide(InputAction.CallbackContext c)
    {
        Showing = !Showing;
        if(Showing)
        {
            FullShow.SetActive(true);
            PartialShow.SetActive(false);
            for (int i = 0; i < UI_ControlGroups.Length; i++)
                UI_ControlGroups[i].SetActive(ShouldShow[i]);
        }
        else
        {
            FullShow.SetActive(false);
            PartialShow.SetActive(true);
            foreach (GameObject g in UI_ControlGroups)
            {
                g.SetActive(false);
            }
        }
    }

    public void SetControl(int control, bool active)
    {
        UI_ControlGroups[control].SetActive(active);
        ShouldShow[control] = active;
    }

    public void OnDestroy()
    {
        ShowHideInput.action.Disable();
        ShowHideInput.action.performed += ShowHide;
    }

#if UNITY_EDITOR

    private void OnValidate()
    {
        ShouldShow = new bool[UI_ControlGroups.Length];
    }

#endif
}

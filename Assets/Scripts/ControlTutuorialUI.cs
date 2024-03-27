using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ControlTutuorialUI : MonoBehaviour
{
    public static ControlTutuorialUI inst;
    public GameObject FullShow;
    public GameObject PartialShow;
    public GameObject[] UI_ControlGroups;
    public InputActionReference ShowHideInput;
    [SerializeField] bool Showing = true;
    [SerializeField] bool[] ShouldShow;
    private void Awake()
    {
        inst = this;
        Showing = false;
        FullShow.SetActive(false);
        PartialShow.SetActive(true);
        ShowHideInput.action.Enable();
        ShowHideInput.action.performed += ShowHide;
    }

    void ShowHide(InputAction.CallbackContext c)
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
        if (Showing)
            UI_ControlGroups[control].SetActive(active);
        ShouldShow[control] = active;
    }

    void OnDestroy()
    {
        ShowHideInput.action.Disable();
        ShowHideInput.action.performed -= ShowHide;
    }

#if UNITY_EDITOR
    [SerializeField] float __Opacity;
    [SerializeField] bool __setOpacity;
    private void OnValidate()
    {
        ShouldShow = new bool[UI_ControlGroups.Length];
        if (__setOpacity)
        {
            __setOpacity = false;
            __setOpacityF();
        }
    }
   
    void __setOpacityF()
    {
        __setOpacityForC(FullShow);
        foreach (GameObject g in UI_ControlGroups)
            __setOpacityForC(g);
    }

    void __setOpacityForC(GameObject g)
    {
        Image[] temp = g.GetComponentsInChildren<Image>();
        foreach(Image i in temp)
        {
            Color c = i.color;
            c.a = __Opacity;
            i.color = c;
        }
        TMP_Text[] text = g.GetComponentsInChildren<TMP_Text>();
        foreach (TMP_Text t in text)
        {
            Color c = t.color;
            c.a = __Opacity;
            t.color = c;
        }
    }

#endif
}

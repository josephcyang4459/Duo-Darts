using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SecretUI : Accomplishment_SubMenu
{
    [SerializeField] Canvas c;
    [SerializeField] string CantUse = "?????";
    [SerializeField] Achievement ChadStory;
    [SerializeField] TMP_Text title;
    [SerializeField] Button OopsAllChadButton;
    [SerializeField] UIToggle OopsAllChadToggle;
    [SerializeField] string OopsAllChadTitle = "Oops All Chad";

    public override void Begin() {
        c.enabled = true;
        Back.action.Enable();
        Back.action.performed += BackFunction;
        if (ChadStory.IsComplete()) {
            OopsAllChadButton.interactable = true;
            title.text = OopsAllChadTitle;
        }
        else {
            OopsAllChadButton.interactable = false;
            title.text = CantUse;
        }
        UIState.inst.SetAsSelectedButton(OopsAllChadButton.gameObject);
    }

    public void SetOopsAllChad() {
        Achievements.Instance.SetAllChad(OopsAllChadToggle.GetState());
    }

    public override void End() {
        c.enabled = false;
        Back.action.Disable();
        Back.action.performed -= BackFunction;
    }

    public void BackFunction(InputAction.CallbackContext c) {
        End();
        Accomplishents.End();
    }

}

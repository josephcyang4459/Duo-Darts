using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DartPartnerStoryUI : MonoBehaviour
{
    [SerializeField] bool Active;
    [SerializeField] bool UsingController;
    [SerializeField] ColorSwatch KeyColors;
    [SerializeField] CharacterList Partners;
    [SerializeField] ImageFill Fill;
    [SerializeField] Vector2 Corner;
    [SerializeField] Image[] ButtonFills;
    [SerializeField] GameObject[] PlayerButtons;
    [SerializeField] Image[] PlayerColor;
    [SerializeField] TMP_Text[] PlayerTexts;

    [SerializeField] float[] DistanceRings;
    [SerializeField] Mouse Mouse;
    [SerializeField] int CurrentRing;

    private void Start() {
        SetActive(true);
    }

    int DistanceFromCorner() {
       
        float distance = Vector2.Distance(Mouse.position.ReadValue(), Corner);
        for (int i = 0; i < DistanceRings.Length; i++) {
            if (distance < DistanceRings[i])
                return i;
        }
        return DistanceRings.Length - 1;
    }

    void IsUsingController(bool b) {
        UsingController = b;
    }

    void SetActive(bool state) {
        enabled = state;
        if (state) {
            Mouse = Mouse.current;
        }
    }

    public void SelectButton(int index) {

    }

    private void FixedUpdate() {
        if (!UsingController) {
            int temp = DistanceFromCorner();
            if (temp != CurrentRing) {
                CurrentRing = temp;
                Fill.SetCurrentImageToFill(ButtonFills[temp]);
            }
        }
    }

#if UNITY_EDITOR
    [SerializeField] bool __set;
    [SerializeField] Transform __Corner;
    private void OnValidate() {
        if (__set) {
            __set = false;
            Corner = __Corner.position;
        }
    }
#endif
}

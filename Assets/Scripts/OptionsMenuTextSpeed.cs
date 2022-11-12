using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuTextSpeed : MonoBehaviour
{
    public Slider Slider;
    public TMP_Text Text;

    public void Update()
    {
        Text.text = Slider.value.ToString();
    }
}

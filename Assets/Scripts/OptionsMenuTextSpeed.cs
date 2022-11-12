using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuTextSpeed : MonoBehaviour
{
    public Slider Slider;
    public TextMeshProUGUI Text;

    public void Update()
    {
        Text.text = Slider.value.ToString();
    }
}

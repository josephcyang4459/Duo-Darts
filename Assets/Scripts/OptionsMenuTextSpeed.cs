using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuTextSpeed : MonoBehaviour
{
    public Slider Slider;
    public TMP_Text Text;

    public Slider volume_slider;
    public TMP_Text volume_text;

    public void volume_change()
    {
        volume_text.text = volume_slider.value.ToString();
        PlayerPrefs.SetFloat("volume", volume_slider.value);
    }

    public void textSpeed_change()
    {
        Text.text = Slider.value.ToString();
        PlayerPrefs.SetFloat("textSpeed", Slider.value);
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public static OptionsMenu inst;
    [SerializeField] Canvas Canvas;
    public Slider TextSpeedSlider;
    public TMP_Text TextSpeedText;

    public Slider VolumeSlider;
    public TMP_Text VolumeText;
    [SerializeField] Caller Caller;
    [SerializeField] AudioClip Clip;

    public delegate void SettingChange(float value);
    public static event SettingChange VolumeChange;
    public static event SettingChange TextSpeedChange;

    private void Start()
    {
        if (inst != null)
        {
            Destroy(this);
            return;
        }
           
        DontDestroyOnLoad(this);
        inst = this;
        VolumeSlider.value = PlayerPrefs.GetFloat("volume", .5f)*10;
        TextSpeedSlider.value = PlayerPrefs.GetFloat("textSpeed", 10);
        VolumeText.text = VolumeSlider.value.ToString();
        TextSpeedText.text = TextSpeedSlider.value.ToString();
    }

    public void ShowOptions(Caller caller)
    {
        Caller = caller;
        Canvas.enabled = true;
    }

    public void HideOptions()
    {
        if (!Canvas.enabled)
            return;
        Canvas.enabled = false;
        if (Caller != null)
        {
            Caller.Ping();
            Caller = null;
        }
      
    }

    public void VolumeSliderFunction()
    {
        VolumeText.text = VolumeSlider.value.ToString();
        float realVolume = VolumeSlider.value / 10f;
        PlayerPrefs.SetFloat("volume", realVolume);
        Audio.inst.PlayClip(Clip);
        VolumeChange(realVolume);
    }

    public void TextSpeedSliderFunction()
    {
        TextSpeedText.text = TextSpeedSlider.value.ToString();
        PlayerPrefs.SetFloat("textSpeed", TextSpeedSlider.value);
        Audio.inst.PlayClip(Clip);
        TextSpeedChange(TextSpeedSlider.value);
    }
}

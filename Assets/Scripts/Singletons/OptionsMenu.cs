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

    public Slider SFXVolumeSlider;
    public TMP_Text SFXVolumeText;


    [SerializeField] UIToggle[] Toggles;

    [SerializeField] Caller Caller;

    [SerializeField] GameObject FirstSelected;
    [SerializeField] GameObject ReturnButton;
    public delegate void SettingChange(float value);
    public static event SettingChange VolumeChange;
    public static event SettingChange SFXVolumeChange;
    public static event SettingChange TextSpeedChange;
    [SerializeField] bool SetUp;
    private void Start(){
        if (inst != null){
            Destroy(gameObject);
            return;
        }
        SetUp = true;
        inst = this;
        DontDestroyOnLoad(this);

        VolumeSlider.value = PlayerPrefs.GetFloat("volume", .5f)*10;
        SFXVolumeSlider.value = PlayerPrefs.GetFloat("sfx_volume", .5f) * 10;
        TextSpeedSlider.value = PlayerPrefs.GetFloat("textSpeed", 5);
        VolumeText.text = VolumeSlider.value.ToString();
        VolumeChange(VolumeSlider.value / 10);
        TextSpeedText.text = TextSpeedSlider.value.ToString();
        TextSpeedChange(TextSpeedSlider.value);
        ControllerOptionsFunction(PlayerPrefs.GetInt("controller", 0));
        SetUp = false;
    }

    public void ShowOptions(Caller caller){
        Caller = caller;
        Canvas.enabled = true;
        DartSticker.inst.SetVisible(false);
        UIState.inst.SetAsSelectedButton(FirstSelected);
    }

    public void HideOptionsNoCall() {
        Caller = null;
        HideOptions();
    }

    public void HideOptions(){
        if (!Canvas.enabled)
            return;
        PlayerPrefs.Save();
        Canvas.enabled = false;
        if (Caller != null)
        {
            Caller.Ping();
            Caller = null;
        }
      
    }

    public void VolumeSliderFunction(){
        VolumeText.text = VolumeSlider.value.ToString();
        float realVolume = VolumeSlider.value / 10f;
        PlayerPrefs.SetFloat("volume", realVolume);
        if (!SetUp)
            Audio.inst.PlayClip(AudioClips.Click);
        VolumeChange(realVolume);
    }

    public void SFXVolumeSliderFunction() {
        SFXVolumeText.text = SFXVolumeSlider.value.ToString();
        float realVolume = SFXVolumeSlider.value / 10f;
        PlayerPrefs.SetFloat("sfx_volume", realVolume);
        if (!SetUp)
            Audio.inst.PlayClip(AudioClips.Click);
        SFXVolumeChange(realVolume);
    }

    public void TextSpeedSliderFunction(){
        TextSpeedText.text = TextSpeedSlider.value.ToString();
        PlayerPrefs.SetFloat("textSpeed", TextSpeedSlider.value);
        if (!SetUp)
            Audio.inst.PlayClip(AudioClips.Click);
        TextSpeedChange(TextSpeedSlider.value);
    }

    public void ControllerOptionsFunction(int index){
        for (int i = 0; i < 3; i++) {
            Toggles[i].ChangeStateNoInvoke(i == index);
            Toggles[i].SetInteractable(i != index);
        }
      if(!SetUp)
        Audio.inst.PlayClip(AudioClips.Click);
        PlayerPrefs.SetInt("controller", index);
        switch (index){
            case 0: ControlState.inst.SetControlState(ControllerState.UseIfConnected);break;
            case 1: ControlState.inst.SetControlState(ControllerState.ForceKeyboard);break;
            case 2: ControlState.inst.SetControlState(ControllerState.ForceController);break;
        }
        UIState.inst.SetAsSelectedButton(ReturnButton);
    }
}

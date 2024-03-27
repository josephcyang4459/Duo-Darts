using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour, Caller {

    [SerializeField] GameObject button;
    [SerializeField] AudioClip HoverClip;
    [SerializeField] AudioClip ClickClip;
    [SerializeField] Color[] Colors;
    [SerializeField] int Index;
    [SerializeField] float Speed;
    [SerializeField] TMP_Text text;
    [SerializeField] Canvas MainMenueCanvas;

    public void PlayHoverClip()
    {
        Audio.inst.PlayClip(HoverClip);
    }

    public void PlayClickClip()
    {
        Audio.inst.PlayClip(ClickClip);
    }

    public void Start()
    {
        PauseMenu.inst.SetEnabled(false);
        Application.targetFrameRate = 60;
        //UI_Helper.SetSelectedUIElement(button);
    }
    
    public void ShowOptions()
    {
        MainMenueCanvas.enabled = false;
        OptionsMenu.inst.ShowOptions(this);
    }

    public void ShowCredits()
    {
        
        SceneManager.LoadScene((int)SceneNumbers.Credits);
    }

    public void PlayGame() {
        PlayerPrefs.Save();
        System.GC.Collect();
        SceneManager.LoadScene((int)SceneNumbers.Game);
    }

    public void QuitGame() {
        Application.Quit();
    }

    private void Update()
    {
       text.color= Vector4.MoveTowards(text.color, Colors[Index], Speed*Time.deltaTime);
        if(Vector4.Distance(text.color, Colors[Index]) < 0.005)
        {
            Index++;
            if (Index >= Colors.Length)
                Index = 0;
        }

    }

    public void Ping()
    {
        MainMenueCanvas.enabled = true;
    }
}

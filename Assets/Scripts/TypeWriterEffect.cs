using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TypeWriterEffect : MonoBehaviour {
    [SerializeField] private bool UseCustomSpeed;
    [SerializeField] float CustomWriteSpeed = 10f;
    [SerializeField] TMP_Text Textlabel;
    [SerializeField] string TextToType;
    public bool Writing;
    [SerializeField] float CurrentTime;
    [SerializeField] int CharIndex;

    public void Awake()
    {
        OptionsMenu.TextSpeedChange += TextSpeedChange; 
    }

    public void TextSpeedChange(float value)
    {
        CustomWriteSpeed = 5/value;
    }

    public void Run(string textToType, TMP_Text textLabel) {
        Writing = true;//maybe check if there is already an active coroutine here because my god was it fucked up
        TextToType = textToType;
        Textlabel = textLabel;
        Textlabel.text = string.Empty;
        CurrentTime = 0;
        CharIndex = 0;
        enabled = true;
    }

    public void Stop()
    {
        enabled = false;
        Writing = false;
        Textlabel.text = TextToType;

    }

    void FixedUpdate() {

        if (TextToType[CharIndex] == ' ')
            CurrentTime += .02f * 4;
        else
            CurrentTime += .02f;

        if (CurrentTime >= CustomWriteSpeed) {
            CharIndex++;
            Textlabel.text = TextToType.Substring(0, CharIndex);

            if (CharIndex >= TextToType.Length) {
                Stop();
            }
        }
        
    
    }
}
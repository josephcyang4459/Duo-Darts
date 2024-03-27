using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TypeWriterEffect : MonoBehaviour {
    [SerializeField] private bool UseCustomSpeed;
    public float CustomWriteSpeed = 10f;
    [SerializeField] private Slider TextSpeedSlider;
    public TMP_Text textlable;
    public string ttt;
    public Coroutine typer;
    public bool writing;

    public void Start()
    {
        OptionsMenu.TextSpeedChange += TextSpeedChange; 
    }

    public void TextSpeedChange(float value)
    {
        CustomWriteSpeed = value;
    }

    public void Run(string TextToType, TMP_Text textLabel) {
        writing = true;//maybe check if there is already an active coroutine here because my god was it fucked up
        typer = StartCoroutine(TypeText(TextToType, textLabel));
    }

    public void Stop()
    {
        if (typer != null)
            StopAllCoroutines();
        writing = false;
        typer = null;
        textlable.text = ttt;

    }

    private IEnumerator TypeText(string TextToType, TMP_Text textLabel) {
      
        textlable = textLabel;
        ttt = TextToType;
        float t = 0;
        int charIndex = 0;

        while (charIndex < TextToType.Length) {
            if (!UseCustomSpeed)
                CustomWriteSpeed = TextSpeedSlider.value;

            if (TextToType[charIndex] == ' ')
                t += Time.deltaTime * CustomWriteSpeed * 4;
            else
                t += Time.deltaTime * CustomWriteSpeed;


            charIndex = Mathf.FloorToInt(t);
            charIndex = Mathf.Clamp(charIndex, 0, TextToType.Length);

            textLabel.text = TextToType.Substring(0, charIndex);

            yield return null;
        }
        
        typer = null;
        writing = false;
    }
}
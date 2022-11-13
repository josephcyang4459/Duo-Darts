using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TypeWriterEffect : MonoBehaviour {
    [SerializeField] private bool UseCustomSpeed;
    [SerializeField] private float CustomWriteSpeed = 10f;
    [SerializeField] private Slider TextSpeedSlider;
    public TMP_Text textlable;
    public string ttt;
    public Coroutine typer;

    public void Run(string TextToType, TMP_Text textLabel) {
        typer = StartCoroutine(TypeText(TextToType, textLabel));
    }

    public void Stop()
    {
        if (typer != null)
            StopCoroutine(typer);
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
    }
}
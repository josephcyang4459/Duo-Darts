using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TypeWriterEffect : MonoBehaviour {
    [SerializeField] private bool UseCustomSpeed;
    [SerializeField] private float CustomWriteSpeed = 10f;
    [SerializeField] private Slider TextSpeedSlider;

    public void Run(string TextToType, TMP_Text textLabel) {
        StartCoroutine(TypeText(TextToType, textLabel));
    }

    private IEnumerator TypeText(string TextToType, TMP_Text textLabel) {
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
    }
}
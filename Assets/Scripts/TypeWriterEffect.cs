using UnityEngine;
using TMPro;

public class TypeWriterEffect : MonoBehaviour {
    [SerializeField] private bool UseCustomSpeed;
    [SerializeField] float CustomWriteSpeed = 10f;
    [SerializeField] TMP_Text Textlabel;
    [SerializeField] string TextToType;
    public bool Writing;
    [SerializeField] float CurrentTime;
    [SerializeField] int CharIndex;

    public void Awake() {
        OptionsMenu.TextSpeedChange += TextSpeedChange;
    }

    public void TextSpeedChange(float value) {
        CustomWriteSpeed = Mathf.Clamp(1 / (value * 10), .001f, .5f);
    }

    public void Run(string textToType, TMP_Text textLabel) {
        if (textToType == null)
            return;
        if (textToType.Length == 0)
            return;
        Writing = true;
        TextToType = textToType;

        Textlabel = textLabel;
        Textlabel.text = string.Empty;
        CurrentTime = 0;
        CharIndex = 0;
        enabled = true;
    }

    public void Stop() {
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
            CurrentTime = 0;
            Textlabel.text = TextToType.Substring(0, CharIndex);

            if (CharIndex >= TextToType.Length) {
                Stop();
            }
        }

    }
}
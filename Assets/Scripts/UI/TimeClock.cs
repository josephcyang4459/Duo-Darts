using UnityEngine;
using UnityEngine.UI;

public class TimeClock : MonoBehaviour
{
    [SerializeField] Canvas TimeCanvas;
    [SerializeField] Image HoursTens;
    [SerializeField] Image HoursOnes;
    [SerializeField] Image MinutesTens;
    [SerializeField] Image MinutesOnes;

    [SerializeField] SpriteCollection SevenSegmentSprites;
    [SerializeField] ColorSwatch TimeColors;

    public void SetVisible(bool state) {
        TimeCanvas.enabled = state;
    }

    public void SetTime(int hour, int minute) {
        HoursTens.sprite = SevenSegmentSprites.Sprites[hour < 10 ? 0 : 1];
        HoursOnes.sprite = SevenSegmentSprites.Sprites[hour % 10];

        MinutesTens.sprite = SevenSegmentSprites.Sprites[minute < 10 ? 0 : minute/10];
        MinutesOnes.sprite = SevenSegmentSprites.Sprites[minute % 10];
    }

#if UNITY_EDITOR
    [SerializeField] int __hour;
    [SerializeField] int __minute;
    [SerializeField] bool __setClock;
    private void OnValidate() {
        if (__setClock) {
            __setClock = false;
            SetTime(__hour, __minute);
        }
    }
#endif
}

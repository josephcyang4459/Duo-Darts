using UnityEngine;
using UnityEngine.UI;

public class TimeClock : MonoBehaviour
{
    [SerializeField] Canvas TimeCanvas;
    [SerializeField] Image HoursTens;
    [SerializeField] Image HoursOnes;
    [SerializeField] Image MinutesTens;
    [SerializeField] Image MinutesOnes;
    [SerializeField] Image[] AllSpritesToColor;
    [SerializeField] SpriteCollection SevenSegmentSprites;
    [SerializeField] ColorSwatch TimeColors;

    public void SetVisible(bool state) {
        TimeCanvas.enabled = state;
    }

    bool UseLateColor(int hour, int minute) {
        if (hour == 8)
            if (minute >= 30)
                return true;
        if (hour > 8)
            return true;
        return false;
    }

    public void SetTime(int hour, int minute) {
        HoursTens.sprite = SevenSegmentSprites.Sprites[hour < 10 ? 0 : 1];
        HoursOnes.sprite = SevenSegmentSprites.Sprites[hour % 10];

        MinutesTens.sprite = SevenSegmentSprites.Sprites[minute < 10 ? 0 : minute/10];
        MinutesOnes.sprite = SevenSegmentSprites.Sprites[minute % 10];
        Color color = TimeColors.colors[UseLateColor(hour, minute) ? 1 : 0];
        foreach(Image i in AllSpritesToColor) {
            i.color = color;
        }
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

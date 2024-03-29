using UnityEngine;
using TMPro;

public class TextCycle : MonoBehaviour
{
    [SerializeField] TMP_Text Text;
    [SerializeField] string[] Strings;
    [SerializeField] float TimerOnEachString;
    [SerializeField] float FadeSpeed;
    TextState State;
    int Index;
    float CurrentTime;

    public void SetText(int i)
    {
        Text.text = Strings[i];
    }

    public void Begin()
    {
        enabled = true;
        CurrentTime = 0;
        Index = 0;
        State = TextState.displaying;
    }

    void Displaying()
    {
        CurrentTime += Time.deltaTime;
        if (CurrentTime > TimerOnEachString)
        {
            State = TextState.fadeOut;
        }
    }

    void FadingOut()
    {
        Text.alpha = Mathf.MoveTowards(Text.alpha, 0, FadeSpeed * Time.deltaTime);
        if(Text.alpha == 0)
        {
            CurrentTime = 0;
            Index++;
            if (Index >= Strings.Length)
                Index = 0;
            Text.text = Strings[Index];
            State = TextState.fadeIn;
        }

    }

    void FadingIn()
    {
        Text.alpha = Mathf.MoveTowards(Text.alpha, 1, FadeSpeed * Time.deltaTime);
        if (Text.alpha == 1)
        {
            State = TextState.displaying;
        }
    }

    private void Update()
    {
        switch (State)
        {
            case TextState.displaying: Displaying();return;
            case TextState.fadeOut: FadingOut();return;
            case TextState.fadeIn: FadingIn(); return;
        }
    }
    enum TextState
    {
        displaying,
        fadeOut,
        fadeIn,
    }
}

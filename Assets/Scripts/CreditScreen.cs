using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreditScreen : MonoBehaviour, Caller
{

    [SerializeField] Vector3 Offset;

    [SerializeField] ImageFill LeftFill;
    [SerializeField] ImageFill RightFill;
    [SerializeField] Image Left;
    [SerializeField] Image Right;
    [SerializeField] Image Hidden0;
    [SerializeField] Image Hidden1;
    [SerializeField] UIAnimationElement AnimationHead;
    [SerializeField] GameObject FirstSelectedButton;
    [SerializeField] TextCycle[] TextCycles;

    public void Start()
    {
        UIState.inst.SetAsSelectedButton(FirstSelectedButton);
        foreach (TextCycle t in TextCycles)
            t.SetText(0);
        AnimationHead.Begin(this);
    }

    public void SelectButton()
    {
        Audio.inst.PlayClip(AudioClips.Click);
        LeftFill.SetCurrentImageToFill(Left);
        RightFill.SetCurrentImageToFill(Right, Right.transform.position+Offset);
    }

    public void UnSelectButton()
    {
        LeftFill.SetCurrentImageToFill(Hidden0);
        RightFill.SetCurrentImageToFill(Hidden1);
    }


    public void ReturnToMain()
    {
        SceneManager.LoadScene(0);
    }

    public void Ping()
    {
        foreach (TextCycle t in TextCycles)
            t.Begin();
    }
}

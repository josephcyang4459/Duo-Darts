using UnityEngine;
/// <summary>
/// Really specific code but i don't wanna make it better because I just want thios project to be over with honestly
/// </summary>
public class CharacterPortraitAnimation : MonoBehaviour
{
    [SerializeField] Transform Portrait;
    [SerializeField] Transform OriginalPosition;
    [SerializeField] AnimationCurve HappyCurve;
    [SerializeField] AnimationCurve NegativeCurve;
    [SerializeField] AnimationCurve DrunkCurve;
    [SerializeField] AnimationCurve CurrentCurve;
    [SerializeField] Expressions Expression;
    float Timer = 0;
    [SerializeField] Vector3 Cache;
    public void ChangeExpression(Expressions expression) {
        Expression = expression;
        switch (expression) {
            case Expressions.Nuetral:
                return;
            case Expressions.Positive: CurrentCurve = HappyCurve;break;
                     case Expressions.Negative:
                CurrentCurve = NegativeCurve; break;
            case Expressions.Drunk:
                CurrentCurve = DrunkCurve; break;
        }
        Timer = 0;
    
        enabled = true;
    }

    private void Update() {
        Timer += Time.deltaTime;
        Cache = OriginalPosition.position;
        if (Expression != Expressions.Drunk) {
            Cache.y += CurrentCurve.Evaluate(Timer);
        }
        else {
            Cache.x += CurrentCurve.Evaluate(Timer);
        }
        Portrait.position = Cache;
        if (Timer >= 1) {
            enabled = false;
        }
    }
}

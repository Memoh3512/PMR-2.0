using UnityEngine;

namespace PMR
{
    [CreateAssetMenu(fileName = "MyIntTimeCurve", menuName = "PMR/TimeCurves/Scripted/Vector2")]
    public class ScriptedTimeCurveVector2 : ScriptedTimeCurve<Vector2>
    {
        protected override Vector2 EvaluateValue(float delta)
        {
            return Vector2.Lerp(startValue, endValue, delta);
        }
    }
}
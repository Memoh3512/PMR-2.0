using UnityEngine;

namespace PMR
{
    [CreateAssetMenu(fileName = "MyIntTimeCurve", menuName = "PMR/TimeCurves/Vector2")]
    public class ValueTimeCurveVector2 : ValueTimeCurve<Vector2>
    {
        protected override Vector2 EvaluateValue(float delta)
        {
            return Vector2.Lerp(startValue, endValue, delta);
        }
    }
}


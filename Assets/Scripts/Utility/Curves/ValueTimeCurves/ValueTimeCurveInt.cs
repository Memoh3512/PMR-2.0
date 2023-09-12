using UnityEngine;

namespace PMR
{
    [CreateAssetMenu(fileName = "MyIntTimeCurve", menuName = "PMR/TimeCurves/Integer")]
    public class ValueTimeCurveInt : ValueTimeCurve<int>
    {
        protected override int EvaluateValue(float delta)
        {
            return (int)Mathf.Lerp(startValue, endValue, delta);
        }
    }
}


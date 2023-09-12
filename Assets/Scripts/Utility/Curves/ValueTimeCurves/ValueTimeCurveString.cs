using UnityEngine;

namespace PMR
{
    [CreateAssetMenu(fileName = "MyIntTimeCurve", menuName = "PMR/TimeCurves/String")]
    public class ValueTimeCurveString : ValueTimeCurve<string>
    {
        protected override string EvaluateValue(float delta)
        {
            int length = (int)Mathf.Lerp(startValue.Length, endValue.Length, delta);
            return endValue[..length];
        }
    }
}


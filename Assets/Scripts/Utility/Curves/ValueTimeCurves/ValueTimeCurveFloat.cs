using UnityEngine;

namespace PMR
{
    [CreateAssetMenu(fileName = "MyIntTimeCurve", menuName = "PMR/TimeCurves/Float")]
    public class ValueTimeCurveFloat : ValueTimeCurve<float>
    {
        protected override float EvaluateValue(float delta)
        {
            return Mathf.Lerp(startValue, endValue, delta);
        }
    }
}


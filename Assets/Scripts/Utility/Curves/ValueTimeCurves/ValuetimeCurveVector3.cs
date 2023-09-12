using UnityEngine;

namespace PMR
{
    [CreateAssetMenu(fileName = "MyIntTimeCurve", menuName = "PMR/TimeCurves/Vector3")]
    public class ValueTimeCurveVector3 : ValueTimeCurve<Vector3>
    {
        protected override Vector3 EvaluateValue(float delta)
        {
            return Vector3.Lerp(startValue, endValue, delta);
        }
    }
}


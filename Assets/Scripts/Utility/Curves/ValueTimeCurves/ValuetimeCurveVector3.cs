using System;
using UnityEngine;

namespace PMR
{
    [Serializable]
    public class ValueTimeCurveVector3 : ValueTimeCurve<Vector3>
    {
        public ValueTimeCurveVector3(TimeCurveData data) : base(data) {}

        protected override Vector3 EvaluateValue(float delta)
        {
            return Vector3.Lerp(startValue, endValue, delta);
        }
    }
}
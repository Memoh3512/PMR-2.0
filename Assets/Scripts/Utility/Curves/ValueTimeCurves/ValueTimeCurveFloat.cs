using System;
using UnityEngine;

namespace PMR
{
    [Serializable]
    public class ValueTimeCurveFloat : ValueTimeCurve<float>
    {
        public ValueTimeCurveFloat(TimeCurveData data) : base(data) {}

        protected override float EvaluateValue(float delta)
        {
            return Mathf.Lerp(startValue, endValue, delta);
        }
    }
}


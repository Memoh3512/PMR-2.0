using System;
using UnityEngine;

namespace PMR
{
    [Serializable]
    public class ValueTimeCurveInt : ValueTimeCurve<int>
    {
        public ValueTimeCurveInt(TimeCurveData data) : base(data) {}

        protected override int EvaluateValue(float delta)
        {
            return (int)Mathf.Lerp(startValue, endValue, delta);
        }
    }
}


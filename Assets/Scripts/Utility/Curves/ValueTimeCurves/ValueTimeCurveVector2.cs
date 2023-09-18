using System;
using UnityEngine;

namespace PMR
{
    [Serializable]
    public class ValueTimeCurveVector2 : ValueTimeCurve<Vector2>
    {
        public ValueTimeCurveVector2(TimeCurveData data) : base(data) {}

        protected override Vector2 EvaluateValue(float delta)
        {
            return Vector2.Lerp(startValue, endValue, delta);
        }
    }
}


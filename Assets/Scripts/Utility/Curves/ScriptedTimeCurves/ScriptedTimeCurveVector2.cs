using System;
using UnityEngine;

namespace PMR
{
    [Serializable]
    public class ScriptedTimeCurveVector2 : ScriptedTimeCurve<Vector2>
    {
        public ScriptedTimeCurveVector2(TimeCurveData data) : base(data) {}

        protected override Vector2 EvaluateValue(float delta)
        {
            return Vector2.Lerp(startValue, endValue, delta);
        }
    }
}
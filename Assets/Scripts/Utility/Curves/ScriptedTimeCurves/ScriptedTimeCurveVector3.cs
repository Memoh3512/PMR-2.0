using System;
using UnityEngine;

namespace PMR
{
    [Serializable]
    public class ScriptedTimeCurveVector3 : ScriptedTimeCurve<Vector3>
    {
        public ScriptedTimeCurveVector3(TimeCurveData data) : base(data) {}

        protected override Vector3 EvaluateValue(float delta)
        {
            return Vector3.Lerp(startValue, endValue, delta);
        }
    }
}
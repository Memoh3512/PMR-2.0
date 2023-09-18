using System;
using UnityEngine;

namespace PMR
{
    [Serializable]
    public class ScriptedTimeCurveString : ScriptedTimeCurve<string>
    {
        public ScriptedTimeCurveString(TimeCurveData data) : base(data) {}

        protected override string EvaluateValue(float delta)
        {
            int length = (int)Mathf.Lerp(startValue.Length, endValue.Length, delta);
            return endValue[..length];
        }
    }
}
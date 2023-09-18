using System;
using UnityEngine;

namespace PMR
{
    [Serializable]
    public class ScriptedTimeCurveInt : ScriptedTimeCurve<int>
    {
        public ScriptedTimeCurveInt(TimeCurveData data) : base(data) {}

        protected override int EvaluateValue(float delta)
        {
            return (int)Mathf.Lerp(startValue, endValue, delta);
        }
    }
}


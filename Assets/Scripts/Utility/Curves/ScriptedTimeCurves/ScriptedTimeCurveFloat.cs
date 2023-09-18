using System;
using UnityEngine;

namespace PMR
{
    [Serializable]
   public class ScriptedTimeCurveFloat : ScriptedTimeCurve<float>
   {
       public ScriptedTimeCurveFloat(TimeCurveData data) : base(data) {}

       protected override float EvaluateValue(float delta)
       {
           return Mathf.Lerp(startValue, endValue, delta);
       }
   }
}


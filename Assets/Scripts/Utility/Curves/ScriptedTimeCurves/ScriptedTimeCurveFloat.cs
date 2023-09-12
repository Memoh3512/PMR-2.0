using UnityEngine;

namespace PMR
{
   [CreateAssetMenu(fileName = "MyIntTimeCurve", menuName = "PMR/TimeCurves/Scripted/Float")]
   public class ScriptedTimeCurveFloat : ScriptedTimeCurve<float>
   {
       protected override float EvaluateValue(float delta)
       {
           return Mathf.Lerp(startValue, endValue, delta);
       }
   }
}


using UnityEngine;

namespace PMR
{
    [CreateAssetMenu(fileName = "MyIntTimeCurve", menuName = "PMR/TimeCurves/Scripted/String")]
    public class ScriptedTimeCurveString : ScriptedTimeCurve<string>
    {
        protected override string EvaluateValue(float delta)
        {
            int length = (int)Mathf.Lerp(startValue.Length, endValue.Length, delta);
            return endValue[..length];
        }
    }
}
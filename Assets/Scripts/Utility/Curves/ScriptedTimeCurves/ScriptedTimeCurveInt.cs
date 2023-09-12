using UnityEngine;

namespace PMR
{
    [CreateAssetMenu(fileName = "MyIntTimeCurve", menuName = "PMR/TimeCurves/Scripted/Integer")]
    public class ScriptedTimeCurveInt : ScriptedTimeCurve<int>
    {
        protected override int EvaluateValue(float delta)
        {
            return (int)Mathf.Lerp(startValue, endValue, delta);
        }
    }
}


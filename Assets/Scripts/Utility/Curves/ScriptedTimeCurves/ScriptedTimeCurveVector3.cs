using UnityEngine;

namespace PMR
{
    [CreateAssetMenu(fileName = "MyIntTimeCurve", menuName = "PMR/TimeCurves/Scripted/Vector3")]
    public class ScriptedTimeCurveVector3 : ScriptedTimeCurve<Vector3>
    {
        protected override Vector3 EvaluateValue(float delta)
        {
            return Vector3.Lerp(startValue, endValue, delta);
        }
    }
}
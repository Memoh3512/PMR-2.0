using UnityEngine;

namespace PMR
{
    [CreateAssetMenu(fileName = "MyTimeCurveData", menuName = "PMR/TimeCurves/Time Curve Data")]
    public class TimeCurveData : ScriptableObject
    {
        [field: SerializeField] private AnimationCurve curveToFollow;
        [Tooltip("The duration of the TimeCurve, aka the time it takes for the curve to go from 0 to 1.")]
        [field: SerializeField] private float duration;

        public AnimationCurve GetCurve() => curveToFollow;
        public float GetDuration() => duration;

    }
}
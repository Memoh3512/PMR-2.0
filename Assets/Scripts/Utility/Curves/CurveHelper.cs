using System;
using UnityEngine;

namespace PMR
{    
    [Serializable]
    public class TimeCurve: ScriptableObject
    {
        [field: SerializeField] private AnimationCurve curveToFollow;
        [field: SerializeField] private float time;
        
        private float startTime;
        
        //Returns the value of the curve at the current time fraction
        public float EvaluateCurve()
        {
            float deltaTime = Time.realtimeSinceStartup - startTime;
            float timeFraction = deltaTime / time;
            
            return curveToFollow.Evaluate(timeFraction);
        }
        
        public void Start(/*MonoBehaviour owner*/)
        {
            /*if (owner == null)
            {
                Debug.LogError("Owner is null!");
                return;
            }*/
            
            startTime = Time.realtimeSinceStartup;
        }

        public bool IsElapsed() => (Time.realtimeSinceStartup - startTime) >= time;

        public bool IsStarted() => startTime != 0;

        public bool IsStartedNotElapsed() => IsStarted() && !IsElapsed();

        public void Stop() => startTime = 0;
    }

    public abstract class ScriptedTimeCurve<TValueType> : TimeCurve
    {
        protected TValueType startValue;
        protected TValueType endValue;

        public void SetValues(TValueType startValue, TValueType endValue)
        {
            this.startValue = startValue;
            this.endValue = endValue;
        }
        
        public TValueType Value()
        {
            return EvaluateValue(EvaluateCurve());
        }
        
        protected abstract TValueType EvaluateValue(float delta);
    }
    
    public abstract class ValueTimeCurve<TValueType> : TimeCurve
    {
        [field: SerializeField] protected TValueType startValue;
        [field: SerializeField] protected TValueType endValue;
        
        public TValueType Value()
        {
            return EvaluateValue(EvaluateCurve());
        }

        protected abstract TValueType EvaluateValue(float delta);
    }
}

using System;
using UnityEngine;

namespace PMR
{
    [Serializable]
    public class TimeCurve
    { 
        [field: SerializeField] private TimeCurveData data;
        
        private float startTime;
        
        public TimeCurve(TimeCurveData data)
        {
            this.data = data;
        }
        
        //Returns the value of the curve at the current time fraction
        public float EvaluateCurve()
        {
            float deltaTime = Time.realtimeSinceStartup - startTime;
            float timeFraction = deltaTime / data.GetDuration();
            
            return data.GetCurve().Evaluate(timeFraction);
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

        public bool IsElapsed() => (Time.realtimeSinceStartup - startTime) >= data.GetDuration();
        public bool IsStarted() => startTime != 0;
        public bool IsStartedNotElapsed() => IsStarted() && !IsElapsed();
        public void Stop() => startTime = 0;
        public void Reset() => Stop();
    }

    [Serializable]
    public abstract class ScriptedTimeCurve<TValueType> : TimeCurve
    {
        protected TValueType startValue;
        protected TValueType endValue;

        protected bool valuesSet;

        protected ScriptedTimeCurve(TimeCurveData data) : base(data) {}

        public void SetValues(TValueType startValue, TValueType endValue)
        {
            this.startValue = startValue;
            this.endValue = endValue;
            valuesSet = true;
        }
        
        public void StartFromEnd(TValueType endValue)
        {
            if (valuesSet)
            {
                SetValues(this.endValue, endValue);
            }
            else
            {
                SetValues(endValue, endValue);
            }
            Start();
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
        
        protected ValueTimeCurve(TimeCurveData data) : base(data) {}

        public TValueType Value()
        {
            return EvaluateValue(EvaluateCurve());
        }

        protected abstract TValueType EvaluateValue(float delta);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Universal.CheckStage
{
    public class StageGoal
    {
        protected string _EventMsg = null;
        public string EventMsg
        {
            get { return _EventMsg; }
        }

        protected bool _Achieved = false;
        public bool Achieved
        {
            get { return _Achieved; }
        }

        protected float _MaxValue = 1f;
        public float MaxValue
        {
            get { return _MaxValue; }
        }

        protected float _NowValue = 0f;
        public float NowValue
        {
            get { return _NowValue; }

            set
            {
                _NowValue = value;
                _Achieved = IsAchieved();
            }
        }

        public StageGoal(string msg)
        {
            _EventMsg = msg;
        }

        protected virtual bool IsAchieved()
        {
            return _NowValue >= _MaxValue;
        }

        public virtual void SetValue(params object[] args)
        {
            NowValue = MaxValue;
        }

        public virtual void ResetValue()
        {
            _NowValue = 0f;
            _Achieved = false;
        }
    }
}
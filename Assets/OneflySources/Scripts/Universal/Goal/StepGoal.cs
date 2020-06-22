using System;
using UnityEngine;
using Universal.Flow;

namespace Universal.Goal
{
    /**********************************************
    * Copyright (C) 2019 讯飞幻境（北京）科技有限公司
    * 模块名: StageGoal.cs
    * 创建者：RyuRae
    * 修改者列表：
    * 创建日期：
    * 功能描述：
    ***********************************************/
    //[CustomEditor(typeof(StageGoal), true)]
    //[System.Serializable]
    public abstract class StepGoal : MonoBehaviour, IAchieveEvent, IFailedEvent
    {
        [Tooltip("当前目标所属步骤id")]
        /// <summary>
        /// 当前步骤id
        /// </summary>
        public string stepId;
        //[Tooltip("当前目标在本步骤的执行顺序")]
        /// <summary>
        /// 当前目标在本步骤的执行顺序
        /// </summary>
        //public int goalOrder;

        protected bool achieved;

        protected bool single;
        [HideInInspector]
        /// <summary>
        /// 是否只执行一次
        /// </summary>
        public bool only;

        /// <summary>
        /// 当前目标是否达成
        /// </summary>
        public bool Achieved { get { return achieved; } }

        protected bool skip;

        /// <summary>
        /// 是否可跳过
        /// </summary>
        public bool Skip { get { return skip; } }

        protected virtual void Start()
        {
            achieved = false;
            single = false;
            skip = false;
        }

        protected virtual void OnEnable()
        {
            FlowManager.Instance.SetListener(this);
        }

        /// <summary>
        /// 设置目标达成
        /// </summary>
        /// <returns></returns>
        public virtual bool IsAchieved()
        {
            return false;
        }

        /// <summary>
        /// 设置达成值
        /// </summary>
        /// <param name="args"></param>
        public virtual void SetValue(params object[] args)
        {

        }

        /// <summary>
        /// 重置当前目标
        /// </summary>
        public virtual void ResetValue()
        {

        }

        public virtual void OnAchieved(Action action = null)
        {
            if (action != null)
                action();
        }

        protected virtual void OnApplicationQuit() 
        {
            FlowManager.Instance.RemoveListener(this);
        }

        public bool IsFailed()
        {
            return false;
        }

        public void OnFailed(Action action = null)
        {
            if (action != null)
                action();
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Anim;
using Universal.Flow;
using Universal.Goal;

namespace Universal.Goal
{
    /**********************************************
    * Copyright (C) 2019 讯飞幻境（北京）科技有限公司
    * 模块名: AnimStageGoal.cs
    * 创建者：RyuRae
    * 修改者列表：
    * 创建日期：
    * 功能描述：
    ***********************************************/
    public class AnimStepGoal : StepGoal
    {
        
        //动画组件
        private Animation anim;
        /// <summary>
        /// 需要播放的动画名称
        /// </summary>
        public string animName;

        protected override void Start()
        {
            base.Start();
            anim = GetComponentInChildren<Animation>();
        }

        public override bool IsAchieved()
        {
            return Achieved;
        }

        public override void SetValue(params object[] args)
        {
            if (string.IsNullOrEmpty(animName)) return;
            //播放动画，动画完成后目标达成
            if (!single)
            {
                AnimManager.Instance.PlayAnim(anim, animName, () =>
                {
                    achieved = true;
                });
                single = true;
            }
        }

        public override void ResetValue()
        {
            //动画重置
            anim[animName].time = 0;
            anim[animName].enabled = true;
            anim.Sample();
            anim[animName].enabled = false;
            achieved = false;
            single = false;
        }

    }
}
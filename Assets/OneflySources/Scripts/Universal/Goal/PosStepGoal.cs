using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Audio;
using Universal.Flow;
using Universal.Goal;

namespace Universal.Goal
{
    /**********************************************
    * Copyright (C) 2019 讯飞幻境（北京）科技有限公司
    * 模块名: PosStageGoal.cs
    * 创建者：RyuRae
    * 修改者列表：
    * 创建日期：
    * 功能描述：
    ***********************************************/
    public class PosStepGoal : StepGoal
    {
        [Tooltip("位置移动的目标点")]
        /// <summary>
        /// 目标点
        /// </summary>
        public GameObject target;
        //物体初始位置
        private Vector3 initPos;
        //物体初始旋转
        private Vector3 initRot;
        //物体初始缩放
        private Vector3 initScale;
        //物体初始父级
        private Transform initParent;

        protected override void Start()
        {
            base.Start();
            Init();
        }

        private void Init()
        {
            initParent = transform.parent;
            initPos = transform.localPosition;
            initRot = transform.localEulerAngles;
            initScale = transform.localScale;
        }

        //当前目标是否达成
        public override bool IsAchieved()
        {
            return Achieved;
        }


        public override void SetValue(params object[] args)
        {
            if (target == null)
            {
                //Debug.LogError("目标物体为空！！！");
                return;
            }
            float distance = Vector3.Distance(transform.localPosition, target.transform.localPosition);
            //Debug.Log(distance);
            if (distance <= 0.5f)
            {
                transform.localPosition = target.transform.localPosition;
                achieved = true;
                //当前卡牌识别关闭
                
            }
        }

        public override void OnAchieved(Action action = null)
        {
            base.OnAchieved(action);
            Debug.Log("OnAchieved");
            //播放位置匹配音效
            AudioPlayer.Instance.PlayAudio(Audio.AudioType.effect, 0);
        }

        /// <summary>
        /// 将当前目标重置
        /// </summary>
        public override void ResetValue()
        {
            transform.parent = initParent;
            transform.localPosition = initPos;
            transform.localEulerAngles = initRot;
            transform.localScale = initScale;
            achieved = false;
            single = false;
        }

        //void OnApplicationQuit()
        //{
        //    Destroy(FlowManager.Instance);
        //}
    }
}
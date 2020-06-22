using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Audio;
using Universal.Flow;

namespace Universal.Goal
{
    /**********************************************
    * Copyright (C) 2019 讯飞幻境（北京）科技有限公司
    * 模块名: AudioStageGoal.cs
    * 创建者：RyuRae
    * 修改者列表：
    * 创建日期：
    * 功能描述：
    ***********************************************/
    public class AudioStepGoal : StepGoal
    {
        [Tooltip("需要播放的音频")]
        /// <summary>
        /// 需要播放的音频
        /// </summary>
        public AudioClip clip;

        /// <summary>
        /// 音频提示下标
        /// </summary>
        //public int index;

        [Tooltip("提示文本")]
        /// <summary>
        /// 提示文本
        /// </summary>
        public string audioText;

        /// <summary>
        /// 音频提示
        /// </summary>
        private AudioHint audioHint;

        protected override void Start()
        {
            base.Start();
            if (audioHint == null)
                audioHint = GameObject.Find("AudioHint").GetComponent<AudioHint>();
        }

        public override bool IsAchieved()
        {
            return Achieved;
        }

        public override void SetValue(params object[] args)
        {
            if (clip == null) return;
            if (!single)
            {
                audioHint.ShowHint(clip, audioText, () =>
                {
                    achieved = true;
                });
                single = true;
            }
        }

        public override void ResetValue()
        {
            achieved = false;
            single = false;
        }


    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Anim;
using Universal.Audio;
using DG.Tweening;

namespace Universal.Goal
{
    /**********************************************
    * Copyright (C) 2019 讯飞幻境（北京）科技有限公司
    * 模块名: AllStageGoal.cs
    * 创建者：RyuRae
    * 修改者列表：
    * 创建日期：
    * 功能描述：
    ***********************************************/

    //[System.Serializable]
    /// <summary>
    /// 位置达成
    /// </summary>
    //public class PosStageGoal : StageGoal
    //{
    //    /// <summary>
    //    /// 达成位置的物体
    //    /// </summary>
    //    public GameObject obj;

    //    /// <summary>
    //    /// 目标点
    //    /// </summary>
    //    public GameObject target;
    //    //物体初始位置
    //    private Vector3 initPos;
    //    //物体初始旋转
    //    private Vector3 initRot;
    //    //物体初始缩放
    //    private Vector3 initScale;
    //    //物体初始父级
    //    private Transform initParent;

    //    public PosStageGoal()
    //    {
    //        Init();
    //    }

    //    private void Init()
    //    {
    //        if (obj != null)
    //        {
    //            initParent = obj.transform.parent;
    //            initPos = obj.transform.localPosition;
    //            initRot = obj.transform.localEulerAngles;
    //            initScale = obj.transform.localScale;
    //        }
    //    }

    //    public PosStageGoal(GameObject obj, GameObject target)
    //    {
    //        this.obj = obj;
    //        this.target = target;
    //        Init();
    //    }

    //    //当前目标是否达成
    //    public override bool IsAchieved()
    //    {
    //        return Achieved;
    //    }


    //    public override void SetValue(params object[] args)
    //    {
    //        if (Vector3.Distance(obj.transform.localPosition, target.transform.localPosition) <= 0.1f)
    //            achieved = true;
    //    }

    //    /// <summary>
    //    /// 将当前目标重置
    //    /// </summary>
    //    public override void ResetValue()
    //    {
    //        obj.transform.parent = initParent;
    //        obj.transform.localPosition = initPos;
    //        obj.transform.localEulerAngles = initRot;
    //        obj.transform.localScale = initScale;
    //        achieved = false;
    //    }

    //}

    /// <summary>
    /// 音频达成
    /// </summary>
    //public class AudioStageGoal : StageGoal
    //{
    //    /// <summary>
    //    /// 需要播放的音频
    //    /// </summary>
    //    public AudioClip clip;
    //    /// <summary>
    //    /// 音频提示下标
    //    /// </summary>
    //    public int index;

    //    /// <summary>
    //    /// 音频提示
    //    /// </summary>
    //    public AudioHint audioHint;

    //    public AudioStageGoal()
    //    {
    //        if (audioHint == null)
    //            audioHint = GameObject.Find("AudioHint").GetComponent<AudioHint>();
    //    }

    //    public AudioStageGoal(AudioClip clip, int index, AudioHint audioHint = null)
    //    {
    //        //this.source = source;
    //        this.clip = clip;
    //        this.index = index;
    //        this.audioHint = audioHint;
    //        if (audioHint == null)
    //            audioHint = GameObject.Find("AudioHint").GetComponent<AudioHint>();
    //    }

    //    public override bool IsAchieved()
    //    {

    //        return Achieved;
    //    }

    //    public override void SetValue(params object[] args)
    //    {
    //        if (!single)
    //        {
    //            audioHint.ShowHint(clip, index, () =>
    //            {
    //                achieved = true;
    //            });
    //            single = true;
    //        }
    //    }

    //    public override void ResetValue()
    //    {
    //        achieved = false;
    //        single = false;
    //    }

    //}

    /// <summary>
    /// 动画达成（以Animation为例，后续需要animator可以自己仿照编写）
    /// </summary>
    //public class AnimStageGoal : StageGoal
    //{
    //    /// <summary>
    //    /// 需要播放动画的物体
    //    /// </summary>
    //    public GameObject obj;
    //    //动画组件
    //    private Animation anim;
    //    /// <summary>
    //    /// 需要播放的动画名称
    //    /// </summary>
    //    public string animName;

    //    public AnimStageGoal()
    //    {
    //        anim = obj.GetComponentInChildren<Animation>();
    //    }

    //    public AnimStageGoal(GameObject obj, string animName)
    //    {
    //        this.obj = obj;
    //        this.animName = animName;
    //        anim = obj.GetComponentInChildren<Animation>();
    //    }

    //    public override bool IsAchieved()
    //    {
    //        return Achieved;
    //    }

    //    public override void SetValue(params object[] args)
    //    {
    //        //播放动画，动画完成后目标达成
    //        if (!single)
    //        {
    //            AnimManager.Instance.PlayAnim(anim, animName, () =>
    //            {
    //                achieved = true;
    //            });
    //            single = true;
    //        }
    //    }

    //    public override void ResetValue()
    //    {
    //        //动画重置
    //        anim[animName].time = 0;
    //        anim[animName].enabled = true;
    //        anim.Sample();
    //        anim[animName].enabled = false;
    //        achieved = false;
    //        single = false;
    //    }

    //}

    /// <summary>
    /// tween动画达成
    /// </summary>
    public class TweenStepGoal : StepGoal
    {
        /// <summary>
        /// 需要做tween动画的物体
        /// </summary>
        public GameObject obj;
        //tween动画
        private Tweener tweener;

        public TweenStepGoal()
        {
            
        }

        public TweenStepGoal(GameObject obj, Tweener tweener)
        {
            this.obj = obj;
            this.tweener = tweener;
        }

        public override bool IsAchieved()
        {
            return Achieved;
        }

        public override void SetValue(params object[] args)
        {
            if (!single)
            {
                tweener.Play().OnComplete(() =>
                {
                    achieved = true;
                });
                single = true;
            }
        }

        public override void ResetValue()
        {
            tweener.Rewind();
            achieved = false;
            single = false;
        }
    }

    /// <summary>
    /// 相机目标达成
    /// </summary>
    public class CamStageGoal : StepGoal
    {

    }
}

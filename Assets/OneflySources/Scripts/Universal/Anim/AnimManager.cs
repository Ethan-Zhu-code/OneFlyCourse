using DevelopEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Universal.Anim
{
    /**********************************************
    * Copyright (C) 2019 讯飞幻境（北京）科技有限公司
    * 模块名: AnimManager.cs
    * 创建者：RyuRae
    * 修改者列表：
    * 创建日期：
    * 功能描述：动画播放管理
    ***********************************************/
    public class AnimManager : MonoSingleton<AnimManager>
    {
        /// <summary>
        /// 播放动画
        /// </summary>
        /// <param name="anim">需要播放动画的组件</param>
        /// <param name="animName">要播放的动画名称</param>
        /// <param name="action">动画播放完成后的回调（可不写）</param>
        public void PlayAnim(Animation anim, string animName, Action action = null)
        {
            StartCoroutine(WaitAnimPlay(anim, animName, action));
        }

        IEnumerator WaitAnimPlay(Animation anim, string animName, Action action = null)
        {
            if (!anim.isPlaying)
                anim.Play(animName);
            if (action != null)
            {
                yield return new WaitForSeconds(anim[animName].length);
                action();
            }
        }
        
    }
}

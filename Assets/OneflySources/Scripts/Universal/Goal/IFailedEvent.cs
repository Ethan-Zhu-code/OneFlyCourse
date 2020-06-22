using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Universal.Goal
{
    /**********************************************
    * Copyright (C) 2019 讯飞幻境（北京）科技有限公司
    * 模块名: IFailedEvent.cs
    * 创建者：RyuRae
    * 修改者列表：
    * 创建日期：
    * 功能描述：
    ***********************************************/
    public interface IFailedEvent
    {
        /// <summary>
        /// 是否目标达成失败
        /// </summary>
        /// <returns></returns>
        bool IsFailed();

        /// <summary>
        /// 当目标失败时调用
        /// </summary>
        /// <param name="action"></param>
        void OnFailed(Action action = null);

    }
}

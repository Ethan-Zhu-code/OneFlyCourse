using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Universal.Goal
{
    /**********************************************
    * Copyright (C) 2019 讯飞幻境（北京）科技有限公司
    * 模块名: IAchieveEvent.cs
    * 创建者：RyuRae
    * 修改者列表：
    * 创建日期：
    * 功能描述：
    ***********************************************/
    public interface IAchieveEvent
    {
        /// <summary>
        /// 是否达成目标
        /// </summary>
        /// <returns></returns>
        bool IsAchieved();

        /// <summary>
        /// 达成目标后的调用
        /// </summary>
        void OnAchieved(Action action = null);
    }
}

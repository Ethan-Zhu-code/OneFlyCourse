using OneFlyLib;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Universal.Card
{
    /**********************************************
    * Copyright (C) 2019 讯飞幻境（北京）科技有限公司
    * 模块名: BeginCard.cs
    * 创建者：RyuRae
    * 修改者列表：
    * 创建日期：
    * 功能描述：开始卡牌
    ***********************************************/
    public class BeginCard : UIParent
    {

        private bool isBegin = false;

        public override void SetTween()
        {
            base.SetTween();
            if (UIManager.Instance.GetVisibleByName(UIName.UISceneBegin) && !isBegin)
            {
                //ManagerEvent.Call(Tips.SetCardsAvailable, false, 0);
                isBegin = true;
                //隐藏收集卡牌界面并倒计时
                UIManager.Instance.GetUI<UISceneBegin>(UIName.UISceneBegin).EquipBegin();              
            }
        }
    }
}

using OneFlyLib;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Audio;

namespace Universal.Card
{
    /**********************************************
    * Copyright (C) 2019 讯飞幻境（北京）科技有限公司
    * 模块名: CardOptionControl.cs
    * 创建者：RyuRae
    * 修改者列表：
    * 创建日期：
    * 功能描述：
    ***********************************************/
    public class CardOptionControl : MonoBehaviour
    {


        void OnEnable()
        {
            ManagerEvent.Register(Tips.CardAdd, CardAddHandler);
            ManagerEvent.Register(Tips.CardUpdate, CardUpdateHandler);
            ManagerEvent.Register(Tips.CardRemove, CardRemoveHandler);
            ManagerEvent.Register(CardOrientation.CardLeft.ToString(), CardLeftHandler);
            ManagerEvent.Register(CardOrientation.CardRight.ToString(), CardRightHandler);
        }

        void OnDisable()
        {
            ManagerEvent.Unregister(Tips.CardAdd, CardAddHandler);
            ManagerEvent.Unregister(Tips.CardUpdate, CardUpdateHandler);
            ManagerEvent.Unregister(Tips.CardRemove, CardRemoveHandler);
            ManagerEvent.Unregister(CardOrientation.CardLeft.ToString(), CardLeftHandler);
            ManagerEvent.Unregister(CardOrientation.CardRight.ToString(), CardRightHandler);
        }

        protected SceneObj so = null;
        protected string tempName = null;
        private void CardAddHandler(params object[] args)
        {
            if (args.Length > 0 && args[0] is SceneObj)
            {
                so = args[0] as SceneObj;
                if (so != null)
                    CardAddEvent(so);
            }
            
        }

        private void CardUpdateHandler(params object[] args)
        {
            if (args.Length > 0 && args[0] is SceneObj)
            {
                so = args[0] as SceneObj;
                if (so != null)
                    CardUpdateEvent(so);
            }
        }


        private void CardRemoveHandler(params object[] args)
        {
            if (args.Length > 0 && args[0] is SceneObj)
            {
                so = args[0] as SceneObj;
                if (so != null)
                    CardRemoveEvent(so);
            }
        }

        private void CardLeftHandler(params object[] args)
        {
            if (args.Length > 0 && args[0] is string)
            {
                tempName = args[0] as string;
                if (!string.IsNullOrEmpty(tempName))
                    CardLeftEvent(tempName);
            }
        }

        private void CardRightHandler(params object[] args)
        {
            if (args.Length > 0 && args[0] is string)
            {
                tempName = args[0] as string;
                if (!string.IsNullOrEmpty(tempName))
                    CardRightEvent(tempName);
            }
        }

        /// <summary>
        /// 卡牌的增加操作事件（只执行一次）
        /// </summary>
        /// <param name="so">增加的卡牌物体</param>
        protected virtual void CardAddEvent(SceneObj so)
        {

        }

        /// <summary>
        /// 卡牌的更细操作事件
        /// </summary>
        /// <param name="so">更新的卡牌物体</param>
        protected virtual void CardUpdateEvent(SceneObj so)
        {

        }

        /// <summary>
        /// 卡牌的移除操作事件（只执行一次）
        /// </summary>
        /// <param name="so">移除的卡牌物体</param>
        protected virtual void CardRemoveEvent(SceneObj so)
        {

        }

        /// <summary>
        /// 卡牌的左转事件（只执行一次）
        /// </summary>
        /// <param name="name">卡牌UI的名称</param>
        protected virtual void CardLeftEvent(string name)
        {

        }

        /// <summary>
        /// 卡牌的右转事件（只执行一次）
        /// </summary>
        /// <param name="name">卡牌UI的名称</param>
        protected virtual void CardRightEvent(string name)
        {

        }
       
    }
}

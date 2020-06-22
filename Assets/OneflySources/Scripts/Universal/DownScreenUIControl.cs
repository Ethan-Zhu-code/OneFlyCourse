using OneFlyLib;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Universal
{
    /**********************************************
    * Copyright (C) 2019 讯飞幻境（北京）科技有限公司
    * 模块名: DownScreenUIControl.cs
    * 创建者：RyuRae
    * 修改者列表：
    * 创建日期：
    * 功能描述：
    ***********************************************/
    public class DownScreenUIControl : MonoBehaviour
    {

        //public Transform[] hints;

        private void Awake()
        {

        }
        private void Start()
        {
            DataReceiver.Instance.SetCheckAvailableCard(true);
        }
        private void OnEnable()
        {
            //ManagerEvent.Register(Tips.SwitchHint_Down, SwitchHint_DownHandler);
            ManagerEvent.Register(Tips.ActiveDownScreen, ActiveDownScreenHandler);
            ManagerEvent.Register(Tips.SetCardsAvailable, SetCardsAvailableHandler);
            ManagerEvent.Register(Tips.CleanAndReset, CleanAndResetHandler);
            ManagerEvent.Register(Tips.SetIfCheckCardAvailable, SetIfCheckCardAvailableHandler);
        }

        private void OnDisable()
        {
            //ManagerEvent.Unregister(Tips.SwitchHint_Down, SwitchHint_DownHandler);
            ManagerEvent.Unregister(Tips.ActiveDownScreen, ActiveDownScreenHandler);
            ManagerEvent.Unregister(Tips.SetCardsAvailable, SetCardsAvailableHandler);
            ManagerEvent.Unregister(Tips.CleanAndReset, CleanAndResetHandler);
            ManagerEvent.Unregister(Tips.SetIfCheckCardAvailable, SetIfCheckCardAvailableHandler);
        }

        #region
        //private void SwitchHint_DownHandler(params object[] args)
        //{
        //    if (args != null)
        //    {
        //        if (args.Length == 1 && args[0] is int)
        //        {
        //            int downScreenIndex = (int)args[0];
        //            if (0 <= downScreenIndex && downScreenIndex < hints.Length)
        //            {
        //                for (int i = 0; i < hints.Length; i++)
        //                {
        //                    hints[i].gameObject.SetActive(i == downScreenIndex);
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        for (int i = 0; i < hints.Length; i++)
        //        {
        //            hints[i].gameObject.SetActive(false);
        //        }
        //    }
        //}
        #endregion

        //下屏不可用
        private void ActiveDownScreenHandler(params object[] args)
        {
            if (args != null && args.Length == 1 && args[0] is bool)
            {
                bool isActive = (bool)args[0];
                DataReceiver.Instance.isLockDownScreen = !isActive;
            }
        }

        //清空和重置下屏UI卡牌
        private void CleanAndResetHandler(params object[] args)
        {
            //清空下屏UI卡牌
            DataReceiver.Instance.SetAllAvailableCards(false, true);
            DataReceiver.Instance.ResetCurrentMutexCard();
        }

        //设置卡牌的显示
        private void SetCardsAvailableHandler(params object[] args)
        {
            if (args != null && args.Length > 1 && args[0] is bool)
            {
                bool isAvailable = (bool)args[0];
                List<int> indexList = new List<int>();
                for (int i = 1; i < args.Length; i++)
                {
                    if (args[i] is int)
                    {
                        indexList.Add((int)args[i]);
                    }
                }
                DataReceiver.Instance.SetAvailableCards(isAvailable, indexList); //注意修改
            }
        }

        //设置检查卡牌是否激活
        public void SetIfCheckCardAvailableHandler(params object[] args)
        {
            if (args != null && args.Length == 1 && args[0] is bool)
            {
                bool isCheck = (bool)args[0];
            }
        }

    }
}

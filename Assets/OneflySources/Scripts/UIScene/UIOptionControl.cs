using OneFlyLib;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**********************************************
* Copyright (C) 2019 讯飞幻境（北京）科技有限公司
* 模块名: UIOptionControl.cs
* 创建者：RyuRae
* 修改者列表：
* 创建日期：
* 功能描述：UI操作控制
***********************************************/
public class UIOptionControl : MonoBehaviour {


    void OnEnable()
    {
        ManagerEvent.Register(Tips.ShowHint, ShowHintHandler);
        
    }


    void OnDisable()
    {
        ManagerEvent.Unregister(Tips.ShowHint, ShowHintHandler);
    }


    private void ShowHintHandler(params object[] args)
    {
        if (args.Length >= 2)
        {
            UISceneHint.HintType type = (UISceneHint.HintType)args[0];
            string content = (string)args[1];
            UIManager.Instance.SetVisible(UIName.UISceneHint, true);
            var uiScene = UIManager.Instance.GetUI<UISceneHint>(UIName.UISceneHint);
            uiScene.ShowHint(type, content);
        }
    }

}

using DevelopEngine;
using OneFlyLib;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Universal.Card;

/**********************************************
* Copyright (C) 2019 讯飞幻境（北京）科技有限公司
* 模块名: ClassControlCard.cs
* 创建者：RyuRae
* 修改者列表：
* 创建日期：
* 功能描述：
***********************************************/
public class ClassControlCard : CardOptionControl
{
   

    protected override void CardLeftEvent(string name)
    {
        if (!string.IsNullOrEmpty(name) && name.Equals("ClassControl"))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
                        Application.Quit();
#endif
        }
    }


    protected override void CardRightEvent(string name)
    {
        if (!string.IsNullOrEmpty(name) && name.Equals("ClassControl"))
        {
            ManagerEvent.Clear();
            SceneManager.LoadScene(1);
        }
    }

}

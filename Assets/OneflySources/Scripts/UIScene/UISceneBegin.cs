using OneFlyLib;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Universal.Audio;


/**********************************************
* Copyright (C) 2019 讯飞幻境（北京）科技有限公司
* 模块名: UISceneBegin.cs
* 创建者：RyuRae
* 修改者列表：
* 创建日期：
* 功能描述：开始UI管理模块
***********************************************/
public class UISceneBegin : UIScene {

    public GameObject info_Intro;
    public GameObject countTime;
    public GameObject stateController;
	protected override void Start () {
        base.Start();
        AudioPlayer.Instance.PlayAudio(Universal.Audio.AudioType.speech,3);
        ManagerEvent.Call(Tips.ActiveDownScreen, true);
        ManagerEvent.Call(Tips.SetCardsAvailable, true, 0, 1);
	}

    public void EquipBegin()
    {
        if (info_Intro != null)
            info_Intro.SetActive(false);
        if (countTime != null)
            countTime.SetActive(true);
        StartCoroutine(Count());
    }

    IEnumerator Count()
    {
        for (int i = 3; i > 0; i--)
        {
            AudioPlayer.Instance.PlayAudio(Universal.Audio.AudioType.speech, 0);
            transform.GetComponentInChildren<Text>().text = i.ToString();

            yield return new WaitForSeconds(1);

        }
        countTime.SetActive(false);
        //Destroy(gameObject);
        SetVisible(false);
        ManagerEvent.Call(Tips.SetCardsAvailable, false, 0);
        ManagerEvent.Call(Tips.SetCardsAvailable, true, 2, 3);
        stateController.SetActive(true);
    }


}

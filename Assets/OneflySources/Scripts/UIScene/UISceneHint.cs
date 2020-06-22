using Course;
using OneFlyLib;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**********************************************
* Copyright (C) 2019 讯飞幻境（北京）科技有限公司
* 模块名: UISceneHint.cs
* 创建者：RyuRae
* 修改者列表：
* 创建日期：
* 功能描述：
***********************************************/
public class UISceneHint : UIScene {

    public enum HintType
    {
        /// <summary>
        /// 结论
        /// </summary>
        CONCLUSION,
        /// <summary>
        /// 错误
        /// </summary>
        ERROR,
        /// <summary>
        /// 注意
        /// </summary>
        NOTICE,
        /// <summary>
        /// 总结
        /// </summary>
        SUMMARY
    }

    public List<GameObject> hints;
    public List<Text> texts;
    public GameObject item_Error;
    private CourseControl courseControl;
    public Transform errorParent;
    protected override void Start () {
        base.Start();
        courseControl = FindObjectOfType<CourseControl>();

    }

    GameObject go = null;
    Text text = null;
    /// <summary>
    /// 显示提示
    /// </summary>
    /// <param name="type">提示类型</param>
    /// <param name="content">提示内容</param>
    public void ShowHint(HintType type, string content = null)
    {
        switch (type)
        {
            case HintType.CONCLUSION:
                SetHint("Conclusion", content);
                break;
            case HintType.ERROR:
                SetHint("Error", content);
                break;
            case HintType.NOTICE:
                SetHint("Notice", content);
                break;
            case HintType.SUMMARY:
                ShowSummary();
                break;
            default:
                break;
        }
    }

    //设置提示
    private void SetHint(string name, string content)
    {
        go = hints.Find(p => p.name.Contains(name));
        hints.ForEach(p => p.SetActive(p == go));
        text = texts.Find(p => p.name.Contains(name));
        text.text = content;
    }

    //显示总结
    private void ShowSummary()
    {
        go = hints.Find(p => p.name.Contains("Summary"));
        hints.ForEach(p => p.SetActive(p == go));
        if (CourseControl.courseInfo == null) return;
        var oper = CourseControl.courseInfo as OperationInfo;
        for (int i = 0; i < oper.Errors.Count; i++)
        {
            GameObject clone = Instantiate(item_Error, errorParent);
            var number = Global.FindChild<Text>(clone.transform, "number");
            if (number != null)
                number.text = (i + 1).ToString();
            var error = Global.FindChild<Text>(clone.transform, "errorOperation");
            if (error != null)
                error.text = oper.Errors[i].error;
            var value = Global.FindChild<Text>(clone.transform, "errorScore");
            if (value != null)
            {
                float scoreValue = oper.EpPoints[oper.Errors[i].error] * oper.Errors[i].value;
                scoreValue = scoreValue >= 100 ? 100 : scoreValue;
                value.text = "-" + scoreValue.ToString();
            }
        }
        var score = Global.FindChild<Text>(go.transform, "Score");
        if (score != null)
            score.text = oper.GetScore().ToString();
        var time = Global.FindChild<Text>(go.transform, "time");
        float currTime = oper.GetCountTime();
        if (currTime > 60)
        {
            time.text = (currTime / 60).ToString("0") + "分" + (currTime % 60).ToString("0") + "秒";
        }
        else
            time.text = currTime.ToString("0") + "秒";
    }

    /// <summary>
    /// 隐藏当前提示
    /// </summary>
    public void HideCurrHint()
    {
        if (go != null)
        {
            go.SetActive(false);
            go = null;
        }
        SetVisible(true);//隐藏当前UIScene
    }
}

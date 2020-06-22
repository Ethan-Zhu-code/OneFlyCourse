using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Newtonsoft.Json;
using DevelopEngine;
using System.IO;
using Newtonsoft.Json;

namespace Course
{
    /**********************************************
    * Copyright (C) 2019 讯飞幻境（北京）科技有限公司
    * 模块名: CourseInfo.cs
    * 创建者：RyuRae
    * 修改者列表：
    * 创建日期：
    * 功能描述：课程信息类
    ***********************************************/
    public class CourseInfo
    {
        protected string courseName;

        protected string purpose;

        protected string conclusion;

        protected string time;

        protected string subject;

        /// <summary>
        /// 实验名称
        /// </summary>
        public string CourseName { get { return courseName; } }
        /// <summary>
        /// 实验目的
        /// </summary>
        public string Purpose { get { return purpose; } }

        /// <summary>
        /// 实验结论
        /// </summary>
        public string Conclusion { get { return conclusion; } }

        /// <summary>
        /// 实验时间
        /// </summary>
        public string Time { get; set; }

        /// <summary>
        /// 实验所属科目
        /// </summary>
        public string Subject { get; set; }


        private float countTime;

        public CourseInfo()
        {
            countTime = UnityEngine.Time.time;
        }

        /// <summary>
        /// 当前对象转为json
        /// </summary>
        public virtual string ToJson()
        {
            //将当前对象转换为json
            string json = JsonConvert.SerializeObject(this);
            return json;
        }

        public virtual void SetProgress(float value)
        {

        }

        public float GetCountTime()
        {
            countTime = UnityEngine.Time.time - countTime;
            return countTime;
        }

        public virtual void AddMessageInErrors(string msg)
        { }

        public virtual void AddMessageInErrors(int id)
        { }


    }

    /// <summary>
    /// 错误信息类
    /// </summary>
    public class ErrorInfo
    {
        /// <summary>
        /// 错误编号
        /// </summary>
        public string id;
        /// <summary>
        /// 错误信息
        /// </summary>
        public string error;
        /// <summary>
        /// 错误对应的分值
        /// </summary>
        public float value;
        /// <summary>
        /// 对应的正确的信息
        /// </summary>
        public string right;

        public ErrorInfo()
        {
        }

        public ErrorInfo(string id, string error, float value, string right)
        {
            this.id = id;
            this.error = error;
            this.value = value;
            this.right = right;
        }
    }

    //[System.Serializable]
    public class OperationInfo : CourseInfo
    {
     
        //操作要点
        private List<string> operates;

        //分数
        private float score = 0;

        //错误信息（错题本）
        private List<ErrorInfo> errors;

        //易错点统计
        private Dictionary<string, int> epPoints;

        //所有错误内容及对应分值
        private List<ErrorInfo> errorPoints;

        //实验完成度
        private string progress;

        //记录错误信息出现次数
        private int count;


        /// <summary>
        /// 操作要点
        /// </summary>
        public List<string> Operates { get { return operates; } }
        /// <summary>
        /// 错误信息
        /// </summary>
        public List<ErrorInfo> Errors { get { return errors; } }
        /// <summary>
        /// 易错点统计
        /// </summary>
        public Dictionary<string, int> EpPoints { get { return epPoints; } }
        /// <summary>
        /// 分数
        /// </summary>
        public float Score { get { return score; } }
        /// <summary>
        /// 完成度
        /// </summary>
        public string Progress { get { return progress; } }


        //目的：将目前操作要点，分数，错误信息，易错点统计，以及实验进度，以报告的形式输出，形成文档
        //操作要点，由教研提供，可从配置文件中读取，也可由程序写入代码中
        //分数，最终输出
        //错误信息，最终总结时将错误信息写入
        //易错点统计，记录错误信息以及对应的错误次数
        public OperationInfo()
        {
            courseName = Configuration.GetContent("CourseName", "name");
            purpose = Configuration.GetContent("Purpose", "name");
            subject = Configuration.GetContent("Subject", "name");
            operates = new List<string>();
            errors = new List<ErrorInfo>();
            epPoints = new Dictionary<string, int>();
            errorPoints = new List<ErrorInfo>();
            operates.AddRange(Configuration.Get("OperationPoint").Values);
            var coll = Configuration.Get("ErrorPoint");
            foreach (var item in coll)
            {
                var arr = item.Value.Split(';');
                errorPoints.Add(new ErrorInfo(item.Key, arr[0], float.Parse(arr[1]), arr[2]));
            }
        }

        public OperationInfo(int index)
        {
            //count = 0;
            courseName = Configuration.GetContent("CourseName", index.ToString());
            purpose = Configuration.GetContent("Purpose", index.ToString());
            subject = Configuration.GetContent("Subject", "name");
            operates = new List<string>();
            errors = new List<ErrorInfo>();
            epPoints = new Dictionary<string, int>();
            errorPoints = new List<ErrorInfo>();
            operates.AddRange(Configuration.Get("OperationPoint").Values);
            var coll = Configuration.Get("ErrorPoint");
            foreach (var item in coll)
            {
                var arr = item.Value.Split(';');
                errorPoints.Add(new ErrorInfo(item.Key, arr[0], float.Parse(arr[1]), arr[2]));
            }
        }

        /// <summary>
        /// 添加错误信息
        /// </summary>
        /// <param name="msg">错误信息</param>
        public override void AddMessageInErrors(string msg)
        {
            var info = errors.Find(p => p.error.Equals(msg));
            if (info == null)
            {
                info = errorPoints.Find(p => p.error.Equals(msg));
                errors.Add(info);
            }

            if (!epPoints.ContainsKey(msg))
                epPoints.Add(msg, 1);
            else
            {
                count = ++epPoints[msg];
                epPoints[msg] = count;
            }
            SetScore();
        }

        /// <summary>
        /// 通过id添加错误信息
        /// </summary>
        /// <param name="id">错误信息id</param>
        public override void AddMessageInErrors(int id)
        {
            string error = GetErrorInfoByIndex(id);
            AddMessageInErrors(error);
        }

        private void SetScore()
        {
            float sum = 0;
            var list = new List<string>(epPoints.Keys);
            for (int i = 0; i < list.Count; i++)
            {
                //错误次数 * 错误分数
                var errorInfo = errorPoints.Find(p => p.error.Equals(list[i]));
                sum += epPoints[list[i]] * errorInfo.value;
            }

            if (!string.IsNullOrEmpty(progress) && progress.Equals("100%"))
                score = 100 - sum >= 0 ? 100 - sum : 0;
        }

        /// <summary>
        /// 设置当前实验进度
        /// </summary>
        /// <param name="value"></param>
        public override void SetProgress(float value)
        {
            progress = value.ToString("0%");
        }

        /// <summary>
        /// 获取当前实验得分
        /// </summary>
        /// <returns></returns>
        public float GetScore()
        {
            SetScore();
            return score;
        }

        /// <summary>
        /// 根据错误信息序号获取错误信息
        /// </summary>
        /// <param name="id">错误序号</param>
        /// <returns>错误内容</returns>
        public string GetErrorInfoByIndex(int id)
        {
            var errorInfo = errorPoints.Find(p => p.id.Equals(id.ToString()));
            return errorInfo.error;
        }

        public override string ToJson()
        {
            string json = JsonConvert.SerializeObject(this, Formatting.Indented);
            return json;
        }

    }

    /// <summary>
    /// 答题信息
    /// </summary>
    public class AnswerInfo
    {
        /// <summary>
        /// 问题序号
        /// </summary>
        public string id;
        /// <summary>
        /// 问题内容
        /// </summary>
        public string question;
        /// <summary>
        /// 问题选项
        /// </summary>
        public List<string> option;
        /// <summary>
        /// 正确答案
        /// </summary>
        public string answer;
        /// <summary>
        /// 回答是否正确
        /// </summary>
        public bool IsCorrect = false;

        public AnswerInfo()
        {

        }

        public AnswerInfo(string id, string question, List<string> option, string answer)
        {
            this.id = id;
            this.question = question;
            this.option = option;
            this.answer = answer;

        }
    }

    /// <summary>
    /// 观察现象类实验
    /// </summary>
    public class ObserveInfo : CourseInfo
    {
        /// <summary>
        /// 所有的答题内容和信息
        /// </summary>
        private List<AnswerInfo> answerInfos;
        /// <summary>
        /// 已答问题
        /// </summary>
        private List<AnswerInfo> answereds;
        //答题正确率
        private float accuracy;
        //当前实验的进度
        private float progress;
        /// <summary>
        /// 答题正确率
        /// </summary>
        public float Accuracy { get { return accuracy; } }
        /// <summary>
        /// 当前实验进度
        /// </summary>
        public float Progress { get { return progress; } }

        public ObserveInfo()
        {
            answerInfos = new List<AnswerInfo>();
            var coll = Configuration.Get("AnswerInfo");
            foreach (var info in coll)
            {
                var arr = info.Value.Split(';');
                var options = arr[1].Split('+');
                answerInfos.Add(new AnswerInfo(info.Key, arr[0], new List<string>(options), arr[2]));
            }
            answereds = new List<AnswerInfo>();
        }

        /// <summary>
        /// 获取当前选择的选项是否正确
        /// </summary>
        /// <param name="content">问题的id或者问题的内容</param>
        /// <param name="option">用户的选项</param>
        public bool GetOption(string content, string option)
        {
            AnswerInfo answer;
            answer = answerInfos.Find(p => p.id.Equals(content) || p.question.Equals(content));
            if ((option.Contains(answer.answer) || answer.answer.Equals(option)))//当前回答正确
            {
                answer.IsCorrect = true;
                answereds.Add(answer);
            }
            return answer.IsCorrect;
        }

        /// <summary>
        /// 获取当前答题正确率
        /// </summary>
        /// <returns></returns>
        public float GetAccuracy()
        {
            float count = 0;
            for (int i = 0; i < answereds.Count; i++)
            {
                if (answereds[i].IsCorrect)
                    count++;
            }
            accuracy = count / (float)answereds.Count;
            return accuracy;
        }

    }

    /// <summary>
    /// 科普课
    /// </summary>
    public class CoupeInfo : CourseInfo
    {
        //需要记录使用时长和答题正确率
    }


    public enum InfoType
    {
        OPERATION,
        COUPE,
        OBSERVE,
        OTHER
    }
}

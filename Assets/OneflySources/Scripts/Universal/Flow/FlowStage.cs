using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Goal;

namespace Universal.Flow
{
    /**********************************************
    * Copyright (C) 2019 讯飞幻境（北京）科技有限公司
    * 模块名: FlowStage.cs
    * 创建者：RyuRae
    * 修改者列表：
    * 创建日期：
    * 功能描述：流程状态
    ***********************************************/
    [System.Serializable]
    public class FlowStage
    {
        //当前流程是否完成
        private bool isDone;

        public bool IsDone { get { return isDone; } }

        private List<StepGoal> goals;

        /// <summary>
        /// 当前步骤需要达成的目标
        /// </summary>
        public List<StepGoal> Goals { get { return goals; } }
        //当期需要达成的目标
        private StepGoal currGoal = null;
        private int index;
        //当前步骤id
        private string stepId;
        /// <summary>
        /// 当前步骤ID
        /// </summary>
        public string StepId { get { return stepId; } }
        public FlowStage()
        {
            index = 0;
        }

        public FlowStage(List<StepGoal> goals)
        {
            this.goals = goals;
            index = 0;
            stepId = goals[0].stepId;
        }

        public void OnSingle()
        {
            OperationHint.Instance.ShowOperationHint(int.Parse(stepId) - 1);
        }

        /// <summary>
        /// 执行当前流程
        /// </summary>
        public virtual void Execute(params object[] args)
        {
           
            for (int i = 0; i < goals.Count; i++)
            {
                if (goals[i].IsAchieved() && !goals[i].only)
                {
                    goals[i].OnAchieved();
                    goals[i].only = true;
                }
                else
                    goals[i].SetValue(args);

                if (goals[i].IsFailed() && !goals[i].only)
                {
                    goals[i].OnFailed();
                    goals[i].only = true;
                }
            }
            Refresh();
        }

        /// <summary>
        /// 刷新当前流程
        /// </summary>
        private void Refresh()
        {
            for (int i = 0; i < goals.Count; i++)
            {
                if (!goals[i].IsAchieved()) return;
            }
            isDone = true;
        }
    }
}

using DevelopEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Goal;

namespace Universal.Flow
{
    /**********************************************
    * Copyright (C) 2019 讯飞幻境（北京）科技有限公司
    * 模块名: FlowManager.cs
    * 创建者：RyuRae
    * 修改者列表：
    * 创建日期：
    * 功能描述：流程管理
    ***********************************************/
    public class FlowManager : MonoSingleton<FlowManager>
    {
              
        private List<FlowStage> flows = new List<FlowStage>();     
        private FlowStage currFlow;
        private int index = 0;
        
        void Start()
        {
            currFlow = null;
            Init();
        }

        private void Init()
        {
            Debug.Log(flows.Count);
            //flows.ForEach(p => Debug.Log(p.StepId));
        }

        
        void Update()
        {

            if (flows.Count > 0 && index < flows.Count)
            {
                if (currFlow != flows[index])
                {
                    currFlow = flows[index];
                    currFlow.OnSingle();
                }
                if (currFlow != null)
                {
                    if (currFlow.IsDone)
                    {
                        index++;
                        currFlow = null;
                    }
                    else
                        currFlow.Execute();
                }
            }
        }

        public float GetProgress()
        {
            return index / (float)flows.Count;
        }

        public void SetListener(StepGoal goal)
        {
            var tempFlow = flows.Find(p => p.StepId.Equals(goal.stepId));
            if (tempFlow != null && !tempFlow.Goals.Contains(goal))
                tempFlow.Goals.Add(goal);
            else if (tempFlow == null)
            {
                List<StepGoal> tempGoals = new List<StepGoal>();
                tempGoals.Add(goal);
                flows.Add(new FlowStage(tempGoals));
            }
            flows.Sort((p, q) => int.Parse(p.StepId).CompareTo(int.Parse(q.StepId)));
        }

        public void RemoveListener(StepGoal goal)
        {
            var tempFlow = flows.Find(p => p.Goals.Find(q => q == goal));
            if (tempFlow != null && tempFlow.Goals.Contains(goal))
                tempFlow.Goals.Remove(goal);
        }

        //void OnApplicationQuit()
        //{
        //    flows.Clear();
        //    currFlow = null;
        //}
    }
}

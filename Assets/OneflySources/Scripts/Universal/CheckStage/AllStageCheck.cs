using Course;
using DevelopEngine;
using OneFlyLib;
using System.Collections;

namespace Universal.CheckStage
{
    public class StageCheck0 : StageCheck
    {
        public StageCheck0(int index) : base(index)
        {
            //这个阶段需要达成的目标 目标与目标之间关系是“和”而不是“或”当所有目标都打成以后才会进入到Exitng通常是stageGoal或者其子类
            //列如：SetGoals(new StageGoal(CamControl.OpeningFinished));
            SetGoals(new StageGoal(Tips.CheckStage));
        }

        protected override IEnumerator Entering()
        {
            //阶段开始前的初始化，比如 那些卡牌可以被识别 上屏出现什么样的动画或者UI之类的 用发送消息来实习
            //ManagerEvent.Call(Tips.CheckStage);
            yield return null;
        }

        protected override IEnumerator Exiting()
        {
            //阶段结束时的操作，一般与开始的的操作相对应，比如关闭UI 或者设置某些卡牌不可用
            //Debugger.Log("完成第一步");
            yield return null;
        }
    }

    public class StageCheck1 : StageCheck
    {
        public StageCheck1(int index) : base(index)
        {
            //这个阶段需要达成的目标 目标与目标之间关系是“和”而不是“或”当所有目标都打成以后才会进入到Exitng通常是stageGoal或者其子类
            //列如：
            //SetGoals(new StageGoal(CamControl.OpeningFinished));

        }

        protected override IEnumerator Entering()
        {
            //阶段开始前的初始化，比如 那些卡牌可以被识别 上屏出现什么样的动画或者UI之类的 用发送消息来实习
            yield return null;
        }

        protected override IEnumerator Exiting()
        {
            //阶段结束时的操作，一般与开始的的操作相对应，比如关闭UI 或者设置某些卡牌不可用

            yield return null;
        }
    }
}

 
using ProcessData;
using UnityEngine;
using Universal.Card;

namespace ProcessData
{
    [System.Serializable]
    public struct Question
    {
        [Header("选择的答题步骤编号")]
        public int stageIndex;

        [Header("选择题的正确答案(A=1,B=2,C=3..)")]
        public int rightOption;

        [Header("选择正确的事件")]
        public EventAsset rightEvent;

        [Header("选择错误触发的事件")]
        public EventAsset wrongEvent;
    }
}

public class ChooseCardHandler : CardOptionControl
{
    public GameObject optionRight;
    public ChooseTrigger chooseCardTrigger;
    public Question[] questions;

    protected override void CardRightEvent(string name)
    {
        if (!string.IsNullOrEmpty(name) && name.Equals("Choose"))
        {
            chooseCardTrigger.optionCollider = null;
            foreach (Question q in questions)
            {
                if (q.stageIndex == StateController.Instance.stateIndex)
                {
                    if (chooseCardTrigger.optionIndex == q.rightOption)
                    {
                        chooseCardTrigger.optionCollider = null;
                        EventController.Instance.EventTrigger(q.rightEvent);
                    }
                    else
                    {
                        chooseCardTrigger.optionCollider = null;
                        EventController.Instance.EventTrigger(q.wrongEvent);
                    }
                    return;
                }
            }
        }
    }
}
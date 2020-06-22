using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Card;
using ProcessData;

public class CardRotateHandler : CardOptionControl
{   
    [Header("控制左旋事件")]
    public BasicInteraction[] leftInteractions;

    [Header("控制右旋事件")]
    public BasicInteraction[] rightInteractions;

    [Header("卡牌名称")]
    public string CardName;

    protected override void CardLeftEvent(string name)
    {
        if (!string.IsNullOrEmpty(name) && name.Equals(CardName))
        {
            //触发左旋事件
            for (int i = 0; i < leftInteractions.Length; i++)
            {
                if (StateController.Instance.stateIndex == leftInteractions[i].stateIndex)
                    EventController.Instance.EventTrigger(leftInteractions[i].eventAsset);
            }
        }
    }

    protected override void CardRightEvent(string name)
    {
        if (!string.IsNullOrEmpty(name) && name.Equals(CardName))
        {   
            //触发右旋事件
            for(int i = 0; i < rightInteractions.Length; i++)
            {
                if (StateController.Instance.stateIndex == rightInteractions[i].stateIndex)
                    EventController.Instance.EventTrigger(rightInteractions[i].eventAsset);
            }

        }
    }
}

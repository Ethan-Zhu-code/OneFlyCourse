using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Card;

public class CardOptionHandler : CardOptionControl
{
    [Header("对应的下屏UI")]
    public CardControl cardUI;
    [Header("左转触发的事件")]
    public EventAsset[] leftEvents;
    [Header("右转触发的事件")]
    public EventAsset[] rightEvents;

    protected override void CardRightEvent(string name)
    {
        if (!string.IsNullOrEmpty(name) && name.Equals(cardUI.gameObject.name))
        {
            foreach (EventAsset currentEvent in rightEvents)
            {
                if (currentEvent.stateName.Equals(StateController.Instance.currentState))
                {
                    EventController.Instance.EventTrigger(currentEvent);
                    return;
                }
            }
        }
    }
    protected override void CardLeftEvent(string name)
    {
        if (!string.IsNullOrEmpty(name) && name.Equals(cardUI.gameObject.name))
        {
            foreach (EventAsset currentEvent in leftEvents)
            {
                if (currentEvent.stateName.Equals(StateController.Instance.currentState))
                {
                    EventController.Instance.EventTrigger(currentEvent);
                    return;
                }
            }
        }
    }
}

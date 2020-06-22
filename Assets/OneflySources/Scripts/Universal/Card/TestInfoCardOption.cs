using OneFlyLib;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Card;
public class TestInfoCardOption : CardOptionControl
{

    protected override void CardAddEvent(SceneObj so)
    {

        string cardName = so.ui.name;
        if (cardName != null && cardName.Equals("TestInfo"))
        {

            UIManager.Instance.SetVisible(UIName.UIScenePurpose, true);

        }

    }

    protected override void CardRemoveEvent(SceneObj so)
    {

        string cardName = so.ui.name;
        if (cardName != null && cardName.Equals("TestInfo"))
        {

            UIManager.Instance.SetVisible(UIName.UIScenePurpose, false);

        }


    }
}

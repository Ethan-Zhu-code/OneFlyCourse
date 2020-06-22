using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Audio;
using ProcessData;
using Universal.Card;
using OneFlyLib;

public class Showconclusion : CardOptionControl
{
    // Start is called before the first frame update
    public BasicInteraction interaction;

    public SceneObj sceneobj;

    private bool flag = false;

    protected override void CardAddEvent(SceneObj so)
    {
        if(so.ui.name.Equals(sceneobj.ui.name))
        {
            if (StateController.Instance.stateIndex == interaction.stateIndex && !flag)
            {
                EventController.Instance.EventTrigger(interaction.eventAsset);
                flag = true;
            }
        }
    }
}

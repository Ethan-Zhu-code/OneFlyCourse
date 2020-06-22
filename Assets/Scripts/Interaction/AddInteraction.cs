using ProcessData;
using UnityEngine;
using Universal.Audio;

/// <summary>
/// 放入卡牌的交互，添加脚本到模型上
/// </summary>
public class AddInteraction : InteractEvent
{
    [SerializeField]
    [Tooltip("交互信息")]
    private BasicInteraction[] basicInteractions;

    private BasicInteraction currentBasicInteraction;

    private void OnEnable()
    {
        if (StateController.Instance == null || !CheckStateIndex())
        {
            return;
        }

        if (!ifFinished)
        {
            if (eventController != null)
            {
                AudioPlayer.Instance.PlayAudio(Universal.Audio.AudioType.effect, 2);             //这里需要改成模型添加的音效，添加到场景的AudioPlayer中
                eventController.EventTrigger(currentBasicInteraction.eventAsset);
            }
            ifFinished = true;
        }
    }

    override public void ResetInteractEvent()
    {
        foreach (BasicInteraction bi in basicInteractions)
        {
            if (bi.stateIndex == StateController.Instance.stateIndex)
            {
                currentBasicInteraction = bi;
                stateIndex = bi.stateIndex;
                ifFinished = false;
                return;
            }
        }

        ifFinished = true;
        stateIndex = -1;
    }
}
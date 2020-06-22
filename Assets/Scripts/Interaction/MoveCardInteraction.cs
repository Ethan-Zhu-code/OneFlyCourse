using ProcessData;
using UnityEngine;
using Universal.Audio;

/// <summary>
/// 模型移动触发的事件
/// </summary>
public class MoveCardInteraction : InteractEvent
{
    [SerializeField]
    private Movement[] moveInteractionGroups;

    private Movement currentMovement;

    private float distance;
    private Vector3 target_Position;
    private Quaternion target_Rotation;
    private GameObject target_gameobject;

    /// <summary>
    /// 在Update中检查距离是否达到阈值内，达到后触发Event事件
    /// </summary>
    private void Update()
    {
        if (!CheckStateIndex())
        {
            //  Debug.Log("index not correct");
            return;
        }

        if (ifFinished || StateController.Instance == null)
        {
            //    Debug.Log("has finished");
            return;
        }

        distance = Vector3.Distance(transform.position, target_Position) * 100;

        Debug.Log(distance);

        if (distance <= currentMovement.triggerDistance && target_gameobject.activeInHierarchy)//到达位置后播放音效，触发Event
        {
            if (!currentMovement.mute)
                AudioPlayer.Instance.PlayAudio(Universal.Audio.AudioType.effect, 1);

            var temp_Position = target_Position;
            var temp_Rotation = target_Rotation;
            ifFinished = true;
            eventController.EventTrigger(currentMovement.eventAsset);

            transform.position = temp_Position;
            transform.rotation = temp_Rotation;
        }
    }

    public override void ResetInteractEvent()
    {
        foreach (Movement movement in moveInteractionGroups)
        {
            if (movement.stateIndex == StateController.Instance.stateIndex)
            {
                if (movement.targetGameObject == null)
                    return;
                currentMovement = movement;
                ifFinished = false;
                stateIndex = movement.stateIndex;
                GetCurrentTransform(currentMovement);
                return;
            }
        }

        ifFinished = true;
    }

    private void GetCurrentTransform(Movement movement)
    {
        target_gameobject = movement.targetGameObject;
        target_Position = movement.targetGameObject.transform.position;
        target_Rotation = movement.targetGameObject.transform.rotation;
    }
}
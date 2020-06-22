using ProcessData;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Timeline;

/**********************************************
* 模块名: EventAsset.cs
* 功能描述：与交互触发的Event的参数有关的配置文件类
***********************************************/

[CreateAssetMenu(menuName = "EventAsset", fileName = "eventAsset", order = 1)]
public class EventAsset : ScriptableObject
{
    public string eventName;

    [Header("对应状态名称")]
    public string stateName;

    [Header("事件触发播放的效果音频")]
    [XmlIgnore] public AudioClip audioClip;

    public string audiClip_Path;

    [Header("事件触发播放的人声音频")]
    public AudioHintContent audioHintContent;

    public string audioHintClip_ID_Path;

    [Header("事件触发后播放的Timeline动画")]
    [XmlIgnore] public TimelineAsset timelineAsset;

    public string timelineAsset_Path;

    [Header("事件执行后激活的卡牌")]
    //[SerializeField]
    public int[] activeTargets;

    [Header("事件执行后禁用的卡牌")]
    public int[] disableTargets;

    [SerializeField]
    [Header("进入步骤改变图片的UI物体及替换的图片")]
    public ChangingOptions[] optionSprites;

    public string[] sprite_Paths;

    [Header("事件触发激活的物体名")]
    public EventObjectString[] gameObejcts_active;

    [Header("事件触发隐藏的物体名")]
    public EventObjectString[] gameObjects_hide;

    

    [Header("事件后改变的模型移动方式")]
    public MoveArea[] moveArea;

    [SerializeField]
    [Header("进入步骤替换新的模型")]
    public TargetSwap[] targetswap;

    [Header("事件是否会更新步骤状态")]
    public bool canLoop = false;

    [Header("触发事件时是否隐藏操作提示")]
    public bool hideOperationHint;

    [Header("触发事件时是否隐藏音频提示")]
    public bool hideAudioHint;

    [Header("错误信息及显示")]
    [SerializeField]
    public ErrorInfo errorInfo;

    [Header("显式指定下一步骤")]
    public NextStateInfo nextStateInfo;

    [HideInInspector]
    public bool ShowIninspector;
    [HideInInspector]
    public bool ShowIninspector2;
    [HideInInspector]
    public bool ShowIninspector3;
    [HideInInspector]
    public bool ShowIninspector4;

}

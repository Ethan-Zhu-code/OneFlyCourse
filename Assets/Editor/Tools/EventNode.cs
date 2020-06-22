using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using UnityEngine.Timeline;
using ProcessData;
using OneFlyLib;

public class EventNode : BaseNode
{
    Vector3 scrollPos = Vector2.zero;

    private EventNode eventNode;

    private string stateName = "";
    private StateInputNode stateInputNode;

    private Rect inputRect;

    protected int uiPrefabDownCount;
    public string[] uiPrefabDown=new string[0];

    public AudioClip audioClip;

    public TimelineAsset timelineAsset;

    public AudioHintContent audioHintContent;

    public bool canLoop = false;


    public int gameObejcts_activeCount;
    public bool[] gameObejcts_activeFold=new bool[99];
    public EventObjectString[] gameObejcts_active = new EventObjectString[0];


    public int gameObjects_hideCount;
    public bool[] gameObjects_hideCountFold = new bool[99];
    public EventObjectString[] gameObjects_hide = new EventObjectString[0];

    public ErrorInfo errorInfo;
    public int moveAreasCount;
    public MoveArea[] moveAreas = new MoveArea[0];

    public bool hideOperationHint;
    public bool hideAudioHint;
    public bool triggerWhilePlaying;
    public NextStateInfo nextStateInfo;

    public string controllerName="";

    public string path;

    public EventAsset eventAsset;



    [Header("事件执行后激活的卡牌")]
    public bool activeTargetsfold;
    public int activeTargetsCount;
    public int[] activeTargets=new int[0];

    [Header("事件执行后禁用的卡牌")]
    public bool disableTargetsfold;
    public int disableTargetsCount;
    public int[] disableTargets = new int[0];

    [Header("进入步骤改变的UI选项及图片")]
    public int optionSpriteCount;
    bool optionSpritefoldout;
    bool[] ChangingOptionsElementsfold = new bool[99];
    public ChangingOptions[] optionSprite = new ChangingOptions[0];





    bool audiofoldout;
    bool errorfoldout;
    bool moveAreafoldout;
    bool[] tempAreaElementsfold=new bool[99];
    bool nextStateInfofold;



    bool loadAssetfold;


    public EventNode()
    {
        eventNode = this;
        windowTitle = "Event 节点";
    }

    public override void DrawWindow() //绘制窗口
    {
        scrollPos = GUI.BeginScrollView(new Rect(0, 0, 250, 300),
     scrollPos, new Rect(0, 0, 0, 2000));

        base.DrawWindow();
        loadAssetfold = EditorGUILayout.Foldout(loadAssetfold, "读取现有EventAsset");
        if (loadAssetfold)
        {
            EditorGUILayout.HelpBox("三思而后行", MessageType.Warning, true);
            eventAsset = EditorGUILayout.ObjectField(eventAsset, typeof(EventAsset), false, GUILayout.Width(150)) as EventAsset;
            if (GUILayout.Button("读取读取现有EventAsset文件"))
            {
                LoadEventAsset(eventAsset);
            }
            GUILayout.Space(10);
        }


        if (stateInputNode != null)
        {
            stateName = stateInputNode.GetStateName();
            EditorGUILayout.LabelField("StateName");
           EditorGUILayout.LabelField(stateName);
        }
        else
        {
            EditorGUILayout.LabelField("  ");
            EditorGUILayout.HelpBox("未连接State节点", MessageType.Error, true);
        }
        if (Event.current.type == EventType.Repaint)
        {
            inputRect = GUILayoutUtility.GetLastRect();
        }


        if (GUILayout.Button("Build Asset"))
        {
            path = "Assets/Resources/Courses/Assets" + stateName;
            SaveAsset(controllerName+"_"+stateName, path);
            Debug.Log("Build");
        }
        if (controllerName == "")
        {
            EditorGUILayout.HelpBox("未设定Event名称", MessageType.Error, true);
        }
        EditorGUILayout.LabelField("ControllerName");
      
        controllerName = EditorGUILayout.TextField(controllerName, GUILayout.Width(150));
      


        EditorGUILayout.LabelField("事件触发播放的效果音频");
        audioClip = EditorGUILayout.ObjectField(audioClip, typeof(AudioClip), false, GUILayout.Width(150)) as AudioClip;
        // EditorGUILayout.LabelField("事件触发播放的人声音频");
        GUILayout.Space(5);
        // EditorGUILayout.BeginHorizontal();
        audiofoldout = EditorGUILayout.Foldout(audiofoldout, "事件触发播放的人声音频");
        if (audiofoldout)
        {
            EditorGUILayout.LabelField("右上角音频提示显示的文字内容");
            EditorGUILayout.LabelField("(为空则只播放语言不显示文字）");
            audioHintContent.audioHintText = EditorGUILayout.TextField(audioHintContent.audioHintText, GUILayout.Width(150));
            EditorGUILayout.LabelField("音频文件");
            audioHintContent.audioHintClip = EditorGUILayout.ObjectField(audioHintContent.audioHintClip, typeof(AudioClip), false, GUILayout.Width(150)) as AudioClip;
            // EditorGUILayout.EndHorizontal();
        }
        GUILayout.Space(5);
        EditorGUILayout.LabelField("事件触发后播放的Timeline动画");
        timelineAsset = EditorGUILayout.ObjectField(timelineAsset, typeof(TimelineAsset), false, GUILayout.Width(150)) as TimelineAsset;
        GUILayout.Space(5);
        canLoop = EditorGUILayout.Toggle("事件是否会重复步骤状态(不更新State)", canLoop);


        GUILayout.Space(5);
        activeTargetsfold = EditorGUILayout.Foldout(activeTargetsfold, "激活的卡牌编号");
        if (activeTargetsfold)
        {
            activeTargetsCount = EditorGUILayout.IntField(activeTargetsCount, GUILayout.Width(30));
            if (activeTargetsCount > 0)
            {
                if (activeTargetsCount != activeTargets.Length)
                {
                    if (activeTargets.Length == 0)
                    {
                        activeTargets = new int[activeTargetsCount];
                    }
                    else if (activeTargets.Length < activeTargetsCount)
                    {
                        int[] temp = activeTargets;
                        activeTargets = new int[activeTargetsCount];
                        for (int i = 0; i < temp.Length; i++)
                        {
                            activeTargets[i] = temp[i];
                        }
                    }
                }

                for (int i = 0; i < activeTargetsCount; i++)
                {
                    activeTargets[i] = EditorGUILayout.IntField(activeTargets[i], GUILayout.Width(200));

                }
            }

        }

        disableTargetsfold = EditorGUILayout.Foldout(disableTargetsfold, "隐藏的卡牌编号");
        if (disableTargetsfold)
        {
            disableTargetsCount = EditorGUILayout.IntField(disableTargetsCount, GUILayout.Width(30));
            if (disableTargetsCount > 0)
            {
                if (disableTargetsCount != disableTargets.Length)
                {
                    if (disableTargets.Length == 0)
                    {
                        disableTargets = new int[disableTargetsCount];
                    }
                    else if (disableTargets.Length < disableTargetsCount)
                    {
                        int[] temp = disableTargets;
                        disableTargets = new int[disableTargetsCount];
                        for (int i = 0; i < temp.Length; i++)
                        {
                            disableTargets[i] = temp[i];
                        }
                    }
                }

                for (int i = 0; i < disableTargetsCount; i++)
                {
                    disableTargets[i] = EditorGUILayout.IntField(disableTargets[i], GUILayout.Width(200));

                }
            }

        }







        EditorGUILayout.LabelField("事件触发激活的物体名");
        gameObejcts_activeCount = EditorGUILayout.IntField(gameObejcts_activeCount, GUILayout.Width(30));
        if (gameObejcts_activeCount > 0)
        {
            if (gameObejcts_activeCount != gameObejcts_active.Length)
            {
                if (gameObejcts_active.Length == 0)
                {
                    gameObejcts_active = new EventObjectString[gameObejcts_activeCount];
                }
                else if (gameObejcts_active.Length < gameObejcts_activeCount)
                {
                    EventObjectString[] objectString = gameObejcts_active;

                    string[] temp =new string[objectString.Length];
                    for (int i = 0; i < objectString.Length; i++)
                    {
                        temp[i] = objectString[i].name;
                    }                 
                    gameObejcts_active = new EventObjectString[gameObejcts_activeCount];


                    TriggerTiming[] actives = new TriggerTiming[objectString.Length];
                    for (int i = 0; i < objectString.Length; i++)
                    {
                        actives[i] = objectString[i].triggerTiming;
                    }


                    for (int i = 0; i < temp.Length; i++)
                    {
                        gameObejcts_active[i].name = temp[i];
                        gameObejcts_active[i].triggerTiming = actives[i];
                    }
                }
            }

            for (int i = 0; i < gameObejcts_activeCount; i++)
            {
                gameObejcts_activeFold[i] = EditorGUILayout.Foldout(gameObejcts_activeFold[i], "GameObject" + i.ToString());
                if (gameObejcts_activeFold[i])
                {
                    EditorGUILayout.LabelField("物体的名称");
                    gameObejcts_active[i].name = EditorGUILayout.TextField(gameObejcts_active[i].name, GUILayout.Width(150)) ;
                    EditorGUILayout.LabelField("激活次序");
                    gameObejcts_active[i].triggerTiming = (TriggerTiming)EditorGUILayout.EnumPopup(
 gameObejcts_active[i].triggerTiming, GUILayout.Width(120));
                }
 
            }
        }





        EditorGUILayout.LabelField("事件触发后隐藏的物体名");
        gameObjects_hideCount = EditorGUILayout.IntField(gameObjects_hideCount, GUILayout.Width(30));
        if (gameObjects_hideCount > 0)
        {
            if (gameObjects_hideCount != gameObjects_hide.Length)
            {
                if (gameObjects_hide.Length == 0)
                {
                    gameObjects_hide = new EventObjectString[gameObjects_hideCount];
                }
                else if (gameObjects_hide.Length < gameObjects_hideCount)
                {
                    EventObjectString[] objectString = gameObjects_hide;

                    string[] temp = new string[objectString.Length];
                    for (int i = 0; i < objectString.Length; i++)
                    {
                        temp[i] = objectString[i].name;
                    }
                    gameObjects_hide = new EventObjectString[gameObjects_hideCount];


                    TriggerTiming[] actives = new TriggerTiming[objectString.Length];
                    for (int i = 0; i < objectString.Length; i++)
                    {
                        actives[i] = objectString[i].triggerTiming;
                    }


                    for (int i = 0; i < temp.Length; i++)
                    {
                        gameObjects_hide[i].name = temp[i];
                        gameObjects_hide[i].triggerTiming = actives[i];
                    }
                }
            }

            for (int i = 0; i < gameObjects_hideCount; i++)
            {
                gameObjects_hideCountFold[i] = EditorGUILayout.Foldout(gameObjects_hideCountFold[i], "GameObject" + i.ToString());
                if (gameObjects_hideCountFold[i])
                {
                    EditorGUILayout.LabelField("物体的名称");
                    gameObjects_hide[i].name = EditorGUILayout.TextField(gameObjects_hide[i].name, GUILayout.Width(150));
                    EditorGUILayout.LabelField("隐藏次序");
                    gameObjects_hide[i].triggerTiming = (TriggerTiming)EditorGUILayout.EnumPopup(
 gameObjects_hide[i].triggerTiming, GUILayout.Width(120));
                }

            }
        }

        GUILayout.Space(5);
        optionSpritefoldout = EditorGUILayout.Foldout(optionSpritefoldout, "进入步骤改变 的UI选项及图片");
        if (optionSpritefoldout)
        {
            optionSpriteCount = EditorGUILayout.IntField(optionSpriteCount, GUILayout.Width(30));
            if (optionSpriteCount > 0)
            {
                if (optionSpriteCount != optionSprite.Length)
                {
                    if (optionSprite.Length == 0)
                    {
                        optionSprite = new ChangingOptions[optionSpriteCount];
                    }
                    else if (optionSprite.Length < optionSpriteCount)
                    {
                        ChangingOptions[] temp = optionSprite;
                        optionSprite = new ChangingOptions[optionSpriteCount];
                        for (int i = 0; i < temp.Length; i++)
                        {
                            optionSprite[i] = temp[i];
                        }
                    }
                }

                for (int i = 0; i < optionSpriteCount; i++)
                {

                    ChangingOptionsElementsfold[i] = EditorGUILayout.Foldout(ChangingOptionsElementsfold[i], "Element" + i.ToString());
                    if (ChangingOptionsElementsfold[i])
                    {
                        EditorGUILayout.LabelField("更换的sprite");
                        optionSprite[i].sprite = EditorGUILayout.ObjectField(optionSprite[i].sprite,
                            typeof(Sprite), false, GUILayout.Width(150)) as Sprite;
                        EditorGUILayout.LabelField("选项物体的名称");
                        optionSprite[i].optionName = EditorGUILayout.TextField(optionSprite[i].optionName, GUILayout.Width(200));
                    }

                }
            }
        }


        GUILayout.Space(5);
        errorfoldout = EditorGUILayout.Foldout(errorfoldout, "Error Info");
        if (errorfoldout)
        {
            EditorGUILayout.LabelField("错误在config文件中的序号");
            errorInfo.errorIndex = EditorGUILayout.IntField(errorInfo.errorIndex, GUILayout.Width(100));
            EditorGUILayout.LabelField("错误UI出现后停留的时间");
            errorInfo.hideHintTime = EditorGUILayout.FloatField(errorInfo.hideHintTime, GUILayout.Width(100));
            EditorGUILayout.LabelField("错误UI上显示的内容");
            errorInfo.errorContent = EditorGUILayout.TextArea(errorInfo.errorContent, GUILayout.ExpandHeight(true));
        }
        GUILayout.Space(5);
        moveAreafoldout = EditorGUILayout.Foldout(moveAreafoldout, "Move Area");
        if (moveAreafoldout)
        {
            moveAreasCount = EditorGUILayout.IntField(moveAreasCount, GUILayout.Width(30));
            if (moveAreasCount > 0)
            {
                if (moveAreasCount != moveAreas.Length)
                {
                    if (moveAreas.Length == 0)
                    {
                        moveAreas = new MoveArea[moveAreasCount];
                    }
                    else if (moveAreas.Length < moveAreasCount)
                    {
                        MoveArea[] temp = moveAreas;
                        moveAreas = new MoveArea[moveAreasCount];
                        for (int i = 0; i < temp.Length; i++)
                        {
                            moveAreas[i] = temp[i];
                        }
                    }
                }
                
                for (int i = 0; i < moveAreasCount; i++)
                {

                    tempAreaElementsfold[i] = EditorGUILayout.Foldout(tempAreaElementsfold[i], "Element"+i.ToString());
                    if (tempAreaElementsfold[i])
                    {
                        moveAreas[i].updatePos = EditorGUILayout.Toggle("是否更新物体位置", moveAreas[i].updatePos);
                        moveAreas[i].updateAngle = EditorGUILayout.Toggle("是否更新物体旋转角度", moveAreas[i].updateAngle);
                        EditorGUILayout.LabelField("模型在TargetControlCenter链表中的下标");
                        moveAreas[i].index = EditorGUILayout.IntField( moveAreas[i].index, GUILayout.Width(30));
                        moveAreas[i].changeArea = EditorGUILayout.Toggle("是否改变移动范围", moveAreas[i].changeArea);
                        if (moveAreas[i].changeArea)
                        {
                            EditorGUILayout.BeginHorizontal();
                            GUILayout.Label("minX", GUILayout.Width(35));
                            moveAreas[i].minX = EditorGUILayout.FloatField( moveAreas[i].minX, GUILayout.Width(35));
                            GUILayout.Label("minY", GUILayout.Width(35));
                            moveAreas[i].minY = EditorGUILayout.FloatField(moveAreas[i].minY, GUILayout.Width(35));
                            GUILayout.Label("minZ", GUILayout.Width(35));
                            moveAreas[i].minZ = EditorGUILayout.FloatField( moveAreas[i].minZ, GUILayout.Width(35));
                            EditorGUILayout.EndHorizontal();
                            EditorGUILayout.BeginHorizontal();
                            GUILayout.Label("maxX", GUILayout.Width(35));
                            moveAreas[i].maxX = EditorGUILayout.FloatField(moveAreas[i].maxX, GUILayout.Width(35));
                            GUILayout.Label("maxY", GUILayout.Width(35));
                            moveAreas[i].maxY = EditorGUILayout.FloatField(moveAreas[i].maxY, GUILayout.Width(35));
                            GUILayout.Label("maxZ", GUILayout.Width(35));
                            moveAreas[i].maxZ = EditorGUILayout.FloatField(moveAreas[i].maxZ, GUILayout.Width(35));
                            EditorGUILayout.EndHorizontal();
                        }
                        moveAreas[i].changeMoveSpace = EditorGUILayout.Toggle("是否改变移动轴向", moveAreas[i].changeMoveSpace);
                        if (moveAreas[i].changeMoveSpace)
                        {
                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField("是否改变移动轴向", GUILayout.Width(80));
                            moveAreas[i].moveSpace = (MOVE_SPACE)EditorGUILayout.EnumPopup(moveAreas[i].moveSpace, GUILayout.Width(80));
                            EditorGUILayout.EndHorizontal();
                        }
                        moveAreas[i].changeRatio = EditorGUILayout.Toggle("是否改变移动比例", moveAreas[i].changeRatio);
                        if (moveAreas[i].changeRatio)
                        {
                            EditorGUILayout.BeginHorizontal();
                            GUILayout.Label("物体随卡牌移动的比例", GUILayout.Width(120));
                            moveAreas[i].ratio = EditorGUILayout.FloatField(moveAreas[i].ratio, GUILayout.Width(50));
                            EditorGUILayout.EndHorizontal();
                        }
                        moveAreas[i].removable = EditorGUILayout.Toggle("物体是否会随着卡牌取走而消失", moveAreas[i].removable);
                    }

                }
            }
        }
        GUILayout.Space(5);
        hideOperationHint = EditorGUILayout.Toggle("触发事件时是否隐藏操作提示", hideOperationHint);
        hideAudioHint = EditorGUILayout.Toggle("触发事件时是否隐藏音频提示", hideAudioHint);
        triggerWhilePlaying = EditorGUILayout.Toggle("是否能在timeline运行时触发", triggerWhilePlaying);
        GUILayout.Space(5);
        nextStateInfofold = EditorGUILayout.Foldout(nextStateInfofold, "显式指定下一步骤");
        if (nextStateInfofold)
        {
            EditorGUILayout.LabelField("手动指定下一步骤（默认读取下一步骤）");
            nextStateInfo.loadAnotherState = EditorGUILayout.Toggle( nextStateInfo.loadAnotherState);
            EditorGUILayout.LabelField("下一步骤的编号");
            EditorGUILayout.LabelField("（StateController中StateAsset的下标）");
            nextStateInfo.index = EditorGUILayout.IntField(nextStateInfo.index, GUILayout.Width(30));
        }
        GUI.EndScrollView();
    }





    public void SaveAsset(string saveName, string path)
    {
        EventAsset stateAsset = ScriptableObject.CreateInstance<EventAsset>();
        stateAsset.name = saveName;

        if (saveName == "" || path == "")
        {
            Debug.LogError(string.Format("[UConfig]: 创建失败 , 信息不完整!"));
            return;
        }
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        path = path + "/" + saveName+"_Event" + ".asset";
        saveName = saveName.Replace(".asset", "");
        UnityEditor.AssetDatabase.CreateAsset(stateAsset, path);
        UnityEditor.AssetDatabase.SaveAssets();
        UnityEditor.AssetDatabase.Refresh();


        EventAsset asset = (EventAsset)Resources.Load("Courses/" + stateName + "/" + saveName+"_Event");

        //Edit This
        asset.stateName = stateName;
        asset.audioClip = audioClip;
        asset.timelineAsset = timelineAsset;
        asset.canLoop = canLoop;
        asset.audioHintContent = audioHintContent;
        asset.gameObejcts_active = gameObejcts_active;
        asset.gameObjects_hide = gameObjects_hide;
        asset.disableTargets = disableTargets;
        asset.activeTargets = activeTargets;
        //asset.objectsID_active = objectsID_active;
        // asset.objectsID_hide = objectsID_hide;
        asset.errorInfo = errorInfo;
        asset.moveArea = moveAreas;
        asset.hideOperationHint = hideOperationHint;
        asset.hideAudioHint = hideAudioHint;
        //asset.triggerWhilePlaying = triggerWhilePlaying;
        asset.nextStateInfo = nextStateInfo;

    }


    void LoadEventAsset(EventAsset asset)
    {
        audioClip = asset.audioClip;
        timelineAsset = asset.timelineAsset;
        canLoop = asset.canLoop;
        audioHintContent = asset.audioHintContent;
        disableTargets = asset.disableTargets;
        activeTargets = asset.activeTargets;
        // objectsID_active = asset.objectsID_active;
        //   objectsID_hide = asset.objectsID_hide;
        gameObejcts_active = asset.gameObejcts_active;
        gameObjects_hide = asset.gameObjects_hide;
        errorInfo = asset.errorInfo;
        moveAreas = asset.moveArea;
        hideOperationHint = asset.hideOperationHint;
        hideAudioHint = asset.hideAudioHint;
        //triggerWhilePlaying = asset.triggerWhilePlaying;
        nextStateInfo = asset.nextStateInfo;
    }


    public override void SetInput(StateInputNode inputNode, Vector2 mousePos)
    {
        mousePos.x -= WindowRect.x;
        mousePos.y -= WindowRect.y;

        //获取我们的输入结点的引用
        //如果我们的鼠标点击在了OutputNode 的 input1 的文本框的 Rect 中时 执行的操作
        if (inputRect.Contains(mousePos))
        {
            Debug.Log("Enter");
            //将输入结点的引用给OutputNode
           stateInputNode = inputNode;
        }
        Debug.Log("Enter");
        //将输入结点的引用给OutputNode
        stateInputNode = inputNode;

        inputNode = null;
    }

    public override void DrawBezier()
    {
        if (stateInputNode != null)
        {
            Rect rect = inputRect;
            rect.x = rect.x + WindowRect.x;
            rect.y = rect.y + WindowRect.y;
            rect.width = 1;
            rect.height = 1;

            //rect.x += WindowRect.x 简化的写法
            AssetGenerateEditor.DrawBezier(stateInputNode.WindowRect, rect);
        }
    }
    public override void DeleteNode(BaseNode node)
    {
        if (stateInputNode!=null)
        {
            if (stateInputNode.eventNodes.Contains(eventNode))
            {
                stateInputNode.eventNodes.Remove(eventNode);
            }
            if (stateInputNode == node)
            {
                stateInputNode = null;

            }
        }
     
    }

}

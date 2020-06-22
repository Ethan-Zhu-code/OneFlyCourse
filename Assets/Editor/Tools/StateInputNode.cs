using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using ProcessData;
using OneFlyLib;
using UnityEngine.Timeline;
using UnityEngine.Audio;

public class StateInputNode : BaseNode {

    Vector3 scrollPos = Vector2.zero;
    public string stateName= "";
    private Rect inputRect;
    

    protected int uiPrefabUpCount;    
    public string[] uiPrefabUp=new string[0];

    public List<EventNode> eventNodes = new List<EventNode>();

    public int moveAreasCount;
    bool moveAreafoldout;
    bool[] tempAreaElementsfold = new bool[99];
    public MoveArea[] moveArea = new MoveArea[0];

    public int optionSpriteCount;
    bool optionSpritefoldout;
    bool[] ChangingOptionsElementsfold = new bool[99];
    public ChangingOptions[] optionSprite = new ChangingOptions[0];

    bool audiofoldout;
    public AudioHintContent audioHintContent;

    public string operationHintText;

    public int targetswapCount;
    bool targetswapfoldout;
    bool[] TargetSwapElementsfold = new bool[99];
    public TargetSwap[] targetswap = new TargetSwap[0];

    //[Header("步骤开始激活的物体名称")]
    bool uiPrefabUp_Active_Enterfoldout;
    public int uiPrefabUp_Active_EnterCount;
    public string[] uiPrefabUp_Active_Enter=new string[0];
    // [Header("步骤开始隐藏的物体名称")]
    bool uiPrefabUp_Hide_Enterfoldout;
    public int uiPrefabUp_Hide_EnterCount;
    public string[] uiPrefabUp_Hide_Enter = new string[0];
    //  [Header("步骤结束激活的物体名称")]
    bool uiPrefabUp_Active_Exitfoldout;
    public int uiPrefabUp_Active_ExitCount;
    public string[] uiPrefabUp_Active_Exit = new string[0];
    //  [Header("步骤结束隐藏的物体名称")]
    bool uiPrefabUp_Hide_Exitfoldout;
    public int uiPrefabUp_Hide_ExitCount;
    public string[] uiPrefabUp_Hide_Exit = new string[0];


    // [Header("进入新步骤激活的卡牌编号")]
    bool activeTargets_Enterfoldout;
    public int activeTargets_EnterCount;
    public int[] activeTargets_Enter = new int[0];
    //[Header("进入新步骤禁用的卡牌编号")]
    bool disableTargets_Enterfoldout;
    public int disableTargets_EnterCount;
    public int[] disableTargets_Enter = new int[0];

    //[Header("结束步骤激活的卡牌编号")]
    bool activeTargets_Exitfoldout;
    public int activeTargets_ExitCount;
    public int[] activeTargets_Exit = new int[0];
    //[Header("结束步骤禁用的卡牌编号")]
    bool disableTargets_Exitfoldout;
    public int disableTargets_ExitCount;
    public int[] disableTargets_Exit=new int[0];

    public int gameObejcts_activeCount;
    public bool[] gameObejcts_activeFold = new bool[99];
    public EventObjectString[] gameObejcts_active;

    public int gameObjects_hideCount;
    public bool[] gameObjects_hideCountFold = new bool[99];
    public EventObjectString[] gameObjects_hide;


    //[Header("进入步骤后播放的Timeline（可无，播放完后才会进行进入新步骤初始化）")]
    public TimelineAsset enterStateTimeline;
   // [Header("结束步骤后播放的Timeline（可无，播放完后才会进入下一步骤）")]
    public TimelineAsset exitStateTimeline;



    protected int effectsCount;
    public bool[] effects=new bool[0];

   // [Header("课程进度，该步骤结束后设置，若为0则不会更新进度")]
    public float progress;


    protected string path ;


    bool coundNodefoldout;
    bool loadAssetfold;

    public StateAsset stateAsset;

    public StateInputNode()
    {
        windowTitle = "State 节点";
    }

    void OnEnable()
    {
        path = "Assets/Resources/Courses/";
    }

    public override void DrawWindow() //绘制窗口
    {
        for (int i = 0; i < eventNodes.Count; i++)
        {
            if (!eventNodes.Contains(eventNodes[i]))
            {
                eventNodes.Remove(eventNodes[i]);
            }
        }
       

        scrollPos = GUI.BeginScrollView(new Rect(0, 0, 250, 300),
       scrollPos, new Rect(0, 0, 0, 2000));
        base.DrawWindow();

        loadAssetfold = EditorGUILayout.Foldout(loadAssetfold, "读取现有StatetAsset");
        if (loadAssetfold)
        {
            EditorGUILayout.HelpBox("三思而后行", MessageType.Warning, true);
            stateAsset = EditorGUILayout.ObjectField(stateAsset, typeof(StateAsset), false, GUILayout.Width(150)) as StateAsset;
            if (GUILayout.Button("读取读取现有EventAsset文件"))
            {
                LoadStateAsset(stateAsset);
            }
            GUILayout.Space(10);
        }

        if (stateName=="")
        {
            EditorGUILayout.HelpBox("未添加StateName", MessageType.Error, true);
        }
        if (GUILayout.Button("Build Asset"))
        {
            path = "Assets/Resources/Courses/" + stateName;
            SaveAsset(stateName, path);
            Debug.Log("Build");
        }
        if (GUILayout.Button("Build All Asset"))
        {
            path = "Assets/Resources/Courses/" + stateName;
            SaveAsset(stateName, path);
            if (eventNodes.Count > 0)
            {
                foreach (EventNode node in eventNodes)
                {
                    node.path = "Assets/Resources/Courses/" + stateName;
                    node.SaveAsset(node.controllerName + "_" + stateName, node.path);
                }
            }

            Debug.Log("Build All");
        }
        EditorGUILayout.LabelField("State状态名称");
        stateName = EditorGUILayout.TextField( stateName,GUILayout.Width(200));

        //moveArea
        GUILayout.Space(5);
        moveAreafoldout = EditorGUILayout.Foldout(moveAreafoldout, "进入步骤后改变的模型移动方式");
        if (moveAreafoldout)
        {
            moveAreasCount = EditorGUILayout.IntField(moveAreasCount, GUILayout.Width(30));
            if (moveAreasCount > 0)
            {
                if (moveAreasCount != moveArea.Length)
                {
                    if (moveArea.Length == 0)
                    {
                        moveArea = new MoveArea[moveAreasCount];
                    }
                    else if (moveArea.Length < moveAreasCount)
                    {
                        MoveArea[] temp = moveArea;
                        moveArea = new MoveArea[moveAreasCount];
                        for (int i = 0; i < temp.Length; i++)
                        {
                            moveArea[i] = temp[i];
                        }
                    }
                }

                for (int i = 0; i < moveAreasCount; i++)
                {

                    tempAreaElementsfold[i] = EditorGUILayout.Foldout(tempAreaElementsfold[i], "Element" + i.ToString());
                    if (tempAreaElementsfold[i])
                    {
                        moveArea[i].updatePos = EditorGUILayout.Toggle("是否更新物体位置", moveArea[i].updatePos);
                        moveArea[i].updateAngle = EditorGUILayout.Toggle("是否更新物体旋转角度", moveArea[i].updateAngle);
                        EditorGUILayout.LabelField("模型在TargetControlCenter链表中的下标");
                        moveArea[i].index = EditorGUILayout.IntField(moveArea[i].index, GUILayout.Width(30));
                        moveArea[i].changeArea = EditorGUILayout.Toggle("是否改变移动范围", moveArea[i].changeArea);
                        if (moveArea[i].changeArea)
                        {
                            EditorGUILayout.BeginHorizontal();
                            GUILayout.Label("minX", GUILayout.Width(35));
                            moveArea[i].minX = EditorGUILayout.FloatField(moveArea[i].minX, GUILayout.Width(35));
                            GUILayout.Label("minY", GUILayout.Width(35));
                            moveArea[i].minY = EditorGUILayout.FloatField(moveArea[i].minY, GUILayout.Width(35));
                            GUILayout.Label("minZ", GUILayout.Width(35));
                            moveArea[i].minZ = EditorGUILayout.FloatField(moveArea[i].minZ, GUILayout.Width(35));
                            EditorGUILayout.EndHorizontal();
                            EditorGUILayout.BeginHorizontal();
                            GUILayout.Label("maxX", GUILayout.Width(35));
                            moveArea[i].maxX = EditorGUILayout.FloatField(moveArea[i].maxX, GUILayout.Width(35));
                            GUILayout.Label("maxY", GUILayout.Width(35));
                            moveArea[i].maxY = EditorGUILayout.FloatField(moveArea[i].maxY, GUILayout.Width(35));
                            GUILayout.Label("maxZ", GUILayout.Width(35));
                            moveArea[i].maxZ = EditorGUILayout.FloatField(moveArea[i].maxZ, GUILayout.Width(35));
                            EditorGUILayout.EndHorizontal();
                        }
                        moveArea[i].changeMoveSpace = EditorGUILayout.Toggle("是否改变移动轴向", moveArea[i].changeMoveSpace);
                        if (moveArea[i].changeMoveSpace)
                        {
                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField("是否改变移动轴向", GUILayout.Width(80));
                            moveArea[i].moveSpace = (MOVE_SPACE)EditorGUILayout.EnumPopup(moveArea[i].moveSpace, GUILayout.Width(80));
                            EditorGUILayout.EndHorizontal();
                        }
                        moveArea[i].changeRatio = EditorGUILayout.Toggle("是否改变移动比例", moveArea[i].changeRatio);
                        if (moveArea[i].changeRatio)
                        {
                            EditorGUILayout.BeginHorizontal();
                            GUILayout.Label("物体随卡牌移动的比例", GUILayout.Width(120));
                            moveArea[i].ratio = EditorGUILayout.FloatField(moveArea[i].ratio, GUILayout.Width(50));
                            EditorGUILayout.EndHorizontal();
                        }
                        moveArea[i].removable = EditorGUILayout.Toggle("物体是否会随着卡牌取走而消失", moveArea[i].removable);
                    }

                }
            }
        }
        GUILayout.Space(5);

        //optionSprite
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
                        optionSprite[i].optionName = EditorGUILayout.TextField(optionSprite[i].optionName, GUILayout.Width(200)) ;
                    }

                }
            }
        }
        GUILayout.Space(5);

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
        EditorGUILayout.LabelField("进入步骤后显示的左上角操作提示");
        operationHintText = EditorGUILayout.TextField(operationHintText, GUILayout.Width(150));

        //TargetSwap
        targetswapfoldout = EditorGUILayout.Foldout(targetswapfoldout, "进入步骤替换新的模型");
        if (targetswapfoldout)
        {
            targetswapCount = EditorGUILayout.IntField(targetswapCount, GUILayout.Width(30));
            if (targetswapCount > 0)
            {
                if (targetswapCount != targetswap.Length)
                {
                    if (targetswap.Length == 0)
                    {
                        targetswap = new TargetSwap[targetswapCount];
                    }
                    else if (targetswap.Length < targetswapCount)
                    {
                        TargetSwap[] temp = targetswap;
                        targetswap = new TargetSwap[targetswapCount];
                        for (int i = 0; i < temp.Length; i++)
                        {
                            targetswap[i] = temp[i];
                        }
                    }
                }

                for (int i = 0; i < targetswapCount; i++)
                {
                    TargetSwapElementsfold[i] = EditorGUILayout.Foldout(TargetSwapElementsfold[i], "Element" + i.ToString());
                    if (TargetSwapElementsfold[i])
                    {
                        EditorGUILayout.LabelField("模型在TargetControlCenter表中的下标");
                        targetswap[i].targetIndex = EditorGUILayout.IntField(targetswap[i].targetIndex, GUILayout.Width(80));
                        EditorGUILayout.LabelField("用于替换的新模型的物体名");
                        targetswap[i].newTargetName = EditorGUILayout.TextField(targetswap[i].newTargetName, GUILayout.Width(150));
                    }

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

                    string[] temp = new string[objectString.Length];
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
                    gameObejcts_active[i].name = EditorGUILayout.TextField(gameObejcts_active[i].name, GUILayout.Width(150));
                    EditorGUILayout.LabelField("激活次序");
                    gameObejcts_active[i].triggerTiming = (TriggerTiming)EditorGUILayout.EnumPopup(
 gameObejcts_active[i].triggerTiming, GUILayout.Width(120));
                }

            }
        }

        GUILayout.Space(5);



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
        uiPrefabUp_Active_Enterfoldout = EditorGUILayout.Foldout(uiPrefabUp_Active_Enterfoldout, "步骤开始激活的物体名称");
        if (uiPrefabUp_Active_Enterfoldout)
        {
            uiPrefabUp_Active_EnterCount = EditorGUILayout.IntField(uiPrefabUp_Active_EnterCount, GUILayout.Width(30));
            if (uiPrefabUp_Active_EnterCount > 0)
            {
                if (uiPrefabUp_Active_EnterCount != uiPrefabUp_Active_Enter.Length)
                {
                    if (uiPrefabUp_Active_Enter.Length == 0)
                    {
                        uiPrefabUp_Active_Enter = new string[uiPrefabUp_Active_EnterCount];
                    }
                    else if (uiPrefabUp_Active_Enter.Length < uiPrefabUp_Active_EnterCount)
                    {
                        string[] temp = uiPrefabUp_Active_Enter;
                        uiPrefabUp_Active_Enter = new string[uiPrefabUp_Active_EnterCount];
                        for (int i = 0; i < temp.Length; i++)
                        {
                            uiPrefabUp_Active_Enter[i] = temp[i];
                        }
                    }
                }

                for (int i = 0; i < uiPrefabUp_Active_EnterCount; i++)
                {
                    uiPrefabUp_Active_Enter[i] = EditorGUILayout.TextField(uiPrefabUp_Active_Enter[i], GUILayout.Width(200));

                }
            }

        }

        GUILayout.Space(5);
        uiPrefabUp_Hide_Enterfoldout = EditorGUILayout.Foldout(uiPrefabUp_Hide_Enterfoldout, "步骤开始隐藏的物体名称");
        if (uiPrefabUp_Hide_Enterfoldout)
        {
            uiPrefabUp_Hide_EnterCount = EditorGUILayout.IntField(uiPrefabUp_Hide_EnterCount, GUILayout.Width(30));
            if (uiPrefabUp_Hide_EnterCount > 0)
            {
                if (uiPrefabUp_Hide_EnterCount != uiPrefabUp_Hide_Enter.Length)
                {
                    if (uiPrefabUp_Hide_Enter.Length == 0)
                    {
                        uiPrefabUp_Hide_Enter = new string[uiPrefabUp_Hide_EnterCount];
                    }
                    else if (uiPrefabUp_Hide_Enter.Length < uiPrefabUp_Hide_EnterCount)
                    {
                        string[] temp = uiPrefabUp_Hide_Enter;
                        uiPrefabUp_Hide_Enter = new string[uiPrefabUp_Hide_EnterCount];
                        for (int i = 0; i < temp.Length; i++)
                        {
                            uiPrefabUp_Hide_Enter[i] = temp[i];
                        }
                    }
                }

                for (int i = 0; i < uiPrefabUp_Hide_EnterCount; i++)
                {
                    uiPrefabUp_Hide_Enter[i] = EditorGUILayout.TextField(uiPrefabUp_Hide_Enter[i], GUILayout.Width(200));

                }
            }

        }

        GUILayout.Space(5);
        uiPrefabUp_Active_Exitfoldout = EditorGUILayout.Foldout(uiPrefabUp_Active_Exitfoldout, "步骤结束激活的物体名称");
        if (uiPrefabUp_Active_Exitfoldout)
        {
            uiPrefabUp_Active_ExitCount = EditorGUILayout.IntField(uiPrefabUp_Active_ExitCount, GUILayout.Width(30));
            if (uiPrefabUp_Active_ExitCount > 0)
            {
                if (uiPrefabUp_Active_ExitCount != uiPrefabUp_Active_Exit.Length)
                {
                    if (uiPrefabUp_Active_Exit.Length == 0)
                    {
                        uiPrefabUp_Active_Exit = new string[uiPrefabUp_Active_ExitCount];
                    }
                    else if (uiPrefabUp_Active_Exit.Length < uiPrefabUp_Active_ExitCount)
                    {
                        string[] temp = uiPrefabUp_Active_Exit;
                        uiPrefabUp_Active_Exit = new string[uiPrefabUp_Active_ExitCount];
                        for (int i = 0; i < temp.Length; i++)
                        {
                            uiPrefabUp_Active_Exit[i] = temp[i];
                        }
                    }
                }

                for (int i = 0; i < uiPrefabUp_Active_ExitCount; i++)
                {
                    uiPrefabUp_Active_Exit[i] = EditorGUILayout.TextField(uiPrefabUp_Active_Exit[i], GUILayout.Width(200));

                }
            }

        }

        GUILayout.Space(5);
        uiPrefabUp_Hide_Exitfoldout = EditorGUILayout.Foldout(uiPrefabUp_Hide_Exitfoldout, "步骤结束隐藏的物体名称");
        if (uiPrefabUp_Hide_Exitfoldout)
        {
            uiPrefabUp_Hide_ExitCount = EditorGUILayout.IntField(uiPrefabUp_Hide_ExitCount, GUILayout.Width(30));
            if (uiPrefabUp_Hide_ExitCount > 0)
            {
                if (uiPrefabUp_Hide_ExitCount != uiPrefabUp_Hide_Exit.Length)
                {
                    if (uiPrefabUp_Hide_Exit.Length == 0)
                    {
                        uiPrefabUp_Hide_Exit = new string[uiPrefabUp_Hide_ExitCount];
                    }
                    else if (uiPrefabUp_Hide_Exit.Length < uiPrefabUp_Hide_ExitCount)
                    {
                        string[] temp = uiPrefabUp_Hide_Exit;
                        uiPrefabUp_Hide_Exit = new string[uiPrefabUp_Hide_ExitCount];
                        for (int i = 0; i < temp.Length; i++)
                        {
                            uiPrefabUp_Hide_Exit[i] = temp[i];
                        }
                    }
                }

                for (int i = 0; i < uiPrefabUp_Hide_ExitCount; i++)
                {
                    uiPrefabUp_Hide_Exit[i] = EditorGUILayout.TextField(uiPrefabUp_Hide_Exit[i], GUILayout.Width(200));

                }
            }

        }

        GUILayout.Space(5);
        activeTargets_Enterfoldout = EditorGUILayout.Foldout(activeTargets_Enterfoldout, "进入新步骤激活的卡牌编号");
        if (activeTargets_Enterfoldout)
        {
            activeTargets_EnterCount = EditorGUILayout.IntField(activeTargets_EnterCount, GUILayout.Width(30));
            if (activeTargets_EnterCount > 0)
            {
                if (activeTargets_EnterCount != activeTargets_Enter.Length)
                {
                    if (activeTargets_Enter.Length == 0)
                    {
                        activeTargets_Enter = new int[activeTargets_EnterCount];
                    }
                    else if (activeTargets_Enter.Length < activeTargets_EnterCount)
                    {
                        int[] temp = activeTargets_Enter;
                        activeTargets_Enter = new int[activeTargets_EnterCount];
                        for (int i = 0; i < temp.Length; i++)
                        {
                            activeTargets_Enter[i] = temp[i];
                        }
                    }
                }

                for (int i = 0; i < activeTargets_EnterCount; i++)
                {
                    activeTargets_Enter[i] = EditorGUILayout.IntField(activeTargets_Enter[i], GUILayout.Width(200));

                }
            }

        }

        GUILayout.Space(5);
        disableTargets_Enterfoldout = EditorGUILayout.Foldout(disableTargets_Enterfoldout, "进入新步骤禁用的卡牌编号");
        if (disableTargets_Enterfoldout)
        {
            disableTargets_EnterCount = EditorGUILayout.IntField(disableTargets_EnterCount, GUILayout.Width(30));
            if (disableTargets_EnterCount > 0)
            {
                if (disableTargets_EnterCount != disableTargets_Enter.Length)
                {
                    if (disableTargets_Enter.Length == 0)
                    {
                        disableTargets_Enter = new int[disableTargets_EnterCount];
                    }
                    else if (disableTargets_Enter.Length < disableTargets_EnterCount)
                    {
                        int[] temp = disableTargets_Enter;
                        disableTargets_Enter = new int[disableTargets_EnterCount];
                        for (int i = 0; i < temp.Length; i++)
                        {
                            disableTargets_Enter[i] = temp[i];
                        }
                    }
                }

                for (int i = 0; i < disableTargets_EnterCount; i++)
                {
                    disableTargets_Enter[i] = EditorGUILayout.IntField(disableTargets_Enter[i], GUILayout.Width(200));

                }
            }

        }

        GUILayout.Space(5);
        activeTargets_Exitfoldout = EditorGUILayout.Foldout(activeTargets_Exitfoldout, "结束步骤激活的卡牌编号");
        if (activeTargets_Exitfoldout)
        {
            activeTargets_ExitCount = EditorGUILayout.IntField(activeTargets_ExitCount, GUILayout.Width(30));
            if (activeTargets_ExitCount > 0)
            {
                if (activeTargets_ExitCount != activeTargets_Exit.Length)
                {
                    if (activeTargets_Exit.Length == 0)
                    {
                        activeTargets_Exit = new int[activeTargets_ExitCount];
                    }
                    else if (activeTargets_Exit.Length < activeTargets_ExitCount)
                    {
                        int[] temp = activeTargets_Exit;
                        activeTargets_Exit = new int[activeTargets_ExitCount];
                        for (int i = 0; i < temp.Length; i++)
                        {
                            activeTargets_Exit[i] = temp[i];
                        }
                    }
                }

                for (int i = 0; i < activeTargets_ExitCount; i++)
                {
                    activeTargets_Exit[i] = EditorGUILayout.IntField(activeTargets_Exit[i], GUILayout.Width(200));

                }
            }

        }

        GUILayout.Space(5);
        disableTargets_Exitfoldout = EditorGUILayout.Foldout(disableTargets_Exitfoldout, "结束步骤禁用的卡牌编号");
        if (disableTargets_Exitfoldout)
        {
            disableTargets_ExitCount = EditorGUILayout.IntField(disableTargets_ExitCount, GUILayout.Width(30));
            if (disableTargets_ExitCount > 0)
            {
                if (disableTargets_ExitCount != disableTargets_Exit.Length)
                {
                    if (disableTargets_Exit.Length == 0)
                    {
                        disableTargets_Exit = new int[disableTargets_ExitCount];
                    }
                    else if (disableTargets_Exit.Length < disableTargets_ExitCount)
                    {
                        int[] temp = disableTargets_Exit;
                        disableTargets_Exit = new int[disableTargets_ExitCount];
                        for (int i = 0; i < temp.Length; i++)
                        {
                            disableTargets_Exit[i] = temp[i];
                        }
                    }
                }

                for (int i = 0; i < disableTargets_ExitCount; i++)
                {
                    disableTargets_Exit[i] = EditorGUILayout.IntField(disableTargets_Exit[i], GUILayout.Width(200));

                }
            }

        }

        EditorGUILayout.LabelField("进入步骤后播放的Timeline");
        EditorGUILayout.LabelField("可无，播放完后才会进行进入新步骤初始化）");
        enterStateTimeline = EditorGUILayout.ObjectField(enterStateTimeline, typeof(TimelineAsset), false, GUILayout.Width(150)) as TimelineAsset;
        EditorGUILayout.LabelField("结束步骤后播放的Timeline");
        EditorGUILayout.LabelField("（可无，播放完后才会进入下一步骤）");
        exitStateTimeline = EditorGUILayout.ObjectField(exitStateTimeline, typeof(TimelineAsset), false, GUILayout.Width(150)) as TimelineAsset;
        coundNodefoldout = EditorGUILayout.Foldout(coundNodefoldout, "连接子节点数");
        effectsCount = eventNodes.Count;
        if (coundNodefoldout)
        {
            EditorGUILayout.LabelField("子节点数");
            // EditorGUILayout.IntField(eventNodes.Count);
            EditorGUILayout.LabelField(eventNodes.Count.ToString());
            EditorGUILayout.LabelField("步骤目标完成情况");
            //effectsCount = EditorGUILayout.IntField(eventNodes.Count, GUILayout.Width(25));
            int tempCount = 0;
            for (int i = 0; i < eventNodes.Count; i++)
            {
                if (!eventNodes[i].canLoop)
                {
                    tempCount++;
                }
            }
            effectsCount = tempCount;
            EditorGUILayout.LabelField(effectsCount.ToString());
            if (effectsCount > 0)
            {
                if (effectsCount != effects.Length)
                {
                    if (effects.Length == 0)
                    {
                        effects = new bool[effectsCount];
                    }
                    else if (effects.Length < effectsCount)
                    {
                        bool[] temp = effects;
                        effects = new bool[effectsCount];
                        for (int i = 0; i < temp.Length; i++)
                        {
                            effects[i] = temp[i];
                        }
                    }
                }
                //for (int i = 0; i < eventNodes.Count; i++)
                //{
                //    effects[i] = EditorGUILayout.Toggle(false);
                //}
            }
        }

        EditorGUILayout.LabelField("课程进度，该步骤结束后设置，若为0则不会更新进度");
        progress = EditorGUILayout.FloatField(progress, GUILayout.Width(150));

        if (effectsCount == 0)
        {
            EditorGUILayout.HelpBox("无连接子节点", MessageType.Warning, true);
        }


        GUI.EndScrollView();
    }

    public override void SetInput(EventNode inputNode, Vector2 mousePos)
    {
        mousePos.x -= WindowRect.x;
        mousePos.y -= WindowRect.y;

        //获取我们的输入结点的引用
        //如果我们的鼠标点击在了OutputNode 的 input1 的文本框的 Rect 中时 执行的操作
        if (inputRect.Contains(mousePos))
        {
           // Debug.Log("Enter");
           // //将输入结点的引用给OutputNode
           //eventNodes.Add(inputNode);
        }
        Debug.Log("Enter");
        //将输入结点的引用给OutputNode
        if (!eventNodes.Contains(inputNode))
        {
            eventNodes.Add(inputNode);
        }
        

        inputNode = null;
    }




    public string GetStateName()
    {
        return stateName;
    }


    private void SaveAsset(string saveName,string path)
    {
        StateAsset stateAsset = ScriptableObject.CreateInstance<StateAsset>();
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
        path = path  +"/"+ saveName + ".asset";
        saveName = saveName.Replace(".asset", "");
        UnityEditor.AssetDatabase.CreateAsset(stateAsset, path);
        UnityEditor.AssetDatabase.SaveAssets();
        UnityEditor.AssetDatabase.Refresh();


        StateAsset asset = (StateAsset)Resources.Load("Courses/" + stateName+ "/" + saveName);
        asset.stateName = stateName;
        asset.moveArea = moveArea;
        asset.optionSprite = optionSprite;
        asset.audioHintContent = audioHintContent;
        asset.operationHintText = operationHintText;
        asset.targetswap = targetswap;
        asset.gameObjects_Hide = gameObjects_hide;
        asset.gameObjects_Active = gameObejcts_active;
        asset.activeTargets_Enter = activeTargets_Enter;
        asset.disableTargets_Enter = disableTargets_Enter;
        asset.activeTargets_Exit = activeTargets_Exit;
        asset.disableTargets_Exit = disableTargets_Exit;
        asset.enterStateTimeline = enterStateTimeline;
        asset.exitStateTimeline = exitStateTimeline;
        asset.progress = progress;
        asset.effects = effects;
    }


    void  LoadStateAsset(StateAsset asset)
    {
        stateName = asset.stateName;
        moveArea = asset.moveArea;
        optionSprite = asset.optionSprite;
        audioHintContent = asset.audioHintContent;
        operationHintText = asset.operationHintText;
        targetswap = asset.targetswap;
        //uiPrefabUp_Active_Enter = asset.uiPrefabUp_Active_Enter;
        //uiPrefabUp_Hide_Enter = asset.uiPrefabUp_Hide_Enter;
        //uiPrefabUp_Active_Exit = asset.uiPrefabUp_Active_Exit;
        //uiPrefabUp_Hide_Exit = asset.uiPrefabUp_Hide_Exit;
        gameObjects_hide=asset.gameObjects_Hide ;
        gameObejcts_active  = asset.gameObjects_Active;

        activeTargets_Enter = asset.activeTargets_Enter;
        disableTargets_Enter = asset.disableTargets_Enter;
        activeTargets_Exit = asset.activeTargets_Exit;
        disableTargets_Exit = asset.disableTargets_Exit;
        enterStateTimeline = asset.enterStateTimeline;
        exitStateTimeline = asset.exitStateTimeline;
        progress = asset.progress;
        effects = asset.effects;

    }


}




//public string stateName;
//public MoveArea[] moveArea;
//public ChangingOptions[] optionSprite;
//public AudioHintContent audioHintContent;
//public string operationHintText;
//public TargetSwap[] targetswap;
//public string[] uiPrefabUp_Active_Enter;
//public string[] uiPrefabUp_Hide_Enter;
//public string[] uiPrefabUp_Active_Exit;
//public string[] uiPrefabUp_Hide_Exit;
//public int[] activeTargets_Enter;
//public int[] disableTargets_Enter;
//public int[] activeTargets_Exit;
//public int[] disableTargets_Exit;
//public TimelineAsset enterStateTimeline;
//public TimelineAsset exitStateTimeline;
//public bool[] effects;
//public float progress;
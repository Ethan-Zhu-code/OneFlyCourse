using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Universal.Card;

/**********************************************
* Copyright (C) 2019 讯飞幻境（北京）科技有限公司
* 模块名: ModelControlEditor.cs
* 创建者：RyuRae
* 修改者列表：
* 创建日期：
* 功能描述：
***********************************************/
[CustomEditor(typeof(ModelControl), true)]
[CanEditMultipleObjects]
public class ModelControlEditor : Editor {

    protected GUIStyle paddingStyle;
    protected Object monoScript;

    protected virtual void OnEnable()
    {
        paddingStyle = new GUIStyle();
        paddingStyle.padding = new RectOffset(15, 0, 0, 0);
        this.monoScript = MonoScript.FromMonoBehaviour(target as MonoBehaviour);
    }

    protected void DrawMonoScript()
    {
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.ObjectField("Script", this.monoScript, typeof(MonoScript), false);
        EditorGUI.EndDisabledGroup();
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        this.DrawMonoScript();

        EditorGUILayout.Space();
        ModelControl model = target as ModelControl;
                
        //model.isMappingGround = OneFlyGUITools.Toggle("是否根据地形移动(isMappingGround)", model.isMappingGround, true);
        EditorGUILayout.Space();
        if (!model.isMappingGround)
        {
            model.showMoveSetting = OneFlyGUITools.BeginFoldOut("移动设置面板", model.showMoveSetting);
            //EditorGUILayout.Space();
            if (model.showMoveSetting)
            {
                OneFlyGUITools.BeginGroup();
                { 
                    #region 不根据地形移动
                    EditorGUILayout.Space();                
                    model.moveSpace = (OneFlyLib.MOVE_SPACE)EditorGUILayout.EnumPopup("移动平面(moveSpace)", model.moveSpace);
                    EditorGUILayout.BeginVertical();
                    EditorGUILayout.Space();

                    switch (model.moveSpace)
                    {
                        case OneFlyLib.MOVE_SPACE.XY:
                            #region 移动类型
                            model.moveType = (MoveType)EditorGUILayout.EnumPopup("移动类型(moveType)", model.moveType);
                            EditorGUILayout.BeginVertical(paddingStyle);
                            //EditorGUILayout.Space();
                            switch (model.moveType)
                            {
                                case MoveType.REALTIME:
                                    #region 根据相机的实时移动（只适用于XY平面）
                                    model.isAccordCamera = OneFlyGUITools.Toggle("是否根据相机移动(isAccordCamera)", model.isAccordCamera, true);
                                    EditorGUILayout.BeginVertical(paddingStyle);
                                    EditorGUILayout.Space();
                                    if (model.isAccordCamera)
                                    {
                                        model.mainCam = (Camera)EditorGUILayout.ObjectField("主摄像机(mainCam)", model.mainCam, typeof(Camera), true);
                                    }
                                    else
                                    {
                                        EditorGUILayout.LabelField("x,y分别代表对应的轴向移动区域");
                                        model.moveRange = EditorGUILayout.Vector2Field("移动区域调整(moveRange)", model.moveRange);
                                    }
                                    EditorGUILayout.Space();
                                    EditorGUILayout.EndVertical();


                                    model.isLimit = EditorGUILayout.BeginToggleGroup("是否限制XY的值(isLimit)", model.isLimit);
                                    EditorGUILayout.BeginVertical(paddingStyle);
                                    EditorGUILayout.Space();

                                    if (model.isLimit)
                                    {
                                        model.minX = EditorGUILayout.FloatField("限定x轴移动最小值(minX)", model.minX);
                                        model.maxX = EditorGUILayout.FloatField("限定x轴移动最大值(maxX)", model.maxX);
                                        model.minY = EditorGUILayout.FloatField("限定y轴移动最小值(minY)", model.minY);
                                        model.maxY = EditorGUILayout.FloatField("限定y轴移动最大值(maxY)", model.maxY);
                                    }
                                    EditorGUILayout.EndVertical();
                                    EditorGUILayout.EndToggleGroup();
                                    #endregion
                                    break;
                                case MoveType.DELTA:
                                    #region 增量移动
                                    EditorGUILayout.LabelField("x,y分别代表对应的轴向移动区域");
                                    model.moveRange = EditorGUILayout.Vector2Field("移动区域调整(moveRange)", model.moveRange);
                                    //EditorGUILayout.Space();
                                    //model.ratio = EditorGUILayout.FloatField("增量移动缩放系数(ratio)", model.ratio);
                                    EditorGUILayout.Space();
                                    model.isLimit = EditorGUILayout.BeginToggleGroup("是否限制XY的值(isLimit)", model.isLimit);
                                    EditorGUILayout.BeginVertical(paddingStyle);
                                    EditorGUILayout.Space();
                                    if (model.isLimit)
                                    {
                                        model.minX = EditorGUILayout.FloatField("限定x轴移动最小值(minX)", model.minX);
                                        model.maxX = EditorGUILayout.FloatField("限定x轴移动最大值(maxX)", model.maxX);
                                        model.minY = EditorGUILayout.FloatField("限定y轴移动最小值(minY)", model.minY);
                                        model.maxY = EditorGUILayout.FloatField("限定y轴移动最大值(maxY)", model.maxY);
                                    }

                                    EditorGUILayout.EndToggleGroup();
                                    EditorGUILayout.EndVertical();
                                    #endregion
                                    break;
                            }
                            //EditorGUILayout.Space();
                            EditorGUILayout.EndVertical();

                            #endregion
                            break;
                        case OneFlyLib.MOVE_SPACE.XZ:
                            model.moveType = (MoveType)EditorGUILayout.EnumPopup("移动类型(moveType)", model.moveType);
                            if (model.moveType != MoveType.OTHER)
                            {
                                EditorGUILayout.BeginVertical(paddingStyle);
                                EditorGUILayout.Space();
                                model.ratio = EditorGUILayout.FloatField("增量移动缩放系数(ratio)", model.ratio);//
                                EditorGUILayout.Space();
                                //EditorGUILayout.LabelField("x代表x轴移动区域，y代表z轴移动区域");
                                model.moveRange = EditorGUILayout.Vector2Field("移动区域调整(moveRange)", model.moveRange);
                                EditorGUILayout.Space();
                                model.isLimit = EditorGUILayout.BeginToggleGroup("是否限制XZ的值(isLimit)", model.isLimit);
                                EditorGUILayout.BeginVertical(paddingStyle);
                                EditorGUILayout.Space();
                                if (model.isLimit)
                                {
                                    model.minX = EditorGUILayout.FloatField("限定x轴移动最小值(minX)", model.minX);
                                    model.maxX = EditorGUILayout.FloatField("限定x轴移动最大值(maxX)", model.maxX);
                                    model.minZ = EditorGUILayout.FloatField("限定z轴移动最小值(minZ)", model.minZ);
                                    model.maxZ = EditorGUILayout.FloatField("限定z轴移动最大值(maxZ)", model.maxZ);
                                }

                                EditorGUILayout.EndToggleGroup();
                                EditorGUILayout.EndVertical();
                                EditorGUILayout.EndVertical();
                            }
                            break;
                        case OneFlyLib.MOVE_SPACE.YZ:
                            model.moveType = (MoveType)EditorGUILayout.EnumPopup("移动类型(moveType)", model.moveType);
                            if (model.moveType != MoveType.OTHER)
                            {
                                EditorGUILayout.BeginVertical(paddingStyle);
                                EditorGUILayout.Space();
                                model.ratio = EditorGUILayout.FloatField("增量移动缩放系数(ratio)", model.ratio);
                                EditorGUILayout.Space();
                                //EditorGUILayout.LabelField("x代表z轴移动区域，y代表y轴移动区域");
                                model.moveRange = EditorGUILayout.Vector2Field("移动区域调整(moveRange)", model.moveRange);
                                EditorGUILayout.Space();
                                model.isLimit = EditorGUILayout.BeginToggleGroup("是否限制yZ的值(isLimit)", model.isLimit);
                                EditorGUILayout.BeginVertical(paddingStyle);
                                EditorGUILayout.Space();
                                if (model.isLimit)
                                {
                                    model.minY = EditorGUILayout.FloatField("限定y轴移动最小值(minY)", model.minY);
                                    model.maxY = EditorGUILayout.FloatField("限定y轴移动最大值(maxY)", model.maxY);
                                    model.minZ = EditorGUILayout.FloatField("限定z轴移动最小值(minZ)", model.minZ);
                                    model.maxZ = EditorGUILayout.FloatField("限定z轴移动最大值(maxZ)", model.maxZ);
                                }

                                EditorGUILayout.EndToggleGroup();
                                EditorGUILayout.EndVertical();
                                EditorGUILayout.EndVertical();
                            }
                            break;
                        case OneFlyLib.MOVE_SPACE.X:
                            model.moveType = (MoveType)EditorGUILayout.EnumPopup("移动类型(moveType)", model.moveType);
                            if (model.moveType != MoveType.OTHER)
                            {
                                EditorGUILayout.BeginVertical(paddingStyle);
                                EditorGUILayout.Space();
                                model.ratio = EditorGUILayout.FloatField("增量移动缩放系数(ratio)", model.ratio);
                                EditorGUILayout.Space();
                                //EditorGUILayout.LabelField("x代表x轴移动区域");
                                model.moveRange = EditorGUILayout.Vector2Field("移动区域调整(moveRange)", model.moveRange);
                                EditorGUILayout.Space();
                                model.isLimit = EditorGUILayout.BeginToggleGroup("是否限制x的值(isLimit)", model.isLimit);
                                EditorGUILayout.BeginVertical(paddingStyle);
                                EditorGUILayout.Space();
                                if (model.isLimit)
                                {
                                    model.minX = EditorGUILayout.FloatField("限定x轴移动最小值(minX)", model.minX);
                                    model.maxX = EditorGUILayout.FloatField("限定x轴移动最大值(maxX)", model.maxX);
                                }

                                EditorGUILayout.EndToggleGroup();
                                EditorGUILayout.EndVertical();
                                EditorGUILayout.EndVertical();
                            }
                            break;
                        case OneFlyLib.MOVE_SPACE.Y:
                            model.moveType = (MoveType)EditorGUILayout.EnumPopup("移动类型(moveType)", model.moveType);
                            if (model.moveType != MoveType.OTHER)
                            {
                                EditorGUILayout.BeginVertical(paddingStyle);
                                EditorGUILayout.Space();
                                model.ratio = EditorGUILayout.FloatField("增量移动缩放系数(ratio)", model.ratio);
                                EditorGUILayout.Space();
                                //EditorGUILayout.LabelField("y代表y轴移动区域");
                                model.moveRange = EditorGUILayout.Vector2Field("移动区域调整(moveRange)", model.moveRange);
                                EditorGUILayout.Space();
                                model.isLimit = EditorGUILayout.BeginToggleGroup("是否限制y的值(isLimit)", model.isLimit);
                                EditorGUILayout.BeginVertical(paddingStyle);
                                EditorGUILayout.Space();
                                if (model.isLimit)
                                {
                                    model.minY = EditorGUILayout.FloatField("限定y轴移动最小值(minY)", model.minY);
                                    model.maxY = EditorGUILayout.FloatField("限定y轴移动最大值(maxY)", model.maxY);
                                }

                                EditorGUILayout.EndToggleGroup();
                                EditorGUILayout.EndVertical();
                                EditorGUILayout.EndVertical();
                            }
                            break;
                        case OneFlyLib.MOVE_SPACE.Z:
                            model.moveType = (MoveType)EditorGUILayout.EnumPopup("移动类型(moveType)", model.moveType);
                            if (model.moveType != MoveType.OTHER)
                            {
                                EditorGUILayout.BeginVertical(paddingStyle);
                                EditorGUILayout.Space();
                                model.ratio = EditorGUILayout.FloatField("增量移动缩放系数(ratio)", model.ratio);
                                EditorGUILayout.Space();
                                //EditorGUILayout.LabelField("y代表z轴移动区域");
                                model.moveRange = EditorGUILayout.Vector2Field("移动区域调整(moveRange)", model.moveRange);
                                EditorGUILayout.Space();
                                model.isLimit = EditorGUILayout.BeginToggleGroup("是否限制z的值(isLimit)", model.isLimit);
                                EditorGUILayout.BeginVertical(paddingStyle);
                                EditorGUILayout.Space();
                                if (model.isLimit)
                                {
                                    model.minZ = EditorGUILayout.FloatField("限定z轴移动最小值(minZ)", model.minZ);
                                    model.maxZ = EditorGUILayout.FloatField("限定z轴移动最大值(maxZ)", model.maxZ);
                                }

                                EditorGUILayout.EndToggleGroup();
                                EditorGUILayout.EndVertical();
                                EditorGUILayout.EndVertical();
                            }
                            break;
                        default:
                            break;
                    }
                    //EditorGUILayout.Space();
                    EditorGUILayout.EndVertical();
                    #endregion
                }
                OneFlyGUITools.EndGroup();
            }
        }
        else
        {
            EditorGUILayout.BeginVertical(paddingStyle);
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("最好拖入简易地形，或用其他物体代替（如plane）");//"若不拖入且有地形，请将地形命名为GroundRange"
            EditorGUILayout.LabelField("若不拖入且有地形，请将地形命名为GroundRange");
            //model.groundObj = (GameObject)EditorGUILayout.ObjectField("物体运动的地形(groundObj)", model.groundObj, typeof(GameObject), true);
            EditorGUILayout.Space();
            EditorGUILayout.EndVertical();
        }

        //model.showEvents = OneFlyGUITools.BeginFoldOut("Events", model.showEvents);
        //if (model.showEvents)
        //{
        //    serializedObject.Update();
        //    SerializedProperty onStart = serializedObject.FindProperty("onStart");
        //    EditorGUILayout.PropertyField(onStart, true);
        //    serializedObject.ApplyModifiedProperties();

        //    serializedObject.Update();
        //    SerializedProperty onPosChange = serializedObject.FindProperty("onPosChange");
        //    EditorGUILayout.PropertyField(onPosChange, true);
        //    serializedObject.ApplyModifiedProperties();

        //    serializedObject.Update();
        //    SerializedProperty onAngleChange = serializedObject.FindProperty("onAngleChange");
        //    EditorGUILayout.PropertyField(onAngleChange, true);
        //    serializedObject.ApplyModifiedProperties();

        //    serializedObject.Update();
        //    SerializedProperty onReset = serializedObject.FindProperty("OnReset");
        //    EditorGUILayout.PropertyField(onReset, true);
        //    serializedObject.ApplyModifiedProperties();
        //}
        model.showAdvanced = OneFlyGUITools.BeginFoldOut("Advanced", model.showAdvanced);
        if (model.showAdvanced)
        {
            //EditorGUILayout.LabelField("x代表x轴移动区域");
            model.invertX = OneFlyGUITools.Toggle("X轴反向移动(invertX)", model.invertX, true);
            EditorGUILayout.Space();
            model.invertY = OneFlyGUITools.Toggle("Y轴反向移动(invertY)", model.invertY, true);
            EditorGUILayout.Space();
            model.invertZ = OneFlyGUITools.Toggle("Z轴反向移动(invertZ)", model.invertZ, true);
            EditorGUILayout.Space();
            model.updatePos = OneFlyGUITools.Toggle("模型随卡牌移动(updatePos)", model.updatePos, true);
            EditorGUILayout.Space();
            model.updateAngle = OneFlyGUITools.Toggle("模型随卡牌旋转(updateAngle)", model.updateAngle, true);
            EditorGUILayout.Space();
            model.stayIn = OneFlyGUITools.Toggle("模型不随卡牌取走而消失(stayIn)", model.stayIn, true);
        }

        //Refresh
        if (GUI.changed)
            EditorUtility.SetDirty(model);
    }
}

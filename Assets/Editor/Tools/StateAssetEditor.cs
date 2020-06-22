using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Playables;
using System.Reflection;
using System;
using System.Linq;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;

[XmlRoot("StateAsset")]

[CustomEditor(typeof(StateAsset))]
[CanEditMultipleObjects]
public class StateAssetEditor : Editor
{
    private SerializedObject obj;

    private SerializedProperty moveArea;
    private SerializedProperty optionSprite;
    private SerializedProperty sprite_Paths;
    private SerializedProperty audioHintContent;
    private SerializedProperty audioHintClip_ID_Path;
    private SerializedProperty operationHintText;
    private SerializedProperty targetswap;
    private SerializedProperty gameObjects_Active;
    private SerializedProperty gameObjects_Hide;
    private SerializedProperty activeTargets_Enter;
    private SerializedProperty disableTargets_Enter;
    private SerializedProperty activeTargets_Exit;
    private SerializedProperty disableTargets_Exit;
    private SerializedProperty enterStateTimeline;
    private SerializedProperty enterStateTimeline_Path;
    private SerializedProperty exitStateTimeline;
    private SerializedProperty exitStateTimeline_Path;
    private SerializedProperty effects;
    private SerializedProperty autoHideHint;
    private SerializedProperty progress;

    //private bool ShowInspector;
    //private bool ShowInspector2;
    //private bool ShowInspector3;
    //private bool ShowInspector4;

    StateAsset stateAsset;

    void OnEnable()
    {
        obj = new SerializedObject(target);

        moveArea = obj.FindProperty("moveArea");
        optionSprite = obj.FindProperty("optionSprite");
        sprite_Paths = obj.FindProperty("sprite_Paths");
        audioHintContent = obj.FindProperty("audioHintContent");
        audioHintClip_ID_Path = obj.FindProperty("audioHintClip_ID_Path");
        operationHintText = obj.FindProperty("operationHintText");
        targetswap = obj.FindProperty("targetswap");
        gameObjects_Active = obj.FindProperty("gameObjects_Active");
        gameObjects_Hide = obj.FindProperty("gameObjects_Hide");
        activeTargets_Enter = obj.FindProperty("activeTargets_Enter");
        disableTargets_Enter = obj.FindProperty("disableTargets_Enter");
        activeTargets_Exit = obj.FindProperty("activeTargets_Exit");
        disableTargets_Exit = obj.FindProperty("disableTargets_Exit");
        enterStateTimeline = obj.FindProperty("enterStateTimeline");
        enterStateTimeline_Path = obj.FindProperty("enterStateTimeline_Path");
        exitStateTimeline = obj.FindProperty("exitStateTimeline");
        exitStateTimeline_Path = obj.FindProperty("exitStateTimeline_Path");
        effects = obj.FindProperty("effects");
        autoHideHint = obj.FindProperty("autoHideHint");
        progress = obj.FindProperty("progress");
    }

    public override void OnInspectorGUI()
    {
        stateAsset = (StateAsset)target;
        obj.Update();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Save Data"))
        {
            Save();
        }
        if (GUILayout.Button("Get Path"))
        {
            GetItemsPath();
        }
        EditorGUILayout.EndHorizontal();

        DrawInspector();

        obj.ApplyModifiedProperties();
    }

    private void GetItemsPath()
    {
        stateAsset = (StateAsset)target;

        if (stateAsset.optionSprite.Length > 0)
        {
            stateAsset.sprite_Paths = new string[stateAsset.optionSprite.Length];
            for (int i = 0; i < stateAsset.sprite_Paths.Length; i++)
            {
                if (stateAsset.optionSprite[i].sprite != null)
                {
                    stateAsset.sprite_Paths[i] = AssetDatabase.GetAssetPath(stateAsset.optionSprite[i].sprite);
                }
            }
        }

        if (stateAsset.audioHintContent.audioHintClip)
        {
            stateAsset.audioHintClip_ID_Path = AssetDatabase.GetAssetPath(stateAsset.audioHintContent.audioHintClip);
        }
        if (stateAsset.enterStateTimeline)
        {
            stateAsset.enterStateTimeline_Path = AssetDatabase.GetAssetPath(stateAsset.enterStateTimeline);
        }
        if (stateAsset.exitStateTimeline)
        {
            stateAsset.exitStateTimeline_Path = AssetDatabase.GetAssetPath(stateAsset.exitStateTimeline);
        }

    }


    private void Save()
    {
        stateAsset = (StateAsset)target;
        XMLOp.Serialize(stateAsset, "Assets/AssetData/"+stateAsset.stateName+".xml");
        AssetDatabase.Refresh();
    }

    private void DrawInspector()
    {
        stateAsset = (StateAsset)target;

        EditorGUI.BeginChangeCheck();

        stateAsset.stateName = EditorGUILayout.TextField("状态名称", stateAsset.stateName);

        stateAsset.ShowInspector = OneFlyGUITools.BeginFoldOut("提示/动画", stateAsset.ShowInspector);

        if (stateAsset.ShowInspector)
        {
            EditorGUILayout.PropertyField(audioHintContent, true);
            EditorGUILayout.PropertyField(audioHintClip_ID_Path, true);
            EditorGUILayout.PropertyField(operationHintText, true);
            EditorGUILayout.PropertyField(enterStateTimeline, true);
            EditorGUILayout.PropertyField(enterStateTimeline_Path, true);
            EditorGUILayout.PropertyField(exitStateTimeline, true);
            EditorGUILayout.PropertyField(exitStateTimeline_Path, true);
        }

        stateAsset.ShowInspector2 = OneFlyGUITools.BeginFoldOut("替换/移动", stateAsset.ShowInspector2);

        if (stateAsset.ShowInspector2)
        {
            EditorGUILayout.PropertyField(moveArea, true);
            EditorGUILayout.PropertyField(optionSprite, true);
            EditorGUILayout.PropertyField(sprite_Paths, true);

            EditorGUILayout.PropertyField(targetswap, true);
        }

        stateAsset.ShowInspector3 = OneFlyGUITools.BeginFoldOut("激活/隐藏", stateAsset.ShowInspector3);

        if(stateAsset.ShowInspector3)
        {
            EditorGUILayout.PropertyField(gameObjects_Active, true);
            EditorGUILayout.PropertyField(gameObjects_Hide, true);
            EditorGUILayout.PropertyField(activeTargets_Enter, true);
            EditorGUILayout.PropertyField(disableTargets_Enter, true);
            EditorGUILayout.PropertyField(activeTargets_Exit, true);
            EditorGUILayout.PropertyField(disableTargets_Exit, true);
        }

        stateAsset.ShowInspector4 = OneFlyGUITools.BeginFoldOut("advance", stateAsset.ShowInspector4);

        if (stateAsset.ShowInspector4)
        {
            EditorGUILayout.PropertyField(effects, true);
            EditorGUILayout.PropertyField(autoHideHint, true);
            EditorGUILayout.PropertyField(progress, true);
        }
    }

}

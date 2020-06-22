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
using UnityEngine.Timeline;


[CustomEditor(typeof(EventAsset))]
[CanEditMultipleObjects]
public class EventAssetEditor : Editor
{
    private SerializedObject obj;

    private SerializedProperty audioClip;
    private SerializedProperty audiClip_Path;
    private SerializedProperty activeTargets;
    private SerializedProperty audioHintContent;
    private SerializedProperty timelineAsset;
    private SerializedProperty timelineAsset_Path;
    private SerializedProperty disableTargets;
    private SerializedProperty optionSprites;
    private SerializedProperty sprite_Paths;
    private SerializedProperty gameObejcts_active;
    private SerializedProperty gameObjects_hide;
    private SerializedProperty moveArea;
    private SerializedProperty targetswap;
    private SerializedProperty errorInfo;
    private SerializedProperty nextStateInfo;

    //private bool ShowIninspector;
    //private bool ShowIninspector2;
    //private bool ShowIninspector3;
    //private bool ShowIninspector4;

    EventAsset eventAsset;
    void OnEnable()
    {
        obj = new SerializedObject(target);

        audioClip = obj.FindProperty("audioClip");
        audiClip_Path = obj.FindProperty("audiClip_Path");
        activeTargets = obj.FindProperty("activeTargets");
        audioHintContent = obj.FindProperty("audioHintContent");
        timelineAsset = obj.FindProperty("timelineAsset");
        timelineAsset_Path = obj.FindProperty("timelineAsset_Path");
        disableTargets = obj.FindProperty("disableTargets");
        optionSprites = obj.FindProperty("optionSprites");
        sprite_Paths = obj.FindProperty("sprite_Paths");
        gameObejcts_active = obj.FindProperty("gameObejcts_active");
        gameObjects_hide = obj.FindProperty("gameObjects_hide");
        moveArea = obj.FindProperty("moveArea");
        targetswap = obj.FindProperty("targetswap");
        errorInfo = obj.FindProperty("errorInfo");
        nextStateInfo = obj.FindProperty("nextStateInfo");

    }
    

    public override void OnInspectorGUI()
    {
        eventAsset = (EventAsset)target;
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



    private void Save()
    {
        eventAsset = (EventAsset)target;
        XMLOp.Serialize(eventAsset, "Assets/AssetData/" + eventAsset.name + ".xml");
        AssetDatabase.Refresh();
    }

    private void GetItemsPath()
    {
        eventAsset = (EventAsset)target;

        if (eventAsset.sprite_Paths.Length > 0)
        {
            eventAsset.sprite_Paths = new string[eventAsset.sprite_Paths.Length];
            for (int i = 0; i < eventAsset.sprite_Paths.Length; i++)
            {
                if (eventAsset.optionSprites[i].sprite != null)
                {
                    eventAsset.sprite_Paths[i] = AssetDatabase.GetAssetPath(eventAsset.optionSprites[i].sprite);
                }
            }
        }

        if (eventAsset.audioHintContent.audioHintClip)
        {
            eventAsset.audioHintClip_ID_Path = AssetDatabase.GetAssetPath(eventAsset.audioHintContent.audioHintClip);
        }





        if (eventAsset.timelineAsset)
        {
            eventAsset.timelineAsset_Path = AssetDatabase.GetAssetPath(eventAsset.timelineAsset);
        }
        if (eventAsset.audioClip)
        {
            eventAsset.audiClip_Path = AssetDatabase.GetAssetPath(eventAsset.audioClip);
        }

    }

    
    private void DrawInspector()
    {
        eventAsset = (EventAsset)target;



        eventAsset.eventName = EditorGUILayout.TextField("事件名称", eventAsset.eventName);

        eventAsset.stateName = EditorGUILayout.TextField("对应状态名称", eventAsset.stateName);

        eventAsset.ShowIninspector = OneFlyGUITools.BeginFoldOut("音频/动画", eventAsset.ShowIninspector);
        if (eventAsset.ShowIninspector)
        {
            EditorGUILayout.PropertyField(audioClip, true);
            EditorGUILayout.PropertyField(audiClip_Path, true);

            EditorGUILayout.PropertyField(audioHintContent, true);
            EditorGUILayout.PropertyField(timelineAsset, true);
            EditorGUILayout.PropertyField(timelineAsset_Path, true);
        }

        eventAsset.ShowIninspector2 = OneFlyGUITools.BeginFoldOut("替换/移动", eventAsset.ShowIninspector2);
        if (eventAsset.ShowIninspector2)
        {
            EditorGUILayout.PropertyField(optionSprites, true);
            EditorGUILayout.PropertyField(sprite_Paths, true);
            EditorGUILayout.PropertyField(moveArea, true);
            EditorGUILayout.PropertyField(targetswap, true);
        }

        eventAsset.ShowIninspector3 = OneFlyGUITools.BeginFoldOut("激活/隐藏", eventAsset.ShowIninspector3);
        if (eventAsset.ShowIninspector3)
        {
            EditorGUILayout.PropertyField(activeTargets, true);
            EditorGUILayout.PropertyField(disableTargets, true);
            EditorGUILayout.PropertyField(gameObejcts_active, true);
            EditorGUILayout.PropertyField(gameObjects_hide, true);
        }
        eventAsset.ShowIninspector4 = OneFlyGUITools.BeginFoldOut("advance", eventAsset.ShowIninspector4);
        if (eventAsset.ShowIninspector4)
        {
            EditorGUILayout.Space();
            eventAsset.canLoop = OneFlyGUITools.Toggle("事件是否会更新步骤状态", eventAsset.canLoop);
            EditorGUILayout.Space();
            eventAsset.hideOperationHint = OneFlyGUITools.Toggle("触发事件时是否隐藏操作提示", eventAsset.hideOperationHint);
            EditorGUILayout.Space();
            eventAsset.hideAudioHint = OneFlyGUITools.Toggle("触发事件时是否隐藏音频提示", eventAsset.hideAudioHint);
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(errorInfo, true);
            EditorGUILayout.PropertyField(nextStateInfo, true);
        }
        
    }
}

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
using UnityEngine.Networking;
using UnityEngine.Timeline;
public class RestoreAssetData : EditorWindow
{
    #region title
    [MenuItem("Tool/Asset恢复工具")]
    static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(RestoreAssetData));
    }
    RestoreAssetData()
    {
        this.titleContent = new GUIContent("Restore Data");
    }
    #endregion

    public StateAsset stateAssetTemp;
    public EventAsset eventAssetTemp;

    public string filenameState;
    public string filenameEvent;

    public void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("State Asset Name",GUILayout.Width(120));
        filenameState = EditorGUILayout.TextField(filenameState);
        if (GUILayout.Button("Restore StateAsset"))
        {
            LoadStateAssetData(filenameState);
            SaveStateAsset(stateAssetTemp.stateName, "Assets/Resources/Restores/" + stateAssetTemp.stateName);
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Event Asset Name", GUILayout.Width(120));
        filenameEvent = EditorGUILayout.TextField(filenameEvent);
        if (GUILayout.Button("Restore EventAsset"))
        {
            LoadEventAssetData(filenameEvent);
            SaveEventAsset(eventAssetTemp.eventName, "Assets/Resources/Restores/" + eventAssetTemp.stateName);
        }
        EditorGUILayout.EndHorizontal();
    }

    public void LoadStateAssetData(string filename)
    {
        var dataDeserialized = XMLOp.Deserialize<StateAsset>("Assets/AssetData/" + filename + ".xml");
        stateAssetTemp = new StateAsset();
        stateAssetTemp.stateName = dataDeserialized.stateName;
        stateAssetTemp.moveArea = dataDeserialized.moveArea;
        stateAssetTemp.optionSprite = dataDeserialized.optionSprite;
        stateAssetTemp.sprite_Paths = dataDeserialized.sprite_Paths;
        stateAssetTemp.audioHintContent = dataDeserialized.audioHintContent;
        stateAssetTemp.audioHintClip_ID_Path = dataDeserialized.audioHintClip_ID_Path;
        stateAssetTemp.operationHintText = dataDeserialized.operationHintText;
        stateAssetTemp.targetswap = dataDeserialized.targetswap;
        stateAssetTemp.gameObjects_Active = dataDeserialized.gameObjects_Active;
        stateAssetTemp.gameObjects_Hide = dataDeserialized.gameObjects_Hide;
        stateAssetTemp.activeTargets_Enter = dataDeserialized.activeTargets_Enter;
        stateAssetTemp.disableTargets_Enter = dataDeserialized.disableTargets_Enter;
        stateAssetTemp.activeTargets_Exit = dataDeserialized.activeTargets_Exit;
        stateAssetTemp.disableTargets_Exit = dataDeserialized.disableTargets_Exit;

        stateAssetTemp.enterStateTimeline = dataDeserialized.enterStateTimeline;
        stateAssetTemp.enterStateTimeline_Path = dataDeserialized.enterStateTimeline_Path;

        stateAssetTemp.exitStateTimeline = dataDeserialized.exitStateTimeline;
        stateAssetTemp.exitStateTimeline_Path = dataDeserialized.exitStateTimeline_Path;

        stateAssetTemp.autoHideHint = dataDeserialized.autoHideHint;
        stateAssetTemp.effects = dataDeserialized.effects;
        stateAssetTemp.progress = dataDeserialized.progress;
        Debug.Log(dataDeserialized.stateName);



        //return stateAsset;
    }

    public void SaveStateAsset(string saveName, string path)
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
        path = path + "/" + saveName + ".asset";
        saveName = saveName.Replace(".asset", "");
        UnityEditor.AssetDatabase.CreateAsset(stateAsset, path);
        UnityEditor.AssetDatabase.SaveAssets();
        UnityEditor.AssetDatabase.Refresh();


        StateAsset asset = (StateAsset)Resources.Load("Restores/" + stateAssetTemp.stateName + "/" + saveName);
        asset.stateName = stateAssetTemp.stateName;
        asset.moveArea = stateAssetTemp.moveArea;
        asset.optionSprite = stateAssetTemp.optionSprite;
        asset.audioHintContent = stateAssetTemp.audioHintContent;
        asset.operationHintText = stateAssetTemp.operationHintText;
        asset.targetswap = stateAssetTemp.targetswap;
        asset.gameObjects_Hide = stateAssetTemp.gameObjects_Hide;
        asset.gameObjects_Active = stateAssetTemp.gameObjects_Active;
        asset.activeTargets_Enter = stateAssetTemp.activeTargets_Enter;
        asset.disableTargets_Enter = stateAssetTemp.disableTargets_Enter;
        asset.activeTargets_Exit = stateAssetTemp.activeTargets_Exit;
        asset.disableTargets_Exit = stateAssetTemp.disableTargets_Exit;
        asset.enterStateTimeline = stateAssetTemp.enterStateTimeline;
        asset.exitStateTimeline = stateAssetTemp.exitStateTimeline;
        asset.progress = stateAssetTemp.progress;
        asset.effects = stateAssetTemp.effects;

        asset.sprite_Paths = stateAssetTemp.sprite_Paths;
        asset.audioHintClip_ID_Path = stateAssetTemp.audioHintClip_ID_Path;
        asset.enterStateTimeline_Path = stateAssetTemp.enterStateTimeline_Path;
        asset.exitStateTimeline_Path = stateAssetTemp.exitStateTimeline_Path;


        if (asset.sprite_Paths.Length > 0)
        {
            for (int i = 0; i < asset.sprite_Paths.Length; i++)
            {
                if (!String.IsNullOrEmpty(asset.sprite_Paths[i]))
                {
                    asset.optionSprite[i].sprite = (Sprite)AssetDatabase.LoadAssetAtPath<Sprite>(asset.sprite_Paths[i]);
                }
            }
        }


        if (!String.IsNullOrEmpty(asset.audioHintClip_ID_Path))
        {
            asset.audioHintContent.audioHintClip = (AudioClip)AssetDatabase.LoadAssetAtPath<AudioClip>(asset.audioHintClip_ID_Path);
        }
        if (!String.IsNullOrEmpty(asset.enterStateTimeline_Path))
        {
            asset.enterStateTimeline = (TimelineAsset)AssetDatabase.LoadAssetAtPath(asset.enterStateTimeline_Path, typeof(TimelineAsset));
        }
        if (!String.IsNullOrEmpty(asset.exitStateTimeline_Path))
        {
            asset.exitStateTimeline = (TimelineAsset)AssetDatabase.LoadAssetAtPath<TimelineAsset>(asset.exitStateTimeline_Path);
        }

    }


    public void LoadEventAssetData(string filename)
    {
        var dataDeserialized = XMLOp.Deserialize<EventAsset>("Assets/AssetData/" + filename + ".xml");
        eventAssetTemp = new EventAsset();
        eventAssetTemp.eventName = dataDeserialized.eventName;
        eventAssetTemp.stateName = dataDeserialized.stateName;
        eventAssetTemp.audioClip = dataDeserialized.audioClip;
        eventAssetTemp.audiClip_Path = dataDeserialized.audiClip_Path;
        eventAssetTemp.audioHintContent = dataDeserialized.audioHintContent;
        eventAssetTemp.audioHintClip_ID_Path = dataDeserialized.audioHintClip_ID_Path;
        eventAssetTemp.timelineAsset = dataDeserialized.timelineAsset;
        eventAssetTemp.timelineAsset_Path = dataDeserialized.timelineAsset_Path;
        eventAssetTemp.canLoop = dataDeserialized.canLoop;
        eventAssetTemp.activeTargets = dataDeserialized.activeTargets;
        eventAssetTemp.disableTargets = dataDeserialized.disableTargets;
        eventAssetTemp.optionSprites = dataDeserialized.optionSprites;
        eventAssetTemp.sprite_Paths = dataDeserialized.sprite_Paths;
        eventAssetTemp.gameObejcts_active = dataDeserialized.gameObejcts_active;
        eventAssetTemp.gameObjects_hide = dataDeserialized.gameObjects_hide;
        eventAssetTemp.errorInfo = dataDeserialized.errorInfo;
        eventAssetTemp.moveArea = dataDeserialized.moveArea;
        eventAssetTemp.hideOperationHint = dataDeserialized.hideOperationHint;
        eventAssetTemp.hideAudioHint = dataDeserialized.hideAudioHint;
        //eventAssetTemp.triggerWhilePlaying = dataDeserialized.triggerWhilePlaying;
        eventAssetTemp.nextStateInfo = dataDeserialized.nextStateInfo;

    }

    public void SaveEventAsset(string saveName, string path)
    {
        EventAsset eventAsset = ScriptableObject.CreateInstance<EventAsset>();
        eventAsset.name = saveName;

        if (saveName == "" || path == "")
        {
            Debug.Log(saveName);
            Debug.LogError(string.Format("[UConfig]: 创建失败 , 信息不完整!"));
            return;
        }
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        path = path + "/" + saveName + ".asset";
        saveName = saveName.Replace(".asset", "");
        UnityEditor.AssetDatabase.CreateAsset(eventAsset, path);
        UnityEditor.AssetDatabase.SaveAssets();
        UnityEditor.AssetDatabase.Refresh();


        EventAsset asset = (EventAsset)Resources.Load("Restores/" + eventAssetTemp.stateName + "/" + saveName);
        asset.eventName = eventAssetTemp.eventName;
        asset.stateName = eventAssetTemp.stateName;
        asset.audioClip = eventAssetTemp.audioClip;
        asset.audiClip_Path = eventAssetTemp.audiClip_Path;
        asset.audioHintContent = eventAssetTemp.audioHintContent;
        asset.audioHintClip_ID_Path = eventAssetTemp.audioHintClip_ID_Path;
        asset.timelineAsset = eventAssetTemp.timelineAsset;
        asset.timelineAsset_Path = eventAssetTemp.timelineAsset_Path;
        asset.canLoop = eventAssetTemp.canLoop;
        asset.activeTargets = eventAssetTemp.activeTargets;
        asset.disableTargets = eventAssetTemp.disableTargets;
        asset.optionSprites = eventAssetTemp.optionSprites;
        asset.sprite_Paths = eventAssetTemp.sprite_Paths;
        asset.gameObejcts_active = eventAssetTemp.gameObejcts_active;
        asset.gameObjects_hide = eventAssetTemp.gameObjects_hide;
        asset.errorInfo = eventAssetTemp.errorInfo;
        asset.moveArea = eventAssetTemp.moveArea;
        asset.hideOperationHint = eventAssetTemp.hideOperationHint;
        asset.hideAudioHint = eventAssetTemp.hideAudioHint;
        //asset.triggerWhilePlaying = eventAssetTemp.triggerWhilePlaying;
        asset.nextStateInfo = eventAssetTemp.nextStateInfo;




        if (asset.sprite_Paths.Length > 0)
        {
            for (int i = 0; i < asset.sprite_Paths.Length; i++)
            {
                if (!String.IsNullOrEmpty(asset.sprite_Paths[i]))
                {
                    asset.optionSprites[i].sprite = (Sprite)AssetDatabase.LoadAssetAtPath<Sprite>(asset.sprite_Paths[i]);
                }
            }
        }


        if (!String.IsNullOrEmpty(asset.audioHintClip_ID_Path))
        {
            asset.audioHintContent.audioHintClip = (AudioClip)AssetDatabase.LoadAssetAtPath<AudioClip>(asset.audioHintClip_ID_Path);
        }
        if (!String.IsNullOrEmpty(asset.timelineAsset_Path))
        {
            asset.timelineAsset = (TimelineAsset)AssetDatabase.LoadAssetAtPath(asset.timelineAsset_Path, typeof(TimelineAsset));
        }
        if (!String.IsNullOrEmpty(asset.audiClip_Path))
        {
            asset.audioClip = (AudioClip)AssetDatabase.LoadAssetAtPath<AudioClip>(asset.audiClip_Path);
        }

    }

}

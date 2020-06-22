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


[CustomEditor(typeof(EventAsset))]
[CanEditMultipleObjects]
public class EventAssetEditor : Editor
{
    private SerializedObject obj;
    EventAsset eventAsset;
    void OnEnable()
    {
        obj = new SerializedObject(target);
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


        DrawDefaultInspector();
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

}

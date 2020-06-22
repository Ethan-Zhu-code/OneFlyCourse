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
    StateAsset stateAsset;

    void OnEnable()
    {
        obj = new SerializedObject(target);
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
        DrawDefaultInspector();

      

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

}

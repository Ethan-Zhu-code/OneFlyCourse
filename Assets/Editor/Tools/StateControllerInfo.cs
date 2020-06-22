using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(StateController))]
[CanEditMultipleObjects]
public class StateControllerInfo : Editor
{

    StateController stateController;
    private SerializedObject obj;
    int stateIndex = 0;
    void OnEnable()
    {
        obj = new SerializedObject(target);
    }
    public override void OnInspectorGUI()
    {
        stateController = (StateController)target;
        obj.Update();
        DrawDefaultInspector();
        if (GUILayout.Button("Load Asset"))
        {
            if (stateController)
            {
                //stateController.TestAssetLoad();
                Debug.Log("Testloaded");
            }
            else
            {
                Debug.LogError("None Asset");
            }

        }
        if (GUILayout.Button("Clear Asset"))
        {
            if (stateController.currentStateAsset)
            {
                //stateController.ClearAsset();
                Debug.Log("Clear");
            }
            else
            {
                Debug.LogError("None Asset");
            }

        }
        if (GUILayout.Button("Test CheckBoolGroup "))
        {
            if (stateController.currentStateAsset)
            {

                Debug.Log(stateController.CheckEffectsFinish());
            }
            else
            {
                Debug.LogError("None Asset");
            }

        }
        if (GUILayout.Button("Restore Effects "))
        {
            if (stateController.effects.Length != 0)
            {

                stateController.RestoreEffect();
            }

        }
        if (GUILayout.Button("Test LoadallEventWithAsset"))
        {
            //   stateController.LoadallEventControllersWithAsset();

        }

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("State", GUILayout.Width(40));

        stateIndex = EditorGUILayout.IntField(stateIndex, GUILayout.Width(40));
        if (GUILayout.Button("LoadState"))
        {
            stateController.LoadState(stateIndex);

        }

        EditorGUILayout.EndHorizontal();
        obj.ApplyModifiedProperties();
    }
}

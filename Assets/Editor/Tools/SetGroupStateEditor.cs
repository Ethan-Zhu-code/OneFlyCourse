using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(SetGroupState))]
[CanEditMultipleObjects]
public class SetGroupStateEditor : Editor {
    private SerializedObject obj;
    SetGroupState groupState=new SetGroupState();

    bool groupStatefold=true;
    bool eventfold=true;
    void OnEnable()
    {
        obj = new SerializedObject(target);
    }


    public override void OnInspectorGUI()
    {
        groupState = (SetGroupState)target;
        obj.Update();

        DrawDefaultInspector();
        eventfold = EditorGUILayout.Foldout(eventfold, "显示/隐藏的Gameobjects组");
        if (eventfold)
        {
            if (groupState.testFunctionlines.Length > 0)
            {
                for (int i = 0; i < groupState.testFunctionlines.Length; i++)
                {
                    if (GUILayout.Button("测试函数事件 "+i+"号",GUILayout.Width(150)))
                    {
                        groupState.InvokeFunction(i);
                    }
                
                }
            }
        }


        groupStatefold = EditorGUILayout.Foldout(groupStatefold, "显示/隐藏的Gameobjects组");
        if (groupStatefold)
        {
            if (groupState.objectGroups.Length > 0)
            {
                for (int i = 0; i < groupState.objectGroups.Length; i++)
                {
                    if (groupState.objectGroups[i].gameObjects.Length > 0)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField(groupState.objectGroups[i].name, GUILayout.Width(80));
                        if (GUILayout.Button("Enable", GUILayout.Width(60)))
                        {
                            groupState.SetActive(i, true);
                        }
                        if (GUILayout.Button("Disable", GUILayout.Width(60)))
                        {
                            groupState.SetActive(i, false);
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                    else
                    {
                        EditorGUILayout.HelpBox("None Object", MessageType.Error);

                    }
                }
            }
        }
     
      



        obj.ApplyModifiedProperties();
    }

}

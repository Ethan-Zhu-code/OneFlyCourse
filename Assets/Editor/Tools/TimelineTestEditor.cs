using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TimelineTest))]
[CanEditMultipleObjects]

public class TimelineTestEditor : Editor
{
    TimelineTest timelineTest;
    private SerializedObject obj;

    void OnEnable()
    {
        obj = new SerializedObject(target);
    }


    public override void OnInspectorGUI()
    {
        timelineTest = (TimelineTest)target;
        obj.Update();


        DrawDefaultInspector();


        if (GUILayout.Button("播放下一动画\n（默认空格键启动）", GUILayout.Width(120), GUILayout.Height(40)))
        {
            timelineTest.PlayNextTimelineAsset();
        }


        EditorGUILayout.BeginHorizontal();



        if (GUILayout.Button("播放所有动画\n（默认A键启动）", GUILayout.Width(180), GUILayout.Height(50)))
        {
            timelineTest.standBy();
            timelineTest.StartTest();
        }


        EditorGUILayout.EndHorizontal();



        obj.ApplyModifiedProperties();



    }










    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

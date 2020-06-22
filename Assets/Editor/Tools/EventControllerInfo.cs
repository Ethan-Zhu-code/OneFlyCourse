using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EventController))]
[CanEditMultipleObjects]
public class EventControllerInfo : Editor
{
    EventController eventController;
   // SerializedProperty idStastic;
    bool needAsset;
    private SerializedObject obj;
    void OnEnable()
    {
        obj = new SerializedObject(target);
       // needAsset = obj.FindProperty("needAsset");

       // idStastic = serializedObject.FindProperty("objectID");
        

    }
    public override void OnInspectorGUI()
    {
        eventController = (EventController)target;
        obj.Update();
        //EditorGUILayout.TextField("ObjectID", eventController.objectID);
       // EditorGUILayout.PropertyField(needAsset);
        //  EditorGUILayout.Toggle("是否需要Asset", needAsset);

        if (true)
        {
            
            DrawDefaultInspector();
            if (GUILayout.Button("Load Asset"))
            {
                //if (eventController.eventAsset)
                //{
                //    //eventController.LoadAsset();
                //    Debug.Log("loaded");
                //}
                //else
                //{
                //    Debug.LogError("None Asset");
                //}

            }
            if (GUILayout.Button("ClearAsset"))
            {
                
                    //eventController.ClearAsset();
                    //Debug.Log("Clear");
               

            }
            if (GUILayout.Button("Test UpdateState"))
            {
                eventController.UpdateStateControllerEffect(true);

            }
            if (GUILayout.Button("Test PlayTimeline"))
            {
                    //eventController.EventTrigger();

            }

            GUILayout.Space(30);
            GUILayout.Label("测试用，建议不用这个");
            GUILayout.Label("用 生成器 里的那个");
            if (GUILayout.Button("Load StateController"))
            {
                //eventController.LoadStateController();
            }
            if(GUILayout.Button("Clear StateController"))
            {
               // eventController.ClearStateController();
            }
        }
        else
        {
            //eventController.ClearAsset();
        }
        obj.ApplyModifiedProperties();
        // serializedObject.ApplyModifiedProperties();
    }
}

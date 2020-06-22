using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System;

public class AutoSave : EditorWindow
{
    private float deltaTime = 25;
    private float currentTime = 0;
    private float signedTime =0;
    private DateTime currentDateTime;


    [MenuItem("Tool/自动保存")]
    static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(AutoSave));
    }

    void OnEnable()
    {
        currentDateTime = DateTime.Now;
        currentTime = Convert.ToSingle(currentDateTime.Second);
        signedTime = currentTime-2;
    }

    AutoSave()
    {
        this.titleContent = new GUIContent("AutoSave");
    }
    void OnGUI()
    {
        //EditorSceneManager.SaveOpenScenes();
        EditorGUILayout.LabelField("保存间隔");
        deltaTime = EditorGUILayout.FloatField(deltaTime);
        saveScene();
       EditorGUILayout.LabelField("当前时间",currentTime.ToString("0"));
        EditorGUILayout.LabelField("记录时间", signedTime.ToString("0"));
        Repaint();
      
    }

    void saveScene()
    {
        currentDateTime = DateTime.Now;
        currentTime= Convert.ToSingle(currentDateTime.Second);


        if (currentTime - signedTime >= deltaTime)
        {
            EditorSceneManager.SaveOpenScenes();
            signedTime = currentTime;
        }

        if (currentTime - signedTime ==0)
        {
            EditorGUILayout.HelpBox("已保存", MessageType.Warning, true);
        }
    }



}

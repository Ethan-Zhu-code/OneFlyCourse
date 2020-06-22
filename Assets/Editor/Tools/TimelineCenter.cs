using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using System.Reflection;
using System;
using System.IO;
using System.Text;

public class TimelineCenter : EditorWindow
{
    [MenuItem("Tool/Timeline Center")]
    static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(TimelineCenter));
    }
    TimelineCenter()
    {
        this.titleContent = new GUIContent("Timeline Center");
    }



    public TimelineAsset[] timelineAssets;
    private PlayableDirector playableDirector;
    private int lineCount=0;
    bool change = false;

    private void OnEnable()
    {
        playableDirector = FindObjectOfType<PlayableDirector>();
        timelineAssets = new TimelineAsset[0];
        lineCount = 0;
        timelineAssets = GetAllPlayableAsset();
        change = true;
    }

    private void OnProjectChange()  
    {
        timelineAssets = new TimelineAsset[0];
        lineCount = 0;
        timelineAssets = GetAllPlayableAsset();
        change = true;
    }

    private void OnHierarchyChange()
    {
        timelineAssets = new TimelineAsset[0];
        lineCount = 0;
        timelineAssets = GetAllPlayableAsset();
        change = true;
    }


    private void OnGUI()
    {

    

        if (change)
        {
            if (timelineAssets.Length > 0)
            {
                lineCount = Mathf.FloorToInt(timelineAssets.Length / 5);
                if (timelineAssets.Length % 5 != 0)
                {
                    lineCount++;
                }

            }
            
            for (int line = 1; line <= lineCount; line++)
            {
                int k = (line) * 5 - 1;
                if (k > timelineAssets.Length - 1)
                {
                    k = timelineAssets.Length - 1;
                }
                EditorGUILayout.BeginHorizontal();
                for (int i = line*5-5; i <= k; i++)
                {

                    string name = timelineAssets[i].name;
                    if (GUILayout.Button(name, GUILayout.Height(20), GUILayout.Width(90)))
                    {
                        SwitchTimelineAsset(i);
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            //change = false;
        }
    




        this.Repaint();

    }




    public void SwitchTimelineAsset(int index)
    {
        if (index >= 0 && index < timelineAssets.Length)
        {
            playableDirector.playableAsset = timelineAssets[index];
        }

    }
    public TimelineAsset[] GetAllPlayableAsset()
    {
        List<TimelineAsset> objectsInScene = new List<TimelineAsset>();
        TimelineAsset[] objects;
        foreach (TimelineAsset go in Resources.LoadAll("Timeline", typeof(TimelineAsset)))
        {
            objectsInScene.Add(go);
        }

  
        objects = new TimelineAsset[objectsInScene.Count];
        objectsInScene.CopyTo(objects);

        return objects;
    }




}

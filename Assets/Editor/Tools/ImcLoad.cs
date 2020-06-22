using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Playables;
using System.Reflection;
using System;
using System.IO;


public class ImcLoad : EditorWindow {
    [MenuItem("泛虚拟现实实验室(别打开)/LoadIMC")]
    static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(ImcLoad));
    }
    ImcLoad()
    {
        this.titleContent = new GUIContent("加载URL模型文件");
    }

    private string path;


    void OnGUI()
    {
        GUILayout.Label("URL");
        path= GUILayout.TextArea(path);


        GUILayout.Label("读取URL");
        if (GUILayout.Button("加载模型", GUILayout.Width(200)))
        {
            ImcLoadFile();
            //if (path != "")
            //{
            //    // ImcLoadFile(path);
            //    ImcLoadFile();
            //}

        }
    }

    private void ImcLoadFile()
    {

        var go = ObjFormatAnalyzerFactory.AnalyzeToGameObject(@"F:\Test\1.imc");
       // var go = ObjFormatAnalyzerFactory.AnalyzeToGameObject(@path);
        //WWW www = new WWW("file:/F:/Test/4.png");

        //while (!www.isDone) { }

        //go.GetComponent<MeshRenderer>().material.mainTexture = www.texture;

    }

    private void ImcLoadFile(string path)
    {
     
           // var go = ObjFormatAnalyzerFactory.AnalyzeToGameObject(@"F:\Test\1.imc");
        var go = ObjFormatAnalyzerFactory.AnalyzeToGameObject(@path);
        //WWW www = new WWW("file:/F:/Test/4.png");

        //while (!www.isDone) { }

        //go.GetComponent<MeshRenderer>().material.mainTexture = www.texture;

    }
   
}

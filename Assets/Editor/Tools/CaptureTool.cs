using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Playables;
using System.Reflection;
using System;
using System.IO;
using UnityEngine.Rendering.PostProcessing;

public class CaptureTool : EditorWindow
{
    [MenuItem("Tool/截图工具")]
    static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(CaptureTool));
    }
    CaptureTool()
    {
        this.titleContent = new GUIContent("截图工具");
    }

    public GameObject cameraDefault;

    public GameObject uiDefault;


    public bool InCaptureMode = false;

    public GameObject captureCamera;

    public GameObject[] renderCameras = new GameObject[4];

    public RenderTexture[] renderTextures = new RenderTexture[4];

    public GameObject Guideline;
    public GameObject tempGuideline;


    public int selectBar = 0;
    public string[] cameraBar = new string[5] { "前", "后", "左", "右", "四个" };

    GameObject cameraGizmos;
    CameraGizmos gizmos;



    private void Update()
    {
        if (cameraDefault == null)
        {
            cameraDefault = GameObject.Find("Camera");
        }
        if (uiDefault == null)
        {
            uiDefault = GameObject.Find("[BasicFramework]");
        }
        if (Guideline == null)
        {
            Guideline=(GameObject)Resources.Load("ToolsResources/Guideline") as GameObject;
        }
        if (cameraGizmos == null)
        {
            cameraGizmos = GameObject.Find("ToolsEditorOnly");
          

        }
    }

    private void OnEnable()
    {
        InCaptureMode = true;
    }

    private void OnDestroy()
    {
        DestoryCamera();
        SetDefaultState(true);
    }

    public void OnGUI()
    {
       // EditorGUILayout.BeginHorizontal();
       // EditorGUILayout.LabelField("进入截屏模式");
       //// InCaptureMode = EditorGUILayout.Toggle(InCaptureMode);
       // EditorGUILayout.EndHorizontal();
        

        if (InCaptureMode)
        {
            selectBar = GUILayout.Toolbar(selectBar, cameraBar, GUILayout.Height(25));
            SetDefaultState(false);
            CreatCamera();
            CreatRenderCamera(0,new Vector3(0,0,0),new Vector3(0,0,0));
            CreatRenderCamera(1,new Vector3(0,0,16),new Vector3(0,180,0));
            CreatRenderCamera(2, new Vector3(2.5f, -0.4f, 3f), new Vector3(0, 325, 0));
            CreatRenderCamera(3, new Vector3(-6.5f, -0.4f, 4f), new Vector3(0, 35, 0));
            if (selectBar == 0)
            {
                Vector2 textureSize = new Vector2(position.width - 10, (position.width - 10) * 9 / 16);
                GUILayout.Box(renderTextures[0], GUILayout.Width(textureSize.x), GUILayout.Height(textureSize.y));
                EditorGUILayout.BeginHorizontal();
                captureCamera.transform.position = new Vector3(0, 0, 0);
                captureCamera.transform.eulerAngles = new Vector3(0, 0, 0);
                if (GUILayout.Button("保存该视角图片", GUILayout.Width(150)))
                {
                    SaveTexture(0);
                }
                if (GUILayout.Button("保存全部图片", GUILayout.Width(150)))
                {
                    SaveAllTexture();
                }
                EditorGUILayout.EndHorizontal();
              
            }
            if (selectBar == 1)
            {
                captureCamera.transform.position = new Vector3(0, 0, 16);
                captureCamera.transform.eulerAngles = new Vector3(0, 180, 0);
                Vector2 textureSize = new Vector2(position.width - 10, (position.width - 10) * 9 / 16);
                GUILayout.Box(renderTextures[1], GUILayout.Width(textureSize.x), GUILayout.Height(textureSize.y));
                EditorGUILayout.BeginHorizontal();
                
                if (GUILayout.Button("保存该视角图片", GUILayout.Width(150)))
                {
                    SaveTexture(1);

                }
                if (GUILayout.Button("保存全部图片", GUILayout.Width(150)))
                {
                    SaveAllTexture();
                }
                EditorGUILayout.EndHorizontal();
               
            }
            if (selectBar == 2)
            {
                captureCamera.transform.position = new Vector3(2.5f, -0.4f, 3f);
                captureCamera.transform.eulerAngles = new Vector3(0, 325, 0);
                Vector2 textureSize = new Vector2(position.width - 10, (position.width - 10) * 9 / 16);
                GUILayout.Box(renderTextures[2], GUILayout.Width(textureSize.x), GUILayout.Height(textureSize.y));
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("保存该视角图片",GUILayout.Width(150)))
                {
                    SaveTexture(2);
                }
                if (GUILayout.Button("保存全部图片", GUILayout.Width(150)))
                {
                    SaveAllTexture();
                }
                EditorGUILayout.EndHorizontal();
               
            }
            if (selectBar == 3)
            {
                captureCamera.transform.position = new Vector3(-6.5f, -0.4f, 4f);
                captureCamera.transform.eulerAngles = new Vector3(0, 35, 0);
                Vector2 textureSize = new Vector2(position.width - 10, (position.width - 10) * 9 / 16);
                GUILayout.Box(renderTextures[3], GUILayout.Width(textureSize.x), GUILayout.Height(textureSize.y));
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("保存该视角图片", GUILayout.Width(150)))
                {

                    SaveTexture(3);

                }
                if (GUILayout.Button("保存全部图片", GUILayout.Width(150)))
                {
                    SaveAllTexture();
                }
                EditorGUILayout.EndHorizontal();
               
            }
            if (selectBar == 4)
            {
                captureCamera.transform.position = new Vector3(0, 0, 0);
                captureCamera.transform.eulerAngles = new Vector3(0, 0, 0);
                Vector2 textureSize = new Vector2((position.width - 10)/2, (position.width - 10) * 9 / 32);
                EditorGUILayout.BeginHorizontal();
                GUILayout.Box(renderTextures[0], GUILayout.Width(textureSize.x), GUILayout.Height(textureSize.y));
                GUILayout.Box(renderTextures[1], GUILayout.Width(textureSize.x), GUILayout.Height(textureSize.y));
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                GUILayout.Box(renderTextures[2], GUILayout.Width(textureSize.x), GUILayout.Height(textureSize.y));
                GUILayout.Box(renderTextures[3], GUILayout.Width(textureSize.x), GUILayout.Height(textureSize.y));
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();

                if (GUILayout.Button("保存全部图片", GUILayout.Width(150)))
                {
                    SaveAllTexture();
                }
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.HelpBox("关闭前请关闭截屏模式", MessageType.Warning);
        }
        if (!InCaptureMode)
        {
            DestoryCamera();
            SetDefaultState(true);
        }


        Repaint();
    }


   




    void SetDefaultState(bool value)
    {
        if (cameraDefault!=null && uiDefault != null
            && cameraDefault.activeInHierarchy == value && uiDefault.activeInHierarchy == value)
        {
            return;
        }
        if (cameraDefault != null)
            { cameraDefault.SetActive(value); }
        if (uiDefault != null)
        { uiDefault.SetActive(value); }
    }

    private void DestoryCamera()
    {
        if (captureCamera != null)
        {
            DestroyImmediate(captureCamera);
            captureCamera = null;
        }


        for (int i = 0; i < renderCameras.Length; i++)
        {
            if (renderCameras[i] != null)
            {
                DestroyImmediate(renderCameras[i]);
                renderCameras[i] = null;
                renderTextures[i] = null;
            }


        }

        if (tempGuideline != null)
        {
            DestroyImmediate(tempGuideline);
            tempGuideline = null ;
        }
        if (cameraGizmos.GetComponent<CameraGizmos>())
        {
            DestroyImmediate(cameraGizmos.GetComponent<CameraGizmos>());
            gizmos =null;
        }

      
      
    }
    private void CreatCamera()
    {
        if (captureCamera != null) { return; }
        captureCamera = new GameObject("CaptureCamera");
        captureCamera.AddComponent<Camera>();
        captureCamera.AddComponent<UnityEngine.Rendering.PostProcessing.PostProcessLayer>();
        captureCamera.GetComponent<UnityEngine.Rendering.PostProcessing.PostProcessLayer>().volumeTrigger= captureCamera.transform;

        LayerMask mask = 1 << 9;
        PostProcessLayer.Antialiasing antialiasing = PostProcessLayer.Antialiasing.SubpixelMorphologicalAntialiasing;
        captureCamera.GetComponent<UnityEngine.Rendering.PostProcessing.PostProcessLayer>().volumeLayer= mask;
        captureCamera.GetComponent<UnityEngine.Rendering.PostProcessing.PostProcessLayer>().antialiasingMode = antialiasing;
        if (tempGuideline == null)
        {
            tempGuideline = (GameObject)Instantiate(Guideline);
        }
        cameraGizmos.AddComponent<CameraGizmos>();
        gizmos = cameraGizmos.GetComponent<CameraGizmos>();
        gizmos.renderCameras = renderCameras;
    }

    private void CreatRenderCamera(int index, Vector3 position, Vector3 angle)
    {
        if (renderCameras[index] != null) { return; }
        renderCameras[index] = new GameObject("RenderCamera"+index);
        renderCameras[index].transform.position = position;
        renderCameras[index].transform.eulerAngles = angle;
        renderCameras[index].AddComponent<Camera>();
        renderCameras[index].AddComponent<UnityEngine.Rendering.PostProcessing.PostProcessLayer>();
        renderCameras[index].GetComponent<UnityEngine.Rendering.PostProcessing.PostProcessLayer>().volumeTrigger = renderCameras[index].transform;
        LayerMask mask = 1 << 9;
        PostProcessLayer.Antialiasing antialiasing = PostProcessLayer.Antialiasing.SubpixelMorphologicalAntialiasing;
        renderCameras[index].GetComponent<UnityEngine.Rendering.PostProcessing.PostProcessLayer>().volumeLayer = mask;
        renderCameras[index].GetComponent<UnityEngine.Rendering.PostProcessing.PostProcessLayer>().antialiasingMode = antialiasing;


        Camera cameraSetting= renderCameras[index].GetComponent<Camera>();
        renderTextures[index]=new RenderTexture(1920, 1080, 0);
        cameraSetting.targetTexture = renderTextures[index];
        cameraSetting.Render();


    }


    void SaveTexture(int index)
    {
        Texture2D screenShot = new Texture2D((int)1920, (int)1080, TextureFormat.RGB24, false,true);

        var previous = RenderTexture.active;
        RenderTexture.active = renderTextures[index];

        screenShot.ReadPixels(new Rect(0, 0, renderTextures[index].width, renderTextures[index].height), 0, 0);

        RenderTexture.active = previous;

        screenShot.Apply();
        byte[] bytes = screenShot.EncodeToPNG();
        var path = EditorUtility.SaveFilePanel("保存图片","", "截图"+index+".png", "png");

        if (path.Length != 0)
        {
            if (bytes != null)
            {
                File.WriteAllBytes(path ,bytes);
            }
        }
    }
    void SaveAllTexture()
    {
        var path = EditorUtility.SaveFolderPanel("请选择本地文件夹保存", "", "");
        Texture2D[] screenShot=new Texture2D[4] ;
        for (int i = 0; i < renderTextures.Length; i++)
        {
             screenShot[i] = new Texture2D((int)1920, (int)1080, TextureFormat.RGB24, false, true);
            var previous = RenderTexture.active;
            RenderTexture.active = renderTextures[i];

            screenShot[i].ReadPixels(new Rect(0, 0, renderTextures[i].width, renderTextures[i].height), 0, 0);

            RenderTexture.active = previous;

            screenShot[i].Apply();
            byte[] bytes = screenShot[i].EncodeToPNG();
            if (path.Length != 0)
            {
                if (bytes != null)
                {
                    File.WriteAllBytes(path + "/" + "截图组" + i + ".png", bytes);
                }
            }
        }
      

        
      

        
    }
}

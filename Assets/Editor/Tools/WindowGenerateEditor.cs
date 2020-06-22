using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Playables;
using System.Reflection;
using System;

public class WindowGenerateEditor : EditorWindow {

    [MenuItem("Tool/生成器")]
    static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(WindowGenerateEditor));
    }
    WindowGenerateEditor()
    {
        this.titleContent = new GUIContent("生成器");
    }

    /// <summary>
    /// 
    /// </summary>

    private bool autoGenerateID=true;
    private GUISkin skinBackground;
    private GUISkin zhuangge;
    private GUISkin cola; 
    Vector3 scrollPos = Vector2.zero;

    private bool generateFoldout;
    private bool checkerFoldout;

    private void OnEnable()
    {

        skinBackground = (GUISkin)Resources.Load("BackRed");
        zhuangge = (GUISkin)Resources.Load("Zhuangge");
        cola = (GUISkin)Resources.Load("Cola");
    }






    void OnGUI()
    {
        scrollPos = GUI.BeginScrollView(new Rect(0, 0, position.width, position.height),
            scrollPos, new Rect(0, 0, 0, 1500));


        GUI.skin = skinBackground;
        GUI.skin.box.fontSize = 20;
     //   GUI.skin.box.normal.textColor = Color.blue;
        //GUI.skin.box.normal.background=(Texture2D)green
        //GUI.skin.box.normal.background;
        GUILayout.Box("NEU_LAB",  GUILayout.Width(300),GUILayout.Height(40));
        GUI.skin = zhuangge;
        GUILayout.Box(" ", GUILayout.Width(300), GUILayout.Height(60));
        GUI.skin = skinBackground;
        GUI.skin.label.fontSize = 20;
        GUI.skin.label.fontStyle = FontStyle.Bold;
        GUI.skin.label.normal.textColor =Color.yellow;
        GUILayout.Label("created by 装哥");
        GUILayout.Space(20);
        GUI.skin.label.fontSize = 15;
        GUI.skin.label.fontStyle = FontStyle.Bold;
        GUI.skin.label.normal.textColor = Color.red;
        GUILayout.Label("ID生成器");
        GUI.skin.label.fontSize = 12;
        generateFoldout = EditorGUILayout.Foldout(generateFoldout, "生成器");
        autoGenerateID = EditorGUILayout.Toggle("是否自动设置ID", autoGenerateID);
        if (generateFoldout)
        {
            GUILayout.Label("生成ID");
            if (GUILayout.Button("Register Objects ID", GUILayout.Width(200)))
            {
                RegisterObjectsID();
                autoGenerateID = true;
                Debug.Log("Register");
            }
            GUILayout.Label("修正ID");
            if (GUILayout.Button("Fix Objects ID", GUILayout.Width(200)))
            {
                FixObjectsID();
                Debug.Log("Fixed");
            }
            GUILayout.Label("清除ID");
            if (GUILayout.Button("Clear Objects ID", GUILayout.Width(200)))
            {
                ClearObjectsID();
                autoGenerateID = false;
                Debug.Log("Clear");
            }

        }



        GUILayout.Space(30);
        GUI.skin.label.fontSize = 15;
        GUI.skin.label.fontStyle = FontStyle.Bold;
        GUI.skin.label.normal.textColor = Color.red;
        GUILayout.Label("检查管理器");       
        GUI.skin.label.fontSize = 12;
        checkerFoldout = EditorGUILayout.Foldout(checkerFoldout, "检查器");
        if (checkerFoldout)
        {
            GUILayout.Label("检查StateController");
            if (GUILayout.Button("Check StateController", GUILayout.Width(200)))
            {
                CheckStateControllerInScene();
                Debug.Log("Check_StateController");
            }

            GUILayout.Label("注册StateController");
            if (GUILayout.Button("Register StateController", GUILayout.Width(200)))
            {
                CheckStateControllerInScene();
                RegisterStateController(FindStateOnlyControllerInScene());
                Debug.Log("Check_StateController");
            }
            GUILayout.Space(10);
            GUILayout.Label("检查Timeline");
            if (GUILayout.Button("Check Timeline", GUILayout.Width(200)))
            {
                CheckPlayableDirectorInScene();
                Debug.Log("Check_Timeline");
            }
        }



        GUILayout.Space(50);
        GUILayout.Label("开发这玩意太难了");
        GUILayout.Label("没事请多请请作者可乐");
        GUILayout.Label("谢了，太难了");
        GUILayout.Label("这种就行");
        GUILayout.Label("     ||o||    ");
        GUILayout.Label("   | |     | |    ");
        GUILayout.Label("   | |     | |    ");
        GUILayout.Label("   ====    ");
        GUI.skin = cola;
        GUILayout.Box("", GUILayout.Width(120), GUILayout.Height(120));
        GUI.EndScrollView();

    }




    void OnHierarchyChange()
    {
        targetObjects = GetAllEventControllerObjectsInScene();
        if (autoGenerateID)
        { RegisterObjectsID(); }
    }

    void OnInspectorUpdate()
    {
        this.Repaint();
    }



    public EventController[] targetObjects;

    private int numberPool = 0;
    //物品ID__string=Name+Number

    public void SignObjectst()
    {
        if (targetObjects != null)
        { ClearArray(targetObjects); }

        targetObjects = GetAllEventControllerObjectsInScene();
        //print("All " + Resources.FindObjectsOfTypeAll<EventController>().Length);

    }




    public void RegisterObjectsID()
    {
        SignObjectst();
        CaulateID(targetObjects);
    }
    public void RegisterObjectsIDForce()
    {
        SignObjectst();
        CaulateID(targetObjects);
    }
    public void RegisterObjectsIDClearly()
    {
        SignObjectst();
        numberPool = 0;
        CaulateID(targetObjects);
    }
    public void ClearObjectsID()
    {
        
        SignObjectst();
        RemoveID(targetObjects);
        numberPool = 0;
       
    }
    public void FixObjectsID()
    {
        autoGenerateID = false;
        SignObjectst();
        FixID(targetObjects);

    }

    public void CheckPlayableDirectorInScene()
    {
        GameObject target;
        int controllerCount = GetAllPlayablesObjectsInScene(out target);
        if (controllerCount == 1)
        {
            if (target.name != "Timeline_Main")
            {
                target.name = "Timeline_Main";
                Debug.Log("已更正名称");
            }
            else
            {
                Debug.Log("Timeline_Main正常");

            }
        }
        else if (controllerCount > 1)
        {
            Debug.LogError("多于一个Timeline");
        }
        else if (controllerCount == 0)
        {
            GameObject go = new GameObject("StateController_Main");
            go.AddComponent<StateController>();

        }


    }


    public void CheckStateControllerInScene()
    {
        int controllerCount = GetAllStateControllerObjectsInScene();
        GameObject target;
        GetAllStateControllerObjectsInScene(out target);
        if (controllerCount == 1)
        {
            if (target.name != "StateController_Main")
            {
                target.name = "StateController_Main";
                Debug.Log("已更正名称");
            }
            else
            {
                Debug.Log("StateController_Main正常");

            }
        }
        else if (controllerCount > 1)
        {
            Debug.LogError("多于一个Controller");
        }
        else if (controllerCount == 0)
        {
            GameObject go = new GameObject("StateController_Main");
            go.AddComponent<StateController>();

        }
        

    }
    private StateController FindStateOnlyControllerInScene()
    {
        StateController target;
        GameObject temp;
        GetAllStateControllerObjectsInScene(out temp);    
        target = temp.GetComponent<StateController>();
        return target;
    }
    public void RegisterStateController(StateController stateController)
    {
        //EventController[] controllers = GetAllEventControllerObjectsInScene();
        //for (int i = 0; i < controllers.Length; i++)
        //{
        //    controllers[i].stateController = stateController;
        //}

    }



    public void ClearArray(EventController[] eventControllers)
    {
        //if (eventControllers != null)
        //{
        //    RemoveID(eventControllers);
        //}
        eventControllers = new EventController[0];
    }
    public EventController[] GetAllEventControllerObjectsInScene()
    {
        List<EventController> objectsInScene = new List<EventController>();
        EventController[] objects;
        foreach (EventController go in Resources.FindObjectsOfTypeAll<EventController>())
        {

            if (go.hideFlags == HideFlags.NotEditable || go.hideFlags == HideFlags.HideAndDontSave)
            { continue; }


            if (EditorUtility.IsPersistent(go.transform.root.gameObject))
            { continue; }


            objectsInScene.Add(go);

        }
        objects = new EventController[objectsInScene.Count];
        objectsInScene.CopyTo(objects);
        
        return objects;
    }

    
    public int GetAllStateControllerObjectsInScene()
    {
       
        int count = 0;
        
        foreach (StateController go in Resources.FindObjectsOfTypeAll<StateController>())
        {

            if (go.hideFlags == HideFlags.NotEditable || go.hideFlags == HideFlags.HideAndDontSave)
            { continue; }


            if (EditorUtility.IsPersistent(go.transform.root.gameObject))
            { continue; }


            count++;

        }
        
        return count;
    }

    public int GetAllStateControllerObjectsInScene(out GameObject target)
    {
        List<StateController> objectsInScene = new List<StateController>();
        int count = 0;

        foreach (StateController go in Resources.FindObjectsOfTypeAll<StateController>())
        {

            if (go.hideFlags == HideFlags.NotEditable || go.hideFlags == HideFlags.HideAndDontSave)
            { continue; }


            if (EditorUtility.IsPersistent(go.transform.root.gameObject))
            { continue; }

            objectsInScene.Add(go);
            count++;

        }
        if (objectsInScene.Count > 0)
        {
            target = objectsInScene[0].gameObject;
        }
        else
        {
            target = null;
        }
       


        return count;
    }


    public int GetAllPlayablesObjectsInScene(out GameObject target)
    {
        List<PlayableDirector> objectsInScene = new List<PlayableDirector>();
        int count = 0;

        foreach (PlayableDirector go in Resources.FindObjectsOfTypeAll<PlayableDirector>())
        {

            if (go.hideFlags == HideFlags.NotEditable || go.hideFlags == HideFlags.HideAndDontSave)
            { continue; }


            if (EditorUtility.IsPersistent(go.transform.root.gameObject))
            { continue; }

            objectsInScene.Add(go);
            count++;

        }
        if (objectsInScene.Count > 0)
        {
            target = objectsInScene[0].gameObject;
        }
        else
        {
            target = null;
        }



        return count;
    }


    public void CaulateID(EventController[] eventControllers)
    {
        for (int i = 0; i < eventControllers.Length; i++)
        {
            eventControllers[i].objectID = eventControllers[i].name;
            int index = eventControllers[i].objectID.IndexOf("#");
            if (index == -1)
            {
                eventControllers[i].objectID = eventControllers[i].name + "#" + numberPool.ToString("d5");
                eventControllers[i].name = eventControllers[i].objectID;
                numberPool++;
            }
            if (index >= 0)
            {
                string tempString= eventControllers[i].name.Substring(index);
                if (tempString.Length > 6)
                {
                   
                    eventControllers[i].objectID = eventControllers[i].objectID.Remove(index);
                    eventControllers[i].objectID = eventControllers[i].objectID + "#" + numberPool.ToString("d5");
                    eventControllers[i].name = eventControllers[i].objectID;
                    numberPool++;
                }
            }
            
            
           

        }

    }
    public void CaulateIDDorce(EventController[] eventControllers)
    {
        for (int i = 0; i < eventControllers.Length; i++)
        {

                int index = eventControllers[i].objectID.IndexOf("#");
                if (index != -1)
                {
                    eventControllers[i].objectID.Remove(index);
                }
                else
                {
                    Debug.LogError("ID NULL");
                    return;
                }
                eventControllers[i].objectID = eventControllers[i].name + "#" + numberPool.ToString("d5");
            eventControllers[i].name = eventControllers[i].objectID;
            numberPool++;
            

        }

    }
    public void RemoveID(EventController[] eventControllers)
    {
        for (int i = 0; i < eventControllers.Length; i++)
        {  
                int index = eventControllers[i].objectID.IndexOf("#");
                if (index != -1)
                {
                    eventControllers[i].objectID=eventControllers[i].objectID.Remove(index);
                eventControllers[i].name = eventControllers[i].objectID;
            }
                else
                {
                    Debug.Log("ID NULL");
                    return;
                }

            

        }

    }
    public void FixID(EventController[] eventControllers)
    {
        RemoveID(eventControllers);
        for (int i = 0; i < eventControllers.Length; i++)
        {
            int index = eventControllers[i].objectID.IndexOf("#");
            if (index == -1)
            {
                eventControllers[i].objectID = eventControllers[i].name;
                
                //eventControllers[i].name = eventControllers[i].name + eventControllers[i].objectID;
            }
            else
            {
                Debug.Log("ID NULL");
                return;
            }



        }

    }
}

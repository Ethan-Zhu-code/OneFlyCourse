    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Playables;
using System.Reflection;
using System;
public enum MenuType //菜单类型
{
    State,
    Event,
    Delete,
    Line
}

public class AssetGenerateEditor : EditorWindow
{
   
    Rect windowRect = new Rect(50, 50, 150, 100);
    //窗口的ID
    int windownID = 0;
    private Vector2 offset;
    private Vector2 drag;
    /// <summary>
    /// 判断是否点击在窗口上
    /// </summary>
    private bool isClickedOnWindow = false;
    /// <summary>
    /// 窗口容器，用于存放窗口
    /// </summary>
    private List<BaseNode> windows = new List<BaseNode>();
    /// <summary>
    /// 当前选中的窗口的下标
    /// </summary>
    private int selectedIndex = -1;

    /// <summary>
    /// 当前鼠标的位置
    /// </summary>
    private Vector2 mousePos;

    /// <summary>
    /// 判断当前是否为画线模式
    /// </summary>
    private bool isDrawLineModel = false;

    /// <summary>
    /// 当前选中的Node
    /// </summary>
    private BaseNode selectNode;

    private BaseNode drawModeSelectedNode;
    private BaseNode drawModeTargetNode;
    Vector3 scrollPos = Vector2.zero;

    private GUISkin window;
    [MenuItem("Tool/资源控制器")]
   
    static void ShowWindow()
    {

        EditorWindow.GetWindow(typeof(AssetGenerateEditor));
    }
    AssetGenerateEditor()
    {
        this.titleContent = new GUIContent("资源控制器");
    }


 



    private void OnEnable()
    {
        
      
        window = (GUISkin)Resources.Load("Window");

    }

    void OnGUI()
    {
        DrawGrid(20, 0.2f, Color.gray);
        DrawGrid(100, 0.4f, Color.gray);
        scrollPos = GUI.BeginScrollView(new Rect(0, 0, position.width, position.height),
           scrollPos, new Rect(0, 0, 5000, 5000));
        //1.获取当前事件
        Event e = Event.current;
        //获取鼠标的位置
        mousePos = e.mousePosition;
       

        FoundSelectedWindow();
        //if (e.button == 0 && e.isMouse)
        //{
        //    FoundSelectedWindow();
        //}
        if (e.button == 0 && e.isMouse && !isDrawLineModel)
        {
            Repaint();
        }
        if (e.button == 1 && e.isMouse && !isDrawLineModel)
        {
            CreateMenu(e);
        }



        if (isDrawLineModel)
        {
            //FoundSelectedWindow();
            drawModeSelectedNode = windows[selectedIndex];


            if (isClickedOnWindow && drawModeSelectedNode != null)
            {
 
                if (drawModeSelectedNode.windowTitle == "State 节点"
                    && selectNode.windowTitle== "Event 节点")
                {
                    Debug.Log("input_Start");
                    //StateInputNode stateInputNode =new StateInputNode();
                    //drawModeSelectedNode = 
                    if (e.button == 0 && e.isMouse && isClickedOnWindow)
                    {
                        selectNode.SetInput((StateInputNode)drawModeSelectedNode, mousePos);
                        drawModeSelectedNode.SetInput((EventNode)selectNode, mousePos);
                        Debug.Log("input_End");
                        isDrawLineModel = false;
                        selectNode = null;
                        drawModeSelectedNode = null;
                        
                    }  
                }
                else if (drawModeSelectedNode.windowTitle == "Event 节点"
                   && selectNode.windowTitle == "State 节点")
                {
                    Debug.Log("input_Start");
                    //StateInputNode stateInputNode =new StateInputNode();
                    //drawModeSelectedNode = 
                    if (e.button == 0 && e.isMouse && isClickedOnWindow)
                    {
                        selectNode.SetInput((EventNode)drawModeSelectedNode, mousePos);
                        drawModeSelectedNode.SetInput((StateInputNode)selectNode, mousePos);
                        Debug.Log("input_End");
                        isDrawLineModel = false;
                        selectNode = null;
                        drawModeSelectedNode = null;
                    }


                }


            }

            if (e.button == 0 && e.isMouse&&!isClickedOnWindow)
            {
                isDrawLineModel = false;
                    selectNode = null;
                drawModeSelectedNode = null;
            }
            

        }

        //画线功能
        if (isDrawLineModel && selectNode != null)
        {
            Rect endRect = new Rect(mousePos, new Vector2(10, 10));
            DrawBezier(selectNode.WindowRect, endRect);

            Repaint();
        }

       // 维护画线功能
        for (int i = 0; i < windows.Count; i++)
        {
            windows[i].DrawBezier();
        }
        ///
        ///
        GUILayout.Toggle(isClickedOnWindow,"Test isClickedOnWindow");
        GUILayout.Toggle(isDrawLineModel, "TestisDrawLineModel");

        if (selectNode != null)
        {
            GUILayout.Label("selectNode");
            GUILayout.TextField(selectNode.ToString());
        }
        if (drawModeSelectedNode != null)
        {
            GUILayout.Label("drawModeSelectedNode");
            GUILayout.TextField(drawModeSelectedNode.ToString());
        }

        BeginWindows(); //开始绘制弹出窗口

        for (int i = 0; i < windows.Count; i++)
        {
            windows[i].WindowRect = GUI.Window(i, windows[i].WindowRect, WindowCallback, windows[i].windowTitle);
        }



        EndWindows();

        GUI.EndScrollView();


    }
    private void WindowCallback(int id)
    {

        GUI.skin = window;
        windows[id].DrawWindow();
        
        GUI.DragWindow();

    }



    private void CreateMenu(Event e)
    {
        FoundSelectedWindow(); 
        if (isClickedOnWindow)
        {
            GenericMenu menu = new GenericMenu();
          
            menu.AddItem(new GUIContent("Draw Line"), false, MenuCallback, MenuType.Line);
            menu.AddItem(new GUIContent("Delete Node"), false, MenuCallback, MenuType.Delete);
            menu.ShowAsContext();

            e.Use();

            isClickedOnWindow = false;
        }
        else
        {
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("State Node"), false, MenuCallback, MenuType.State);
            menu.AddItem(new GUIContent("Event State"), false, MenuCallback, MenuType.Event);
     

            menu.ShowAsContext();

            e.Use();
        }

    }


    private void FoundSelectedWindow()
    {
        for (int i = 0; i < windows.Count; i++)
        {
            if (windows[i].WindowRect.Contains(mousePos))
            {
                //Debug.Log(windows[i].windowTitle+ selectedIndex);
                isClickedOnWindow = true;
                selectedIndex = i;
                //Debug.Log(windows[i].windowTitle + selectedIndex);
                break;
            }
            else
            {
                isClickedOnWindow = false;
            }
        }
    }


    private void DrawGrid(float gridSpacing, float gridOpacity, Color gridColor)
    {
        int widthDivs = Mathf.CeilToInt(position.width / gridSpacing);
        int heightDivs = Mathf.CeilToInt(position.height / gridSpacing);

        Handles.BeginGUI();
        Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);

        offset += drag * 0.5f;
        Vector3 newOffset = new Vector3(offset.x % gridSpacing, offset.y % gridSpacing, 0);

        for (int i = 0; i < widthDivs; i++)
        {
            Handles.DrawLine(new Vector3(gridSpacing * i, -gridSpacing, 0) + newOffset, new Vector3(gridSpacing * i, position.height, 0f) + newOffset);
        }

        for (int j = 0; j < heightDivs; j++)
        {
            Handles.DrawLine(new Vector3(-gridSpacing, gridSpacing * j, 0) + newOffset, new Vector3(position.width, gridSpacing * j, 0f) + newOffset);
        }

        Handles.color = Color.white;
        Handles.EndGUI();
    }
    private void MenuCallback(object type)
    {
        //Debug.Log("Enter!!!" + ((MenuType)type).ToString());
        switch ((MenuType)type)
        {
            //在鼠标位置创建指定大小的小窗口
            case MenuType.State:
                StateInputNode stateNode = new StateInputNode();
                stateNode.WindowRect = new Rect(mousePos.x, mousePos.y, 250, 300);
                windows.Add(stateNode);
                break;
            case MenuType.Event:
                EventNode eventNode = new EventNode();
                eventNode.WindowRect = new Rect(mousePos.x, mousePos.y, 250, 300);
                windows.Add(eventNode);
                break;
         
           
            case MenuType.Line:
               // FoundSelectedWindow();
                //1.找到开始的位置（矩形）
                selectNode = windows[selectedIndex];
                //2.切换当前模式为画线模式
                isDrawLineModel = true;
                break;
            case MenuType.Delete:
                //删除对应的子窗口
                for (int i = 0; i < windows.Count; i++)
                {
                    windows[i].DeleteNode(windows[selectedIndex]);
                }

                windows.RemoveAt(selectedIndex);
                break;
            default:
                throw new ArgumentOutOfRangeException("type", type, null);
        }
    }
    public static void DrawBezier(Rect start, Rect end)
    {
        Vector3 startPos = new Vector3(start.x + start.width , start.y + start.height / 2, 0);
        Vector3 endPos = new Vector3(end.x + end.width, end.y + end.height / 2, 0);
        Vector3 startTan = startPos + Vector3.right * 100;
        Vector3 endTan = endPos + Vector3.left * 100;
        Color shadow = new Color(0.1f, 0.1f, 0.1f, 0.7f);

        for (int i = 0; i < 5; i++)
        {
            Handles.DrawBezier(startPos, endPos, startTan, endTan, shadow, null, 1 + (i * 2));
        }

        Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.green, null, 1);

    }

}

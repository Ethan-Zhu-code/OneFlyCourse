using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BaseNode
{
    public string windowTitle; //结点窗口标题
    public Rect WindowRect; //窗口框

    Vector3 scrollPos = Vector2.zero;

    public virtual void DrawWindow()
    {
      
        EditorGUILayout.LabelField(windowTitle);
      
    }

    public virtual void SetInput(StateInputNode inputNode, Vector2 mousePos)
    {

    }
    public virtual void SetInput(EventNode inputNode, Vector2 mousePos)
    {

    }
    //public virtual void SetInputEventNodes(EventNode inputNode, Vector2 mousePos)
    //{

    //}

    public virtual void DrawBezier()
    {
    }

    public virtual void DeleteNode(BaseNode node)
    {
    }

}

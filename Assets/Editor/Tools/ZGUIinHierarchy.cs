using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Playables;
using OneFlyLib;
using Universal.Card;

[InitializeOnLoad]
public class ZGUIinHierarchy {
   
    static ZGUIinHierarchy()
    {
        EditorApplication.hierarchyWindowItemOnGUI += HierarchyTips;
    }

    private static void HierarchyTips(int instanced, Rect rect)
    {
        var obj = EditorUtility.InstanceIDToObject(instanced) as GameObject;
        if (obj != null)
        {
            if (obj.GetComponent<StateController>())
            {
                Rect r = new Rect(rect);
                r.x = r.width - 20;
                r.width = 80;
                r.y += 2;
                GUIStyle style = new GUIStyle();
                style.normal.textColor = Color.red;
                GUI.Label(r, "[State]", style);
            }
            if (obj.GetComponent<EventController>())
            {
                Rect r = new Rect(rect);
                r.x = r.width - 20;
                r.width = 80;
                r.y += 2;
                GUIStyle style = new GUIStyle();
                style.normal.textColor = Color.blue;
                GUI.Label(r, "[Event]", style);
            }
            if (obj.GetComponent<PlayableDirector>())
            {
                Rect r = new Rect(rect);
                r.x = r.width - 35;
                r.width = 80;
                r.y += 2;
                GUIStyle style = new GUIStyle();
                style.normal.textColor = Color.green;
                GUI.Label(r, "[Timeline]", style);
            }
            if (obj.GetComponent<ModelControl>())
            {
                Rect r = new Rect(rect);
                r.x = r.width - 35;
                r.width = 80;
                r.y += 2;
                GUIStyle style = new GUIStyle();
                style.normal.textColor = Color.yellow;
                GUI.Label(r, "[ModelControl]", style);
            }
            if (obj.GetComponent<ReflectionProbe>())
            {
                Rect r = new Rect(rect);
                r.x = r.width - 35;
                r.width = 80;
                r.y += 2;
                GUIStyle style = new GUIStyle();
                style.normal.textColor = new Color(0.3f,1,0.6f,1);
                GUI.Label(r, "[反射球]", style);
            }
            if (obj.GetComponent<Camera>()&& obj.GetComponent<Camera>().targetTexture==null)
            {
                Rect r = new Rect(rect);
                r.x = r.width - 35;
                r.width = 80;
                r.y += 2;
                GUIStyle style = new GUIStyle();
                style.normal.textColor = new Color(0.6f,0.3f,0.2f, 1);
                GUI.Label(r, "[Camera]", style);
            }
            if (obj.GetComponent<Camera>() && obj.GetComponent<Camera>().targetTexture != null)
            {
                Rect r = new Rect(rect);
                r.x = r.width - 35;
                r.width = 80;
                r.y += 2;
                GUIStyle style = new GUIStyle();
                style.normal.textColor = new Color(0.4f, 0.7f, 0.8f, 1);
                GUI.Label(r, "[Render]", style);
            }
            if (obj.GetComponent<CardControl>())
            {
                Rect r = new Rect(rect);
                r.x = r.width - 35;
                r.width = 80;
                r.y += 2;
                GUIStyle style = new GUIStyle();
                style.normal.textColor = new Color(1, 0.3f, 0.6f, 1);
                GUI.Label(r, "[CardControl]", style);
            }
            if (obj.tag=="EditorOnly")
            {
                Rect r = new Rect(rect);
                r.x = r.width - 35;
                r.width = 80;
                r.y += 2;
                GUIStyle style = new GUIStyle();
                style.normal.textColor = Color.gray;
                GUI.Label(r, "[编辑器工具]", style);
            }
            if (obj.name == "Targets")
            {
                Rect r = new Rect(rect);
                r.x = r.width - 35;
                r.width = 80;
                r.y += 2;
                GUIStyle style = new GUIStyle();
                style.normal.textColor = new Color(1,0,1,1);
                GUI.Label(r, "[Targets]", style);
            }
        }

    }


}

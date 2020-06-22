
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(BezierPipeRenderer))]
[CanEditMultipleObjects]
public class CreatPipe : Editor
{
    BezierPipeRenderer bezierPipeRenderer;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        bezierPipeRenderer = (BezierPipeRenderer)target;

        bezierPipeRenderer.preview = EditorGUILayout.Toggle("Preview", bezierPipeRenderer.preview);


        if (GUILayout.Button("Creat Pipe"))
        {
            bezierPipeRenderer.CreatMesh();
        }

    }


}

#endif
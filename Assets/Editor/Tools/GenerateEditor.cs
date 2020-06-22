using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(IDGenerate))]
[CanEditMultipleObjects]


public class GenerateEditor : Editor
{
    //IDGenerate iDGenerate;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
      //  iDGenerate = (IDGenerate)target;


        //if (GUILayout.Button("Sign Objects"))
        //{
            
        //    iDGenerate.SignObjectst();
        //    Debug.Log("Signed");
        //}
        //if (GUILayout.Button("Register Objects ID"))
        //{

        //    iDGenerate.RegisterObjectsID();
        //    Debug.Log("Register");
        //}

    }
   

    // void OnHierarchyChange()
    //{
    //    //iDGenerate = (IDGenerate)target;
    //    //iDGenerate.RegisterObjectsID();
    //    Debug.Log("change");
    //}
}


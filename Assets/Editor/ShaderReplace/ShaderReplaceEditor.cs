using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ShaderReplaceEditor : EditorWindow {
    [MenuItem("批量工具/材质替换器")]
    static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(ShaderReplaceEditor));
    }
    ShaderReplaceEditor()
    {
        this.titleContent = new GUIContent("材质替换器");
    }



    private GameObject[] objects = new GameObject[0];
    private Material[] defaultMaterials;
    private Material targetMaterial;

    Material[] temp;
    void OnGUI()
    {
        SelectObjects();
        EditorGUILayout.LabelField("替换的材质");
        targetMaterial = EditorGUILayout.ObjectField(targetMaterial, typeof(Material), false, GUILayout.Width(150)) as Material;
        if (GUILayout.Button("Replace"))
        {
            if (objects.Length > 0)
            {
                ReplaceMaterial(objects);
            }
            Debug.Log("Replace");
        }
        Repaint();

    }


    private void SelectObjects()
    {
        if (Selection.objects.Length != 0)
        {
            foreach (var go in Selection.objects)
            {
                objects = new GameObject[Selection.objects.Length];
                for (int i = 0; i < Selection.objects.Length; i++)
                {
                    if (Selection.objects.GetType() == typeof(GameObject))
                    {
                        Debug.Log("::::::::");
                    }
                    objects[i] = (GameObject)Selection.objects[i];
                }
            }
        }
        else
        {
            objects = new GameObject[0];
        }
        if (objects.Length > 0)
        {
            for (int i = 0; i < objects.Length; i++)
            {
                GUILayout.Label(objects[i].name);
            }
        }
        else
        {
            GUILayout.Label("Null");
        }
    }

    private void ReplaceMaterial(GameObject[] objects )
    {
        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i].GetComponent<MeshRenderer>())
            {
                temp = new Material[objects[i].GetComponent<MeshRenderer>().sharedMaterials.Length];
                for (int j = 0; j < temp.Length; j++)
                {
                    temp[j] = targetMaterial;
                }

                objects[i].GetComponent<MeshRenderer>().sharedMaterials = temp;
            }
            }


        }

    }



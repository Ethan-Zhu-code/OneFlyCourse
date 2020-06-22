using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ImcFile", fileName = "ImcFile", order = 3)]
public class ImcFile:ScriptableObject
{
    public string GroupName = "";                         //g gameobject.name
    public List<Vector3> PointList= new List<Vector3>(); //v mesh.vertices
    public List<int> FaceList = new List<int>();          //f	mesh.triangles
}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HighlightingSystem;

public class Highlight : MonoBehaviour
{
    public Highlighter[] lighter;
    void Start()
    {
        
    }

    //开始高亮
    public void Startlight()
    {
        for(int i = 0; i < lighter.Length; i++)
        {
            lighter[i].TweenStart();
        }
    }

    //停止高亮
    public void Stoplight()
    {
        for(int i = 0; i < lighter.Length; i++)
        {
            lighter[i].TweenStop();
        }
    }
}

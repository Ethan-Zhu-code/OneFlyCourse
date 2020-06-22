using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DevelopEngine;


public class CameraControl : MonoSingleton<CameraControl> {
    public GameObject[] Targets;
    public GameObject Camera;
    // Use this for initialization
    private void Start()
    {
        Init();
        ChangePos(0);
    }
    private void Awake()
    {
        //Camera = GetComponentInChildren<Camera>().gameObject;
       
    }

    private void Init()
    {
        if (Camera == null)
        {
            Camera = GameObject.Find("Camera");
            Center = GameObject.Find("Center");
            GameObject cameraPos = GameObject.Find("CameraPos");
            var list = new List<GameObject>() { cameraPos};
            Targets = list.ToArray();
        }
    }

   
    public void ChangePos(int num,float time=999)
    {
        lock (gameObject)
        {
            currentNumber = num;
            if (Targets == null || Camera == null) return;
            if (time != 999)
            {
                Camera.transform.DOMove(Targets[num].transform.position, time);
                Camera.transform.DORotate(Targets[num].transform.rotation.eulerAngles, time);
            }
            else
            {
                Camera.transform.position = Targets[num].transform.position;
                Camera.transform.rotation = Targets[num].transform.rotation;
            }
        }
    }
    
    public GameObject Center;
    /// <summary>
    /// 当前目标编号
    /// </summary>
    int currentNumber;
    public void ResetPos()
    {
        ChangePos(currentNumber);
    }
    public void Rotate(float angel)
    {
        lock (gameObject)
        {
            Init();
            Camera.transform.RotateAround(Center.transform.position, Vector3.up, angel);
        }
    }
}

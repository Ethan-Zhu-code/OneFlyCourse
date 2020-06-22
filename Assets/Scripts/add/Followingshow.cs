using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Followingshow : MonoBehaviour
{
    public GameObject[] follows;
    
    public void FollowShow()
    {
        for (int i = 0; i < follows.Length; i++)
            follows[i].SetActive(true);
    }

    public void FollowHide()
    {
        for (int i = 0; i < follows.Length; i++)
            follows[i].SetActive(false);
    }
}

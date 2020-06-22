using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

public class TimelineTest : MonoBehaviour
{
    PlayableDirector playableDirector;

    [SerializeField]
    TimelineAsset[] timelineAssets;

    int index = 0;
    bool testing = false;
    [Header("播放所有timeline时的播放间隔（单位：秒)")]
    public float inteval=0;
    float intvl;

    void Start()
    {
        playableDirector = GetComponent<PlayableDirector>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayNextTimelineAsset();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            standBy();
            StartTest();
        }


        if (testing)
        {
            if (index== timelineAssets.Length)
            {
                testing = false;
            }

            if (playableDirector.state == PlayState.Paused)
            {
                intvl += Time.fixedDeltaTime;
            }

            if (intvl >=inteval)
            {
                intvl = 0;
                PlayNextTimelineAsset();
            }
        }
    }


    public void PlayNextTimelineAsset() //若未在播放，播放下一动画
    {
        if (playableDirector.state == PlayState.Paused)
        {
            if (index < timelineAssets.Length)
            {
                playableDirector.Play(timelineAssets[index++]);
            }
        }
    }

    public void StartTest()         //开始全动画播放
    {
        testing = true;
    }

    public void standBy()           //置高间隔时间
    {
        intvl = inteval;

    }



}

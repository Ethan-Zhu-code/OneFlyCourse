using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Startfire : MonoBehaviour
{
    public ParticleSystem[] fire;
    

    public void StartFire(int id)
    {
        fire[id].Play();
    }

    public void StopFile(int id)
    {
        fire[id].Stop();
    }
}

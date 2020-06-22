using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class StartMovie : MonoBehaviour
{
    //private MovieTexture movie;
    public bool isShowLogo = false;
    private VideoPlayer player;
    // Use this for initialization
    void Awake()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        if (Display.displays.Length > 1)
            Display.displays[1].Activate();
    }
    void Start()
    {
        player = GetComponent<VideoPlayer>();
        if (isShowLogo)
        {
            //movie = Resources.Load("open the door") as MovieTexture;//door_0
            float time = (float)player.clip.length;
            Invoke("Load", time);
        }
        else
        {
            Invoke("Load", 0f);
        }
    }

    // Update is called once per frame
    //void OnGUI()
    //{
    //    if (!isShowLogo) return;
    //    GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), movie, ScaleMode.StretchToFill);
    //    if (!movie.isPlaying)
    //    {
    //        movie.Play();
    //    }
    //}
    void Load()
    {
        SceneManager.LoadScene(1);
    }
    private void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
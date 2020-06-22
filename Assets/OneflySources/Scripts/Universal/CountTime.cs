using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Universal;
using Universal.Audio;

public class CountTime : MonoBehaviour
{
	// Use this for initialization
	void Start () {

    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.A))
        {
            StartCoroutine(Count());

        }
    }


    IEnumerator Count()
    {
        for (int i = 3; i > 0; i--)
        {
            AudioPlayer.Instance.PlayAudio(Universal.Audio.AudioType.speech,1);
            transform.GetComponentInChildren<Text>().text = i.ToString();
            
                yield return new WaitForSeconds(1);
           
        }

        Destroy(gameObject);

    }
}

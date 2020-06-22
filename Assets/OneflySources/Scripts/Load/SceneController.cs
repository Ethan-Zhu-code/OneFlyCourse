using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Universal
{
    public class SceneController : MonoBehaviour
    {
        private int displayProgress;
        public AsyncOperation asyncOperation;
        private Coroutine loadCoroutine;

        public void LoadScene(string sceneName, Action OnSceneLoaded = null)
        {
            if (loadCoroutine == null)
            {
                loadCoroutine = StartCoroutine(DoLoadScene(sceneName, OnSceneLoaded));
            }
        }

        public IEnumerator DoLoadScene(string sceneName, Action OnSceneLoaded)
        {
            displayProgress = 0;
            int toProgress = 0;

            asyncOperation = SceneManager.LoadSceneAsync(sceneName);
            asyncOperation.allowSceneActivation = false;
            while (asyncOperation.progress < 0.9f)
            {
                toProgress = (int)asyncOperation.progress * 100;
                while (displayProgress < toProgress)
                {
                    ++displayProgress;
                    //SetLoadingPercentage(displayProgress);
                    yield return new WaitForEndOfFrame();
                }
            }
            toProgress = 100;
            while (displayProgress < toProgress)
            {
                ++displayProgress;
                //SetLoadingPercentage(displayProgress);
                yield return new WaitForEndOfFrame();
            }
            //asyncOperation.allowSceneActivation = true;
            loadCoroutine = null;
            if (OnSceneLoaded != null)
            {
                OnSceneLoaded();
            }
        }

        //public void SetLoadingPercentage(int DisplayProgress)
        //{
        //    m_Slider.value = DisplayProgress * 0.01f;
        //    m_Text.text = DisplayProgress.ToString() + "%";
        //}
    }
}

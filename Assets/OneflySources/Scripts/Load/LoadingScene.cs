using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Universal
{
    public class LoadingScene : MonoBehaviour
    {
        public bool isShowLogo = false;
        private MovieTexture movie;
        private SceneController sceneController;
        private Coroutine switchSceneCoroutine = null;

        void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            movie = Resources.Load("Videos/door_0") as MovieTexture;
            sceneController = GetComponent<SceneController>();
            if (sceneController == null)
            {
                sceneController = gameObject.AddComponent<SceneController>();
            }
            if (Display.displays.Length > 1)
                Display.displays[1].Activate();
        }
        void Start()
        {
            if (isShowLogo)
            {
                if (switchSceneCoroutine == null)
                {
                    switchSceneCoroutine = StartCoroutine(SceneSwitching());
                }
            }
            else
            {
                SceneManager.LoadScene("Page0");
            }
        }

        void OnGUI()
        {
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), movie, ScaleMode.StretchToFill);
        }

        IEnumerator SceneSwitching()
        {
            bool isLoadFinished = false;
            sceneController.LoadScene("Page0", () => { isLoadFinished = true; });
            float playTime = 0f;
            movie.Play();
            float maxTime = movie.duration + 7.5f;
            while (playTime <= maxTime || !isLoadFinished)
            {
                playTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            //isShowLogo = false;
            sceneController.asyncOperation.allowSceneActivation = true;
        }

        private void OnDisable()
        {
            if (switchSceneCoroutine != null)
            {
                StopCoroutine(switchSceneCoroutine);
            }
            switchSceneCoroutine = null;
        }
    }
}
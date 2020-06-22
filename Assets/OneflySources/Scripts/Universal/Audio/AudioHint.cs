using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Universal;
using DevelopEngine;
using OneFlyLib;
using System;

/// <summary>
/// New Edition
/// </summary>

namespace Universal.Audio
{
    public enum HintType
    {
        /// <summary>
        /// 当前文件内加载
        /// </summary>
        LOCAL,
        /// <summary>
        /// 从配置文件加载
        /// </summary>
        LOAD
    }

    public class AudioHint : MonoSingleton<AudioHint>
    {
        private string[] AudioHintInfo =
        {
        "穷居闹市无人问，富在深山有远亲，人情冷暖多变化，世事无常换乾坤",
    };

        private Text audioInfo;
        private RectTransform backgroundTransform;
        float bgHeight;
        public HintType hintType = HintType.LOCAL;

        private void Awake()
        {
            audioInfo = gameObject.GetComponentInChildren<Text>();
            backgroundTransform = transform.Find("background").GetComponent<RectTransform>();
        }

        int GetLineCount()
        {
            string strText = audioInfo.text;
            audioInfo.text = "";
            float linrHeight = audioInfo.preferredHeight;
            audioInfo.text = strText;

            return (int)(audioInfo.preferredHeight / 23f - 1);
        }

        private Coroutine showCoroutine = null;

        #region 根据下标播放音频
        /// <summary>
        /// 播放音频和音频提示
        /// </summary>   
        /// <param name="audioIndex">音频下标</param> 
        /// <param name="strIndex">音频提示下标(从配置文件加载提示时可不赋值)</param>
        public void ShowHint(int audioIndex, int strIndex = 0)
        {
            StopShowCoroutine();
            showCoroutine = StartCoroutine(StartHint(audioIndex, strIndex));
        }

        IEnumerator StartHint(int index, int strIndex)
        {
            SetHintAndAnim(strIndex);
            yield return AudioPlayer.Instance.SpeechPlay(index, StopShowCoroutine);
        }

        //设置提示文本和提示框动画
        private void SetHintAndAnim(int strIndex)
        {
            bgHeight = backgroundTransform.sizeDelta.y;
            switch (hintType)
            {
                case HintType.LOCAL:
                    audioInfo.text = AudioHintInfo[strIndex];
                    break;
                case HintType.LOAD:
                    audioInfo.text = Configuration.GetContent("AudioHint", strIndex.ToString());
                    break;
            }
            gameObject.transform.DOLocalMoveX(-500f, 1.2f);
            backgroundTransform.sizeDelta = new Vector2(backgroundTransform.sizeDelta.x, GetLineCount() * 30 + 115);
        }
        #endregion

        #region 直接播放音频
        /// <summary>
        /// 直接播放音频和音频提示
        /// </summary>
        /// <param name="clip">要播放的音频</param>
        /// <param name="strIndex">音频提示下标</param>
        /// <param name="action">播放完成回调</param>
        public void ShowHint(AudioClip clip, string content, Action action = null)
        {
            StopShowCoroutine();
            showCoroutine = StartCoroutine(StartHint(clip, content, action));
        }

        IEnumerator StartHint(AudioClip clip, string content, Action action = null)
        {
            bgHeight = backgroundTransform.sizeDelta.y;
            audioInfo.text = content;
            gameObject.transform.DOLocalMoveX(-500f, 1.2f);
            backgroundTransform.sizeDelta = new Vector2(backgroundTransform.sizeDelta.x, GetLineCount() * 30 + 115);
            action += StopShowCoroutine;
            yield return AudioPlayer.Instance.SpeechPlay(clip, action);
        }

        #endregion

        /// <summary>
        /// 只播放音频不显示文本
        /// </summary>
        /// <param name="audioindex"></param>
        public void ShowHint(int audioindex)
        {
            StopShowCoroutine();
            showCoroutine = StartCoroutine(StartHint(audioindex));
        }
        IEnumerator StartHint(int index)
        {
            yield return AudioPlayer.Instance.SpeechPlay(index, StopShowCoroutine);
        }
        public void StopShowCoroutine()
        {
            AudioPlayer.Instance.StopSpeech();
            gameObject.transform.DOLocalMoveX(500f, 1.2f);
            if (showCoroutine != null)
            {
                StopCoroutine(showCoroutine);
                showCoroutine = null;
            }
        }
    }
}

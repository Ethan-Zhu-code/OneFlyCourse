using OneFlyLib;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Universal.CheckStage
{
    public class StageControl : MonoBehaviour
    {
        [Header("是否锁定鼠标")]
        public bool isLockCursor = true;
        [Header("实验步骤数")]
        public int StageCheckCount;

        public static string GoToBegin = "GoToBegin";
        public static string GoToEnd = "GoToEnd";

        public static string StageEntered = "StageEntered";
        public static string StageFinished = "StageFinished";
        public static string StageExited = "StageExited";

        private List<StageCheck> stageCheckList = new List<StageCheck>();
        private int stageCheckIndex = -1;
        private Coroutine stageCoroutine = null;

        private void Awake()
        {
            InitAllStages();
        }

        private void InitAllStages()
        {
            for (int i = 0; i < StageCheckCount; i++)
            {
                var nameSpace = typeof(StageControl).Namespace;
                string classFullName = string.Format("StageCheck{0}", i.ToString());
                if (!string.IsNullOrEmpty(nameSpace))
                    classFullName = nameSpace + "." + classFullName;
                Type type = Type.GetType(classFullName);
                if (type != null)
                {
                    var check = Activator.CreateInstance(type, i) as StageCheck;
                    stageCheckList.Add(check);
                }
            }
        }

        private void Start()
        {
            //ManagerEvent.Send(DownScreenUIControl.SetIfCheckCardAvailable, true); //开启卡牌识别 部分项目可以不需要这一句
            GoToBeginHandler();
        }

        private void OnEnable()
        {
            ManagerEvent.Register(GoToBegin, GoToBeginHandler);
            ManagerEvent.Register(GoToEnd, GoToEndHandler);
            ManagerEvent.Register(StageEntered, StageEnteredHandler);
            ManagerEvent.Register(StageFinished, StageFinishedHandler);
            ManagerEvent.Register(StageExited, StageExitedHandler);
        }

        private void OnDisable()
        {
            ManagerEvent.Register(GoToBegin, GoToBeginHandler);
            ManagerEvent.Register(GoToEnd, GoToEndHandler);
            ManagerEvent.Unregister(StageEntered, StageEnteredHandler);
            ManagerEvent.Unregister(StageFinished, StageFinishedHandler);
            ManagerEvent.Unregister(StageExited, StageExitedHandler);
        }

        private void Update()
        {
            lockCursor();
        }

        private void lockCursor()
        {
            if (isLockCursor)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        // --------------------------------------------------------------

        private void GoToBeginHandler(params object[] args)
        {
            if (stageCheckList != null && stageCheckList.Count > 0)
            {
                for (int i = 0; i < stageCheckList.Count; i++)
                {
                    stageCheckList[i].ResetStage();
                }
            }

            EnterStage(0);
        }

        private void GoToEndHandler(params object[] args)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
                        Application.Quit();
#endif
        }

        // --------------------------------------------------------------
        private void EnterStage(object arg)
        {
            if (arg != null && arg is int)
            {
                int index = (int)arg;
                //Debug.Log("EnterStageHandler 指定 stage：" + index);
                StartStageEnterOrExit(index, true);
            }
            else
            {
                //Debug.Log("EnterStageHandler 下一个");
                StartStageEnterOrExit(stageCheckIndex + 1, true);
            }
        }

        private void StartStageEnterOrExit(int index, bool isEnter)
        {
            if (0 <= index && index < stageCheckList.Count && stageCoroutine == null)
            {
                //Debug.Log("StartStageEnterOrExit ~~ isEnter :" + isEnter);
                stageCheckIndex = index;
                if (isEnter)
                {
                    stageCoroutine = StartCoroutine(stageCheckList[index].StartEnter());
                }
                else
                {
                    stageCoroutine = StartCoroutine(stageCheckList[index].StartExit());
                }
            }
        }

        // --------------------------------------------------------------
        private bool CheckStageIndex(params object[] args)
        {
            if (args != null && args.Length == 1 && args[0] is int)
            {
                if (stageCheckIndex == (int)args[0])
                {
                    if (stageCoroutine != null)
                    {
                        StopCoroutine(stageCoroutine);
                        stageCoroutine = null;
                    }
                    return true;
                }
                else return false;
            }
            else return false;
        }

        private void StageEnteredHandler(params object[] args)
        {
            if (CheckStageIndex(args))
            {
                //Debug.Log("StageEnteredHandler stage: " + stageCheckIndex);
            }
        }

        private void StageFinishedHandler(params object[] args)
        {
            if (CheckStageIndex(args))
            {
                //Debug.Log("StageFinishedHandler stage: " + stageCheckIndex);
                StartStageEnterOrExit(stageCheckIndex, false);
            }
        }

        private void StageExitedHandler(params object[] args)
        {
            if (CheckStageIndex(args))
            {
                //Debug.Log("StageExitedHandler stage: " + stageCheckIndex);
                EnterStage(null);
            }
        }
    }
}

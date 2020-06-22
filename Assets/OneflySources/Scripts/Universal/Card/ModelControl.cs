using OneFlyLib;
using System;
using UnityEngine;

namespace Universal.Card
{
    public enum MoveType
    {
        REALTIME,
        DELTA,
        OTHER
    }

    /// <summary>
    /// 卡牌3D物体的控制
    /// </summary>
    public class ModelControl : LabObject
    {

        #region Editor Properties
        [HideInInspector]
        public bool showMoveSetting;
        [HideInInspector]
        public bool showEvents;
        [HideInInspector]
        public bool showAdvanced;
        #endregion
        /// <summary>
        /// 当前卡牌位置
        /// </summary>
        //protected Vector3 currVec;
        /// <summary>
        /// 当前模型位置
        /// </summary>
        protected Vector3 currPos;

        [Header("3D物体移动方式")]
        public MoveType moveType = MoveType.DELTA;

        [Header("增量移动的缩放系数")]
        public float ratio = 1;

        private GameObject ui;

        private bool flag = false;

        public bool stayIn = true;

        public bool updatePos = true;
        public bool updateAngle = false;

        [Header("X轴上反向移动")]
        public bool invertX;

        [Header("Y轴上反向移动")]
        public bool invertY;

        [Header("Z轴上反向移动")]
        public bool invertZ;

        /// <summary>
        /// 当前物体的初始化
        /// </summary>
        public override void Init()
        {
            base.Init();
            //ui = GetUI();
            //Debugger.Log(ui.name.ToString());
        }

        /// <summary>
        /// 物体的入场动画（若有，只执行一次）
        /// </summary>
        public override void SetTween()
        {
            base.SetTween();
            currPos = transform.localPosition;
        }

        private Vector3 offset;
        private float distance;

        /// <summary>
        /// 通过位置处理事件
        /// </summary>
        /// <param name="pos"></param>
        public override void HandlerEventByPos(Vector3 pos)
        {
            if (updatePos && TimelineManager.Instance.playableDirector.state == UnityEngine.Playables.PlayState.Paused)
                switch (moveType)
                {
                    case MoveType.REALTIME:
                        transform.localPosition = pos;
                        break;

                    case MoveType.DELTA:
                        //初始化增量移动的初始位置
                        //OnSingleEvent(() => {
                        //    InitDeltaVec(pos);
                        //    currPos = transform.localPosition;
                        //});
                        currPos = transform.localPosition;
                        MovementDelta(pos);
                        break;

                    case MoveType.OTHER:
                        break;

                    default:
                        break;
                }
            else
                flag = false;
        }

        protected override Vector3 GetTransformPoint(float xpos, float ypos)
        {
            if (isMappingGround) { return Vector3.zero; }
            else
            {
                float x; float y; float z; Vector3 vec; switch (moveSpace)
                {
                    case MOVE_SPACE.NOMOVE: return transform.position;

                    case MOVE_SPACE.XY: if (isAccordCamera) { float h = Mathf.Abs(transform.position.z - mainCam.transform.position.z); Vector3 pos = mainCam.ViewportToScreenPoint(new Vector3(xpos, 1 - ypos, 0)); x = (pos.x - Screen.width * 0.5f) / (Screen.width * 0.5f); y = (pos.y - Screen.height * 0.5f) / (Screen.height * 0.5f); float posValue = Mathf.Tan(mainCam.fieldOfView * 0.5f * Mathf.Deg2Rad); float width = h * posValue; vec = mainCam.transform.TransformPoint(new Vector3(x * width * ((float)Screen.width / (float)Screen.height), y * width, 0)); vec = new Vector3(vec.x, vec.y, transform.position.z); return vec; } else { x = moveRange.x * (xpos - 0.5f); y = moveRange.y * (0.5f - ypos); vec = new Vector3(x, y, transform.position.z); return vec; }
                    case MOVE_SPACE.XZ: x = moveRange.x * (xpos - 0.5f); z = moveRange.y * (0.5f - ypos); vec = new Vector3(x, transform.position.y, z); return vec;

                    case MOVE_SPACE.YZ: y = moveRange.y * (0.5f - ypos); z = moveRange.x * (xpos - 0.5f); vec = new Vector3(transform.position.x, y, z); return vec;

                    case MOVE_SPACE.X: x = moveRange.x * (xpos - 0.5f); vec = new Vector3(x, transform.position.y, transform.position.z); return vec;

                    case MOVE_SPACE.Y: y = moveRange.y * (0.5f - ypos); vec = new Vector3(transform.position.x, y, transform.position.z); return vec;

                    case MOVE_SPACE.Z: z = moveRange.y * (0.5f - ypos); vec = new Vector3(transform.position.x, transform.position.y, z); return vec;

                    default: return Vector3.zero;
                }
            }
        }

        //增量移动
        private void MovementDelta(Vector3 pos)
        {
            offset = pos - currVec;
            distance = Vector3.Distance(pos, currVec);
            if (distance < 0.01f)
                return;

            //模型的父级为零点时可以直接用offset向量
            Vector3 tempPos = TempPosIncrement(currPos);

            if (flag)
            {
                //transform.localPosition = tempPos;
                transform.localPosition = GetCurrentPositionWithRestriction(tempPos);
            }
            else
                flag = true;
            currPos = transform.localPosition;
            currVec = pos;
        }

        //获取经限制处理后的坐标向量
        private Vector3 GetCurrentPositionWithRestriction(Vector3 pos)
        {
            switch (moveSpace)
            {
                case MOVE_SPACE.NOMOVE: return pos;
                case MOVE_SPACE.X: return new Vector3(Mathf.Clamp(pos.x, minX, maxX), pos.y, pos.z);
                case MOVE_SPACE.Y: return new Vector3(pos.x, Mathf.Clamp(pos.y, minY, maxY), pos.z);
                case MOVE_SPACE.Z: return new Vector3(pos.x, pos.y, Mathf.Clamp(pos.z, minZ, maxZ));
                case MOVE_SPACE.XY: return new Vector3(Mathf.Clamp(pos.x, minX, maxX), Mathf.Clamp(pos.y, minY, maxY), pos.z);
                case MOVE_SPACE.XZ: return new Vector3(Mathf.Clamp(pos.x, minX, maxX), pos.y, Mathf.Clamp(pos.z, minZ, maxZ));
                case MOVE_SPACE.YZ: return new Vector3(pos.x, Mathf.Clamp(pos.y, minY, maxY), Mathf.Clamp(pos.z, minZ, maxZ));
                default: return pos;
            }
        }

        //根据移动轴向计算增量后的位置
        private Vector3 TempPosIncrement(Vector3 increment)
        {
            if (invertY)
                offset.y *= -1;
            if (invertX)
                offset.x *= -1;
            if (invertZ)
                offset.z *= -1;

            switch (moveSpace)
            {
                case MOVE_SPACE.NOMOVE: return increment;
                case MOVE_SPACE.X: return increment + new Vector3((offset * ratio).x, 0, 0);
                case MOVE_SPACE.Y: return increment + new Vector3(0, (offset * ratio).y, 0);
                case MOVE_SPACE.Z: return increment + new Vector3(0, 0, (offset * ratio).z);
                case MOVE_SPACE.XY: return increment + new Vector3((offset * ratio).x, (offset * ratio).y, 0);
                case MOVE_SPACE.XZ: return increment + new Vector3((offset * ratio).x, 0, (offset * ratio).z);
                case MOVE_SPACE.YZ: return increment + new Vector3(0, (offset * ratio).y, (offset * ratio).z);
                default: return increment;
            }
        }

        /// <summary>
        /// 通过角度处理事件
        /// </summary>
        /// <param name="angle"></param>
        public override void HandlerEventByAngle(float angle)
        {
            if (updateAngle)
                base.HandlerEventByAngle(angle);
            //Debugger.Log("模型角度:" + angle.ToString());
        }

        /// <summary>
        /// 物体的出场动画（若有，只执行一次）
        /// </summary>
        public override void SetTweenBack()
        {
            if (!stayIn)
                base.SetTweenBack();
            flag = false;
        }

        /// <summary>
        /// 物体的重置（只执行一次）
        /// </summary>
        public override void EquipReset()
        {
            base.EquipReset();
        }

        private bool single = false;

        private void OnSingleEvent(Action action)
        {
            if (!single)
            {
                action();
                single = true;
            }
        }
    }
}
using OneFlyLib;
using UnityEngine;

namespace Universal.Card
{
    public enum CardOrientation { Unknow, CardUp, CardDown, CardLeft, CardRight } //卡牌朝向

    /// <summary>
    /// 卡牌UI的控制
    /// </summary>
    public class CardControl : UIParent
    {
        public GameObject left;
        public GameObject left_select;

        public GameObject right;
        public GameObject right_select;
        private GameObject model;

        //[HideInInspector]
        //public bool PointLock;
        private CardOrientation currentOrientation = CardOrientation.Unknow;

        protected override void Awake()
        {
            base.Awake();
            //  PointLock = true;
        }

        /// <summary>
        /// 当前卡牌UI的初始化
        /// </summary>
        public override void InitUI()
        {
            base.InitUI();
            //eg:
            //    model = GetModel();
            //    maxAngle = 90;
            //    minAngle = -90;
        }

        /// <summary>
        /// UI的入场动画（若有,只执行一次）
        /// </summary>
        public override void SetTween()
        {
            base.SetTween();
        }

        /// <summary>
        /// 通过获取的位置处理事件（此位置是卡牌UI的位置）
        /// </summary>
        /// <param name="vec"></param>
        public override void HandlerEventByPos(Vector3 vec)
        {
            //if (!PointLock) return;
            base.HandlerEventByPos(vec);
            //Debugger.Log("UI位置：" + vec.ToString());
        }

        /// <summary>
        /// 根据角度处理事件（此角度大部分时间代表箭头的旋转角度）
        /// </summary>
        /// <param name="angle"></param>
        public override void HandlerEventByAngle(float angle)
        {
            //if (!PointLock) return;

            base.HandlerEventByAngle(angle);
            //Debugger.Log("UI角度：" + angle.ToString());
            //eg:
            //    angle = angle > 180 ? angle - 360 : angle;
            //    float result = Mathf.Clamp(angle, minAngle, maxAngle);
            //    point.transform.localEulerAngles = new Vector3(0, 0, result);
            CardOrientation orientation = ConvertAngleToOrientation(angle);
            SetUIByCardOrientation(orientation);
            if (currentOrientation != orientation)
            {
                currentOrientation = orientation;

                //当达到触发选项时弹回
                if ((currentOrientation == CardOrientation.CardLeft && left.activeInHierarchy) ||
                    (currentOrientation == CardOrientation.CardRight && right.activeInHierarchy))
                {
                    if (left_select)
                        left_select.SetActive(false);
                    if (right_select)
                        right_select.SetActive(false);
                    //重置
                    ManagerEvent.Call(orientation.ToString(), name);
                    UIReset();
                    currentOrientation = CardOrientation.CardUp;
                }
            }
        }

        private CardOrientation ConvertAngleToOrientation(float angle)
        {
            CardOrientation orientation = CardOrientation.Unknow;

            if ((347f <= angle && angle <= 360f) || (0f <= angle && angle <= 13f)) // +-13
            {
                orientation = CardOrientation.CardUp;
            }
            else if (77f <= angle && angle <= 103f && right != null && right.activeInHierarchy) // 90 +- 13
            {
                orientation = CardOrientation.CardRight;
            }
            else if (167f <= angle && angle <= 193f) // 180+-13
            {
                orientation = CardOrientation.CardDown;
            }
            else if (257f <= angle && angle <= 283f && left != null && left.activeInHierarchy) // 270 +- 13
            {
                orientation = CardOrientation.CardLeft;
            }

            return orientation;
        }

        private void SetUIByCardOrientation(CardOrientation orientation)
        {
            switch (orientation)
            {
                case CardOrientation.CardRight:
                    if (right_select != null) right_select.SetActive(true);
                    break;

                case CardOrientation.CardLeft:
                    if (left_select != null) left_select.SetActive(true);
                    break;

                case CardOrientation.CardUp:
                case CardOrientation.CardDown:
                case CardOrientation.Unknow:
                    if (left_select != null) left_select.SetActive(false);
                    if (right_select != null) right_select.SetActive(false);
                    break;
            }
        }

        /// <summary>
        /// UI的出场动画（若有,只执行一次）
        /// </summary>
        public override void SetTweenBack()
        {
            base.SetTweenBack();
        }

        /// <summary>
        /// 卡牌UI移除的重置（只执行一次）
        /// </summary>
        public override void UIReset()
        {
            base.UIReset();
        }
    }
}
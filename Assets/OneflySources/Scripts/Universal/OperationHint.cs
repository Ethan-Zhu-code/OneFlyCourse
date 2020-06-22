using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using DevelopEngine;
using Universal.Audio;

namespace Universal
{
    public class OperationHint : MonoSingleton<OperationHint>
    {
        private string[] operateInfo ={
        "放置开始卡牌，开始课程。",
        "请按照提示依次摆放雄鸭嘴兽、雌鸭嘴兽、缩放和环境卡牌到对应位置。",
        "向右旋转缩放功能卡牌，观察雄鸭嘴兽和雌鸭嘴兽的外形特征。",
        "将环境卡牌放在雄鸭嘴兽旁，可见鸭嘴兽生存环境，学习鸭嘴兽生存环境的特点。",
        "现在请观察雌鸭嘴兽、雄鸭嘴兽，你能发现两者有什么不同吗？",
        "向右旋转雄鸭嘴兽卡牌，观察雄鸭嘴兽的尖刺和毒腺位置。",
        "请先收起雄鸭嘴兽，缩放和环境卡牌，摆放打洞、睡觉、觅食卡牌，保留雌鸭嘴兽卡牌。",
        "向右旋转打洞卡牌，观察鸭嘴兽的打洞的过程。",
        "向右旋转睡觉卡牌，观察睡着的鸭嘴兽。",
        "向右旋转觅食卡牌，观察鸭嘴兽捕食的过程。",
        "请先收起打洞、睡觉、觅食卡牌，再按照提示摆放雌鸭嘴兽,繁殖卡牌到对应位置。",
        "向右旋转繁殖卡牌，可见雌鸭嘴兽产卵的繁殖方式。",
        "向左旋转繁殖卡牌，可见雌鸭嘴兽喂养鸭嘴兽宝宝的画面。",
        "放入并旋转课程控制卡牌，可以选择重新开始或者退出课程。"
    };

        public Text text;
        private string currInfo;
        private RectTransform backgroundTransform;
        float bgHeight;
        public HintType hintType = HintType.LOCAL;

        public string[] OperateInfo
        {
            get
            {
                return operateInfo;
            }

            set
            {
                operateInfo = value;
            }
        }

        public void Awake()
        {
            backgroundTransform = transform.Find("background").GetComponent<RectTransform>();
            bgHeight = backgroundTransform.sizeDelta.y;
        }
        private void Start()
        {
        }

        /// <summary>
        /// 显示操作提示
        /// </summary>
        /// <param name="index"></param>
        public void ShowOperationHint(int index)
        {
            
            bgHeight = backgroundTransform.sizeDelta.y;

            switch (hintType)
            {
                case HintType.LOCAL:
                    text.text = operateInfo[index];
                    break;
                case HintType.LOAD:
                    text.text = Configuration.GetContent("OperationHint", index.ToString());
                    break;
            }

            backgroundTransform.sizeDelta = new Vector2(backgroundTransform.sizeDelta.x, GetLineCount() * 30 + 85);
            //transform.localPosition = new Vector2(transform.localPosition.x, (bgHeight - backgroundTransform.sizeDelta.y) / 2 + transform.localPosition.y);
            transform.DOLocalMoveX(645f, 1);
        }

        public void ShowOperationHint(string content)
        {
            if (content == "")
                return;
            bgHeight = backgroundTransform.sizeDelta.y;
            text.text = content;
            backgroundTransform.sizeDelta = new Vector2(backgroundTransform.sizeDelta.x, GetLineCount() * 30 + 85);
            transform.DOLocalMoveX(645f, 1);
            //gameObject.transform.localPosition = new Vector2(transform.localPosition.x, (bgHeight - backgroundTransform.sizeDelta.y) / 2 + transform.localPosition.y);
        }

        public void CloseOperationHint()
        {
            transform.DOLocalMoveX(-645f, 1);
        }

        int GetLineCount()
        {
            string strText = text.text;
            text.text = "";
            float linrHeight = text.preferredHeight;
            text.text = strText;

            return (int)(text.preferredHeight / 23f);
        }

    }
    
}

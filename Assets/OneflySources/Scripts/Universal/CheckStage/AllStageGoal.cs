using OneFlyLib;

namespace Universal.CheckStage
{
    public class StageGoal_CheckCardStr : StageGoal
    {
        private string cardName;

        public StageGoal_CheckCardStr(string msg, string cardName) : base(msg)
        {
            this.cardName = cardName;
        }

        public override void SetValue(params object[] args)
        {
            if (args != null && args.Length == 1 && args[0] is SceneObj)
            {
                string name = ((SceneObj)args[0]).ui.name;
                if (cardName.Equals(name))
                {
                    NowValue = MaxValue;
                }
            }
        }
    }

    public class CheckMsg_bool_string : StageGoal
    {
        private bool bValue;
        private string sValue;

        public CheckMsg_bool_string(string msg, bool boolValue, string strValue) : base(msg)
        {
            bValue = boolValue;
            sValue = strValue;
        }

        public override void SetValue(params object[] args)
        {
            if (args != null && args.Length == 2 && args[0] is bool && args[1] is string)
            {
                bool boolValue = (bool)args[0];
                string strValue = (string)args[1];

                if (sValue.Equals(strValue))
                {
                    bool result = bValue == boolValue;
                    NowValue = result ? MaxValue : MaxValue - 1f;                   
                }
            }
        }
    }

    public class StageGoal_CheckAudioIndex : StageGoal
    {
        private int audioIdex;

        public StageGoal_CheckAudioIndex(string msg, int audioIdex) : base(msg)
        {
            this.audioIdex = audioIdex;
        }

        public override void SetValue(params object[] args)
        {
            if (args != null && args.Length == 1 && args[0] is int)
            {
                int index = (int)args[0];
                if (audioIdex == index)
                {
                    NowValue = MaxValue;
                }
            }
        }
    }

    public class StageGoal_CheckAnimationPlay : StageGoal
    {
        private string animationName;

        public StageGoal_CheckAnimationPlay(string msg, string animationName) : base(msg)
        {
            this.animationName = animationName;
        }

        public override void SetValue(params object[] args)
        {
            if (args != null && args.Length == 1 && args[0] is string)
            {
                string animName = (string)args[0];
                if (animationName.Equals(animName))
                {
                    NowValue = MaxValue;
                }
            }
        }
    }
}
namespace TsubakiTranslator.BasicLibrary
{
    public class GameData
    {

        /// <summary>
        /// 游戏名（非进程名，但在游戏名未知的情况下先使用进程名替代）
        /// </summary>
        public string GameName
        {
            get; set;
        }

        /// <summary>
        /// 游戏文件路径
        /// </summary>
        public string GamePath
        {
            get; set;
        }


        /// <summary>
        /// 特殊码值，仅在hook模式有效
        /// </summary>
        public string HookCode
        {
            get; set;
        }
        /// <summary>
        /// 文本重复次数
        /// </summary>
        public int DuplicateTimes { get; set; }
    }
}
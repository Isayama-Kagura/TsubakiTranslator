using CommunityToolkit.Mvvm.ComponentModel;

namespace TsubakiTranslator.BasicLibrary
{
    public class GameData : ObservableObject
    {

        /// <summary>
        /// 游戏名（非进程名，但在游戏名未知的情况下先使用进程名替代）
        /// </summary>
        private string gameName;


        /// <summary>
        /// 特殊码值，仅在hook模式有效
        /// </summary>
        private string hookCode;


        /// <summary>
        /// 文本重复次数
        /// </summary>
        private int duplicateTimes;

        public string GameName
        {
            get => gameName;
            set => SetProperty(ref gameName, value);
        }
        public string HookCode
        {
            get => hookCode;
            set => SetProperty(ref hookCode, value);
        }
        public int DuplicateTimes
        {
            get => duplicateTimes;
            set => SetProperty(ref duplicateTimes, value);
        }

    }
}
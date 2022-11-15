using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace TsubakiTranslator.BasicLibrary
{
    public partial class RegexRuleData : ObservableObject
    {
        [ObservableProperty]
        private string sourceRegex;
        [ObservableProperty]
        private string destinationRegex;

        public RegexRuleData(string sourceRegex, string destinationRegex)
        {
            this.sourceRegex = sourceRegex;
            this.destinationRegex = destinationRegex;
        }

    }

    public partial class GameData : ObservableObject
    {
        [ObservableProperty]
        /// <summary>
        /// 游戏名（非进程名，但在游戏名未知的情况下先使用进程名替代）
        /// </summary>
        private string gameName;

        [ObservableProperty]
        /// <summary>
        /// 游戏名（非进程名，但在游戏名未知的情况下先使用进程名替代）
        /// </summary>
        private string processName;

        [ObservableProperty]
        /// <summary>
        /// 特殊码值，仅在hook模式有效
        /// </summary>
        private string hookCode;

        [ObservableProperty]
        /// <summary>
        /// 文本重复次数
        /// </summary>
        private int duplicateTimes;

        public ObservableCollection<RegexRuleData> RegexRuleItems { get; set; }

        public GameData()
        {
            RegexRuleItems = new ObservableCollection<RegexRuleData>();
        }

    }

    public partial class GamesConfig : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<GameData> gameDatas;
        [ObservableProperty]
        private ObservableCollection<RegexRuleData> clipBoardRegexRules;


        public GamesConfig()
        {
            GameDatas = new ObservableCollection<GameData>();
            ClipBoardRegexRules = new ObservableCollection<RegexRuleData>();
        }



    }
}
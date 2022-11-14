using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace TsubakiTranslator.BasicLibrary
{
    public class RegexRuleData : ObservableObject
    {
        private string sourceRegex;
        private string destinationRegex;

        public RegexRuleData(string sourceRegex, string destinationRegex)
        {
            this.sourceRegex = sourceRegex;
            this.destinationRegex = destinationRegex;
        }

        public string SourceRegex
        {
            get => sourceRegex;
            set => SetProperty(ref sourceRegex, value);
        }

        public string DestinationRegex
        {
            get => destinationRegex;
            set => SetProperty(ref destinationRegex, value);
        }
    }

    public class GameData : ObservableObject
    {

        /// <summary>
        /// 游戏名（非进程名，但在游戏名未知的情况下先使用进程名替代）
        /// </summary>
        private string gameName;

        /// <summary>
        /// 游戏名（非进程名，但在游戏名未知的情况下先使用进程名替代）
        /// </summary>
        private string processName;


        /// <summary>
        /// 特殊码值，仅在hook模式有效
        /// </summary>
        private string hookCode;


        /// <summary>
        /// 文本重复次数
        /// </summary>
        private int duplicateTimes;

        public ObservableCollection<RegexRuleData> RegexRuleItems { get; set; }

        public GameData()
        {
            RegexRuleItems = new ObservableCollection<RegexRuleData>();
        }

        public string GameName
        {
            get => gameName;
            set => SetProperty(ref gameName, value);
        }

        public string ProcessName
        {
            get => processName;
            set => SetProperty(ref processName, value);
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

    public class GamesConfig : ObservableObject
    {
        private ObservableCollection<GameData> gameDatas;
        private ObservableCollection<RegexRuleData> clipBoardRegexRules;


        public GamesConfig()
        {
            GameDatas = new ObservableCollection<GameData>();
            ClipBoardRegexRules = new ObservableCollection<RegexRuleData>();
        }

        public ObservableCollection<GameData> GameDatas
        {
            get => gameDatas;
            set => SetProperty(ref gameDatas, value);
        }
        public ObservableCollection<RegexRuleData> ClipBoardRegexRules
        {
            get => clipBoardRegexRules;
            set => SetProperty(ref clipBoardRegexRules, value);
        }


    }
}
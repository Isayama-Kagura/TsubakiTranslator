using MaterialDesignThemes.Wpf;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using TsubakiTranslator.BasicLibrary;


namespace TsubakiTranslator
{
    /// <summary>
    /// UserConfig.xaml 的交互逻辑
    /// </summary>
    public partial class UserGamePage : UserControl
    {

        public GamesConfig GamesConfig { get; }
        private ObservableCollection<string> ProcessStrings { get; }

        public UserGamePage()
        {
            InitializeComponent();

            GamesConfig = FileHandler.DeserializeObject<GamesConfig>(System.AppDomain.CurrentDomain.BaseDirectory + @"config/GamesData.json", new GamesConfig());
            ProcessStrings = new ObservableCollection<string>();

            GameList.ItemsSource = GamesConfig.GameDatas;
            ClipboardRegexDataGrid.ItemsSource = GamesConfig.ClipBoardRegexRules;
            GameProcessList.ItemsSource = ProcessStrings;
            HistoryGameProcessList.ItemsSource = ProcessStrings;

        }


        private void DeleteGame_Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            GameData item = (GameData)GameList.SelectedItem;
            GamesConfig.GameDatas.Remove(item);
        }

        private void OpenHistoryGame_Button_Click(object sender, RoutedEventArgs e)
        {
            SetProcessItems();

            GameData item = (GameData)GameList.SelectedItem;

            //刷新上下文
            HistoryGameInfo.DataContext = null;
            HistoryGameInfo.DataContext = item;

            //HistoryGameName.Text = item.GameName;
            //HistoryGameHookCode.Text = item.HookCode;
            //HistoryGameDuplicateTimes.Text = item.DuplicateTimes.ToString();

        }

        //历史游戏记录中打开游戏
        private async void AcceptGame_Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            GameData item = (GameData)GameList.SelectedItem;

            item.GameName = HistoryGameName.Text;
            item.HookCode = HistoryHookCode.Text;
            int.TryParse(HistoryDuplicateTimes.Text, out int times);
            item.DuplicateTimes = times;

            string processInfo = (string)HistoryGameProcessList.SelectedItem;

            if (processInfo == null)
                return;

            Process gameProcess = GetGameProcessByProcessString(processInfo);

            TextHookHandler textHookHandler = new TextHookHandler(gameProcess);


            LinkedList<RegexRuleData> regexRules = new LinkedList<RegexRuleData>();
            foreach (var rule in item.RegexRuleItems)
                regexRules.AddLast(rule);
            SourceTextHandler sourceTextHandler = new SourceTextHandler(item.DuplicateTimes, regexRules);

            Window mainWindow = Window.GetWindow(this);
            mainWindow.Hide();
            TranslateWindow translateWindow = new TranslateWindow(mainWindow, textHookHandler, sourceTextHandler);
            translateWindow.Show();


            if (item.HookCode.Trim().Length != 0)
                await textHookHandler.AttachProcessByHookCode(item.HookCode);

        }

        private void OpenGameByPid_Button_Click(object sender, RoutedEventArgs e)
        {
            SetProcessItems();
        }

        //注入进程打开游戏
        private async void AcceptProcess_Button_Click(object sender, RoutedEventArgs e)
        {
            string processInfo = (string)GameProcessList.SelectedItem;
            if (processInfo == null)
                return;

            Process gameProcess = GetGameProcessByProcessString(processInfo);

            int.TryParse(GameProcessDuplicateTimes.Text, out int times);

            GameData item = new GameData
            {
                HookCode = GameProcessHookCode.Text,
                DuplicateTimes = times,
                GameName = gameProcess.ProcessName
            };

            GamesConfig.GameDatas.Add(item);

            TextHookHandler textHookHandler = new TextHookHandler(gameProcess);

            SourceTextHandler sourceTextHandler = new SourceTextHandler(times, new LinkedList<RegexRuleData>());

            Window mainWindow = Window.GetWindow(this);
            mainWindow.Hide();
            TranslateWindow translateWindow = new TranslateWindow(mainWindow, textHookHandler, sourceTextHandler);
            translateWindow.Show();


            if (GameProcessHookCode.Text != null && GameProcessHookCode.Text.Trim().Length != 0)
                await textHookHandler.AttachProcessByHookCode(GameProcessHookCode.Text);
        }

        private void GamePage_DialogClosing(object sender, MaterialDesignThemes.Wpf.DialogClosingEventArgs eventArgs)
        {
            ProcessStrings.Clear();
        }

        private void SetProcessItems()
        {
            Process[] ps = Process.GetProcesses();

            foreach (Process p in ps)
                ProcessStrings.Add($"{p.ProcessName} - {p.Id}");
        }

        private Process GetGameProcessByProcessString(string processString)
        {
            Regex reg = new Regex(@"\s-\s(\d+?)$");
            Match match = reg.Match(processString);

            int.TryParse(match.Groups[1].Value, out int pid);

            Process gameProcess = Process.GetProcessById(pid);

            return gameProcess;
        }

        private void AddRegexRule_Button_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var data = btn.DataContext as GameData;

            data.RegexRuleItems.Add(new RegexRuleData("",""));
        }

        private void RemoveRegexRule_Button_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var data = btn.DataContext as GameData;

            data.RegexRuleItems.Clear();
        }

        private void MonitorClipBoard_Button_Click(object sender, RoutedEventArgs e)
        {
            LinkedList<RegexRuleData> regexRules = new LinkedList<RegexRuleData>();
            foreach (var rule in GamesConfig.ClipBoardRegexRules)
                regexRules.AddLast(rule);
            SourceTextHandler sourceTextHandler = new SourceTextHandler(1, regexRules);


            Window mainWindow = Window.GetWindow(this);
            mainWindow.Hide();
            TranslateWindow translateWindow = new TranslateWindow(mainWindow, sourceTextHandler);
            translateWindow.Show();
        }

        private void Clipboard_AddRegexRule_Button_Click(object sender, RoutedEventArgs e)
        {
            GamesConfig.ClipBoardRegexRules.Add(new RegexRuleData("", ""));
        }

        private void Clipboard_RemoveRegexRule_Button_Click(object sender, RoutedEventArgs e)
        {
            GamesConfig.ClipBoardRegexRules.Clear();
        }
    }
  

}

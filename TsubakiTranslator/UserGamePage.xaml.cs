using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
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

        //public GamesConfig GamesConfig { get; }

        private class GameProcess
        {
            public int PID { get; set; }
            public string ProcessName { get; set; }
            public string ProcessDetail { get; set; }
        }

        public UserGamePage()
        {
            InitializeComponent();

            this.DataContext = App.GamesConfig;
        }


        private void DeleteGame_Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            GameData item = (GameData)GameList.SelectedItem;
            App.GamesConfig.GameDatas.Remove(item);
        }

        private void OpenHistoryGame_Button_Click(object sender, RoutedEventArgs e)
        {
            SetProcessItems();

            GameData item = (GameData)GameList.SelectedItem;

            //刷新上下文
            HistoryGameInfo.DataContext = null;
            HistoryGameInfo.DataContext = item;

        }

        //历史游戏记录中打开游戏
        private void AcceptGame_Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            GameData item = (GameData)GameList.SelectedItem;

            item.GameName = HistoryGameName.Text;
            item.HookCode = HistoryHookCode.Text;
            int.TryParse(HistoryDuplicateTimes.Text, out int times);
            item.DuplicateTimes = times;

            GameProcess processInfo = (GameProcess)HistoryGameProcessList.SelectedItem;

            if (processInfo == null)
                return;

            item.ProcessName = processInfo.ProcessName;

            Process gameProcess = Process.GetProcessById(processInfo.PID);

            TextHookHandler textHookHandler;
            if (item.HookCode != null && item.HookCode.Trim().Length != 0)
                textHookHandler = new TextHookHandler(gameProcess, item.HookCode);
            else
                textHookHandler = new TextHookHandler(gameProcess, null);

            LinkedList<RegexRuleData> regexRules = new LinkedList<RegexRuleData>();
            foreach (var rule in item.RegexRuleItems)
                regexRules.AddLast(rule);
            SourceTextHandler sourceTextHandler = new SourceTextHandler(item.DuplicateTimes, regexRules);

            Window mainWindow = Window.GetWindow(this);
            mainWindow.Hide();
            TranslateWindow translateWindow = new TranslateWindow(mainWindow, textHookHandler, sourceTextHandler);
            translateWindow.Show();

        }

        private void OpenGameByPid_Button_Click(object sender, RoutedEventArgs e)
        {
            SetProcessItems();
        }

        //注入进程打开游戏
        private void AcceptProcess_Button_Click(object sender, RoutedEventArgs e)
        {
            GameProcess processInfo = (GameProcess)GameProcessList.SelectedItem;
            if (processInfo == null)
                return;

            Process gameProcess = Process.GetProcessById(processInfo.PID);

            int.TryParse(GameProcessDuplicateTimes.Text, out int times);

            var result = from data in App.GamesConfig.GameDatas
                         where data.ProcessName.Equals(gameProcess.ProcessName)
                         select data;

            if (result.Count() == 0)
            {
                GameData item = new GameData
                {
                    HookCode = GameProcessHookCode.Text,
                    DuplicateTimes = times,
                    GameName = Path.GetFileName(gameProcess.MainModule.FileVersionInfo.FileName),
                    ProcessName = gameProcess.ProcessName
                };

                App.GamesConfig.GameDatas.Add(item);
            }

            TextHookHandler textHookHandler;
            if (GameProcessHookCode.Text != null && GameProcessHookCode.Text.Trim().Length != 0)
                textHookHandler = new TextHookHandler(gameProcess, GameProcessHookCode.Text);
            else
                textHookHandler = new TextHookHandler(gameProcess, null);

            SourceTextHandler sourceTextHandler = new SourceTextHandler(times, new LinkedList<RegexRuleData>());

            Window mainWindow = Window.GetWindow(this);
            mainWindow.Hide();
            TranslateWindow translateWindow = new TranslateWindow(mainWindow, textHookHandler, sourceTextHandler);
            translateWindow.Show();

        }


        private void SetProcessItems()
        {
            Process[] ps = Process.GetProcesses();
            List<GameProcess> list = new List<GameProcess>();

            foreach (Process p in ps)
            {
                GameProcess gameProcess = new GameProcess { PID = p.Id, ProcessName = p.ProcessName, ProcessDetail = $"{p.ProcessName} - {p.Id}" };
                list.Add(gameProcess);
            }


            list.Sort((x, y) => string.Compare(x.ProcessName, y.ProcessName));
            ObservableCollection<GameProcess> processStrings = new ObservableCollection<GameProcess>(list);
            GameProcessList.ItemsSource = processStrings;
            HistoryGameProcessList.ItemsSource = processStrings;
        }


        private void AddRegexRule_Button_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var data = btn.DataContext as GameData;

            data.RegexRuleItems.Add(new RegexRuleData("", ""));
        }

        private void RemoveRegexRule_Button_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var data = btn.DataContext as GameData;

            data.RegexRuleItems.Clear();
        }

        //剪切板模式
        private void MonitorClipBoard_Button_Click(object sender, RoutedEventArgs e)
        {
            LinkedList<RegexRuleData> regexRules = new LinkedList<RegexRuleData>();
            foreach (var rule in App.GamesConfig.ClipBoardRegexRules)
                regexRules.AddLast(rule);
            SourceTextHandler sourceTextHandler = new SourceTextHandler(1, regexRules);


            Window mainWindow = Window.GetWindow(this);
            mainWindow.Hide();
            TranslateWindow translateWindow = new TranslateWindow(mainWindow, sourceTextHandler);
            translateWindow.Show();
        }

        private void Clipboard_AddRegexRule_Button_Click(object sender, RoutedEventArgs e)
        {
            App.GamesConfig.ClipBoardRegexRules.Add(new RegexRuleData("", ""));
        }

        private void Clipboard_RemoveRegexRule_Button_Click(object sender, RoutedEventArgs e)
        {
            App.GamesConfig.ClipBoardRegexRules.Clear();
        }


        //OCR模式
        private void OcrModeConfirm_Button_Click(object sender, RoutedEventArgs e)
        {
            Window mainWindow = Window.GetWindow(this);
            mainWindow.Hide();
            TranslateWindow translateWindow = new TranslateWindow(mainWindow);
            translateWindow.Show();
        }

        [SupportedOSPlatform("windows10.0.10240")]
        private void Ocr_Button_Click(object sender, RoutedEventArgs e)
        {
            OcrTipsPanel.Children.Clear();

            var languagesName = OcrProgram.GetSupportedLanguages();
            foreach (string lang in languagesName)
            {
                TextBlock tb = new TextBlock();
                tb.Text = lang;
                OcrTipsPanel.Children.Add(tb);
            }
        }
    }


}

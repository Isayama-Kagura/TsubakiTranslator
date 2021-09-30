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

        public ObservableCollection<GameData> GameItems { get; }
        private ObservableCollection<string> ProcessItems { get; }


        public UserGamePage()
        {
            InitializeComponent();

            GameItems = FileHandler.DeserializeObject<ObservableCollection<GameData>>(System.AppDomain.CurrentDomain.BaseDirectory + @"config/GameData.json", new ObservableCollection<GameData>());
            ProcessItems = new ObservableCollection<string>();

            GameList.ItemsSource = GameItems;
            GameProcessList.ItemsSource = ProcessItems;

        }

        private void Config_DialogHost_OnDialogClosing(object sender, MaterialDesignThemes.Wpf.DialogClosingEventArgs eventArgs)
        {
            this.InputGameName.Text = "";
            this.InputGamePath.Text = "";
            this.InputHookCode.Text = "";
        }

        private void AcceptGame_Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            int.TryParse(InputDuplicateTimes.Text, out int num);
            GameData item = new GameData
            {
                GameName = InputGameName.Text,
                GamePath = InputGamePath.Text,
                HookCode = InputHookCode.Text,
                DuplicateTimes = num
            };

            GameItems.Add(item);
        }

        private void DeleteGame_Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            GameData item = (GameData)GameList.SelectedItem;
            GameItems.Remove(item);
        }

        private void SelectPath_Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.InputGamePath.Text = FileHandler.SelectPath();
        }

        private async void OpenGame_Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            
            GameData item = (GameData)GameList.SelectedItem;
            Process gameProcess = FileHandler.OpenGame(item.GamePath, UserConfigPage.TranslateAPIConfig.LEIsEnabled ? UserConfigPage.TranslateAPIConfig.LEPath : null);
            TextHookHandler textHookHandler = new TextHookHandler(gameProcess, item.DuplicateTimes);

            if (item.HookCode != null && item.HookCode.Trim().Length != 0)
                await textHookHandler.AttachProcessByHookCode(item.HookCode);

            Window mainWindow = Window.GetWindow(this);
            mainWindow.Hide();
            TranslateWindow translateWindow = new TranslateWindow(mainWindow, textHookHandler);
            translateWindow.Show();

        }

        private void OpenGameByPid_Button_Click(object sender, RoutedEventArgs e)
        {
            ProcessItems.Clear();
            Process[] ps = Process.GetProcesses();
            foreach(Process p in ps)
            {
                ProcessItems.Add($"{p.Id} - {p.ProcessName}");
            }

        }

        private async void AcceptProcess_Button_Click(object sender, RoutedEventArgs e)
        {
            string processInfo = (string)GameProcessList.SelectedItem;

            Regex reg = new Regex(@"(\d+?)\s-\s");
            Match match = reg.Match(processInfo);

            int.TryParse(match.Groups[1].Value, out int pid);

            Process gameProcess = Process.GetProcessById(pid);

            int.TryParse(GameProcessDuplicateTimes.Text, out int times);
            
            TextHookHandler textHookHandler = new TextHookHandler(gameProcess, times);

            if (GameProcessHookCode.Text != null && GameProcessHookCode.Text.Trim().Length != 0)
                await textHookHandler.AttachProcessByHookCode(GameProcessHookCode.Text);

            Window mainWindow = Window.GetWindow(this);
            mainWindow.Hide();
            TranslateWindow translateWindow = new TranslateWindow(mainWindow, textHookHandler);
            translateWindow.Show();

        }

    }
  

}

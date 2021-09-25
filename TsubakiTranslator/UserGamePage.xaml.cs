using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
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

        public ObservableCollection<GameData> Items { get; }


        public UserGamePage()
        {
            InitializeComponent();

            Items = FileHandler.DeserializeObject<ObservableCollection<GameData>>(System.AppDomain.CurrentDomain.BaseDirectory + @"config/GameData.json", new ObservableCollection<GameData>());

            GameList.ItemsSource = Items;
        }

        private void Config_DialogHost_OnDialogClosing(object sender, MaterialDesignThemes.Wpf.DialogClosingEventArgs eventArgs)
        {
            this.InputGameName.Text = "";
            this.InputGamePath.Text = "";
            this.InputHookCode.Text = "";
        }

        private void AddGame_OnDialogClosing(object sender, System.Windows.RoutedEventArgs e)
        {
            int.TryParse(InputDuplicateTimes.Text, out int num);
            GameData item = new GameData
            {
                GameName = InputGameName.Text,
                GamePath = InputGamePath.Text,
                HookCode = InputHookCode.Text,
                DuplicateTimes = num
            };

            Items.Add(item);
        }

        private void DeleteGame_Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            GameData item = (GameData)GameList.SelectedItem;
            Items.Remove(item);
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

            if (item.HookCode != null && item.HookCode.Length != 0)
                await textHookHandler.AttachProcessByHookCode(item.HookCode);

            Window mainWindow = Window.GetWindow(this);
            mainWindow.Hide();
            TranslateWindow translateWindow = new TranslateWindow(mainWindow, textHookHandler);
            translateWindow.Show();

        }
    }
  

}

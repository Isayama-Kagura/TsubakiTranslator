using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using TsubakiTranslator.BasicLibrary;

namespace TsubakiTranslator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow:Window
    {
        private UserGamePage UserGamePage { get; }

        private UserConfigPage UserConfigPage { get; }

        private AboutMePage AboutMePage { get; }

        private static WindowConfig windowConfig;
        public static WindowConfig WindowConfig { get => windowConfig ; }

        public MainWindow()
        {
            InitializeComponent();

            UserGamePage = new UserGamePage();
            UserConfigPage = new UserConfigPage();
            AboutMePage = new AboutMePage();


            windowConfig = FileHandler.DeserializeObject<WindowConfig>(System.AppDomain.CurrentDomain.BaseDirectory + @"config/WindowConfig.json" ,new WindowConfig());

            this.Width = WindowConfig.MainWindowWidth;
            this.Height = WindowConfig.MainWindowHeight;
            this.DataContext = WindowConfig;

            MyGame.IsSelected = true;
        }

        private void ColorZone_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            base.DragMove();//实现整个窗口的拖动
        }


        private void ListViewItem_UserGame_Selected(object sender, RoutedEventArgs e)
        {
            MainPageContent.Content = UserGamePage;
        }

        private void ListViewItem_UserConfig_Selected(object sender, RoutedEventArgs e)
        {
            MainPageContent.Content = UserConfigPage;
        }


        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            //this.Hide();
            WindowState = WindowState.Minimized;
        }

        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Maximized;
        }

        private void RestoreButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Normal;
        }

        private void MainWindow_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                WindowMaximizeButton.Visibility = Visibility.Collapsed;
                WindowRestoreButton.Visibility = Visibility.Visible;
            }
            if ( this.WindowState == WindowState.Normal)
            {
                WindowMaximizeButton.Visibility = Visibility.Visible;
                WindowRestoreButton.Visibility = Visibility.Collapsed;
            }

        }

        private void On_MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            WindowConfig.MainWindowHeight = this.Height;
            WindowConfig.MainWindowWidth = this.Width;

            FileHandler.SerializeObject<WindowConfig>(WindowConfig, System.AppDomain.CurrentDomain.BaseDirectory + @"config/WindowConfig.json");
            FileHandler.SerializeObject<ObservableCollection<GameData>>(UserGamePage.GameItems, System.AppDomain.CurrentDomain.BaseDirectory + @"config/GameData.json");
            FileHandler.SerializeObject<TranslateAPIConfig>(UserConfigPage.TranslateAPIConfig, System.AppDomain.CurrentDomain.BaseDirectory + @"config/APIConfig.json");


        }

        private void ListViewItem_AboutMe_Selected(object sender, RoutedEventArgs e)
        {
            MainPageContent.Content = AboutMePage;
        }

        private void ColorZone_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            WindowState = WindowState.Maximized;
        }
    }
}

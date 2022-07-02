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
        private WinStylePage WinStylePage { get; }

        public MainWindow()
        {
            InitializeComponent();

            UserGamePage = new UserGamePage();
            UserConfigPage = new UserConfigPage();
            AboutMePage = new AboutMePage();
            WinStylePage = new WinStylePage();

            this.DataContext = App.WindowConfig;

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



        private void ListViewItem_AboutMe_Selected(object sender, RoutedEventArgs e)
        {
            MainPageContent.Content = AboutMePage;
        }

        private void ColorZone_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
                WindowState = WindowState.Normal;
            else if (this.WindowState == WindowState.Normal)
                WindowState = WindowState.Maximized;
        }

        private void ListViewItem_WinStyle_Selected(object sender, RoutedEventArgs e)
        {
            MainPageContent.Content = WinStylePage;
        }
    }
}

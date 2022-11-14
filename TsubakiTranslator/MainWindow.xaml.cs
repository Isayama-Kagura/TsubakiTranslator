using System.Windows;
using System.Windows.Input;

namespace TsubakiTranslator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = App.WindowConfig;

        }

        private void ColorZone_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            base.DragMove();//实现整个窗口的拖动
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

        private void ColorZone_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
                WindowState = WindowState.Normal;
            else if (this.WindowState == WindowState.Normal)
                WindowState = WindowState.Maximized;
        }

    }
}

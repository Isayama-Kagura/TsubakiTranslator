using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TsubakiTranslator.BasicLibrary;

namespace TsubakiTranslator
{
    /// <summary>
    /// TranslateWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TranslateWindow : Window
    {
        private Window mainWindow { get; set; }
        private TranslatedResultDisplay TranslatedResultDisplay { get; set; }
        private HookResultDisplay HookResultDisplay { get; set; }
        private TextHookHandler textHookHandler;
        public TextHookHandler TextHookHandler { get => textHookHandler; }


        public TranslateWindow(Window mainWindow, TextHookHandler textHookHandler)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            this.textHookHandler = textHookHandler;

            this.Height = MainWindow.windowConfig.TranslateWindowHeight;
            this.Width = MainWindow.windowConfig.TranslateWindowWidth;
            this.Left = MainWindow.windowConfig.TranslateWindowLeft;
            this.Top = MainWindow.windowConfig.TranslateWindowTop;
            this.Topmost = MainWindow.windowConfig.TranslateWindowTopmost;
            if (this.Topmost)
            {
                PinButton.Visibility = Visibility.Visible;
                PinOffButton.Visibility = Visibility.Collapsed;
            }


            //注意顺序，Hook窗口的事件处理先于Translate结果窗口
            HookResultDisplay = new HookResultDisplay(this);
            TranslatedResultDisplay = new TranslatedResultDisplay(textHookHandler);

            HookDisplayButton.IsChecked = true;

        }

        //供Hook文本选择界面使用
        public void SwitchToTranslateDisplay()
        {
            //翻译当前选择的文本
            App.Current.Dispatcher.Invoke(new Action<object, DataReceivedEventArgs>(TranslatedResultDisplay.DisplayTranslateResult),
                new Object(),textHookHandler.LastEventArgs);

            TranslateDisplayButton.IsChecked = true;
        }


        private void On_TranslateWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainWindow.windowConfig.TranslateWindowHeight = this.Height;
            MainWindow.windowConfig.TranslateWindowWidth = this.Width;
            MainWindow.windowConfig.TranslateWindowLeft = this.Left;
            MainWindow.windowConfig.TranslateWindowTop = this.Top;
            MainWindow.windowConfig.TranslateWindowTopmost = this.Topmost;

            mainWindow.Show();
            
        }

        private void TranslateWindow_CloseButton_Click(object sender, RoutedEventArgs e)
        {
            TextHookHandler.ProcessTextractor.OutputDataReceived -= HookResultDisplay.AlterItemInStackPanel;
            TextHookHandler.ProcessTextractor.OutputDataReceived -= TranslatedResultDisplay.DisplayTranslateResult;
            TextHookHandler.CloseTextractor();
            this.Close();
        }

        private void Tranlate_Display_Button_Checked(object sender, RoutedEventArgs e)
        {
            TranslateWindowContent.Content = TranslatedResultDisplay;
        }

        private void Hook_Display_Button_Checked(object sender, RoutedEventArgs e)
        {
            TranslateWindowContent.Content = HookResultDisplay;
        }

        private void ColorZone_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            base.DragMove();//实现整个窗口的拖动
        }

        private void Pin_Button_Click(object sender, RoutedEventArgs e)
        {
            PinButton.Visibility =  Visibility.Collapsed;
            PinOffButton.Visibility = Visibility.Visible;
            this.Topmost = false;
        }

        private void PinOff_Button_Click(object sender, RoutedEventArgs e)
        {
            PinButton.Visibility = Visibility.Visible;
            PinOffButton.Visibility = Visibility.Collapsed;
            this.Topmost = true;
        }

        private void TranslateWindow_MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
    }
}
